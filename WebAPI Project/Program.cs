
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebAPI_Project.Models;

namespace WebAPI_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ITIContext>(op => op/*.UseLazyLoadingProxies()""*/.UseSqlServer(builder.Configuration.GetConnectionString("iticon")));

            //setup the serialize setting to stop and retrive only the collected data when detecte a cycle refrence error
            //builder.Services.AddControllers().AddJsonOptions(x =>
            //                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles/*after RefrenceHandler u can spcify the depth of serialization by set serializes option to .maxdepth*/);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
