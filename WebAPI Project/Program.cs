
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using WebAPI_Project.Models;
using WebAPI_Project.WorkUnit;


namespace WebAPI_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string AllowedOrigins = "allowedOrigins";
            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "ITI API",
                    Description = "API to manage student and department info",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Ahmed Adly",
                        Email = "adly_ahmed@api.eg"
                    }
                });
                opt.IncludeXmlComments("D:\\ITI(PD&BI)\\API\\labs\\lab 2\\WebAPI Project\\WebAPI Project\\read.xml");

            });

            builder.Services.AddDbContext<ITIContext>(op => op/*.UseLazyLoadingProxies()""*/.UseSqlServer(builder.Configuration.GetConnectionString("iticon")));

            //setup the serialize setting to stop and retrive only the collected data when detecte a cycle refrence error
            //builder.Services.AddControllers().AddJsonOptions(x =>
            //                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles/*after RefrenceHandler u can spcify the depth of serialization by set serializes option to .maxdepth*/);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(AllowedOrigins, builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });
            builder.Services.AddScoped<UnitOfWork>();

            builder.Services.AddAuthentication(op => op.DefaultAuthenticateScheme = "myschema")
                .AddJwtBearer("myschema",
                    opt =>
                    {
                        string key = "This is my Secret Key -Abdo Rehan-";
                        var SecKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
                        opt.TokenValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = SecKey,
                            ValidateLifetime = true,
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    }
                );
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors(AllowedOrigins);

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
