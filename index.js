const dataContainer = document.getElementById("data-container");
const departmentsOptions = document.getElementById("departmentsOptions");
const supervisorOptions = document.getElementById("supervisorOptions");
const sbmtBtn = document.getElementById("submit");
const nameInp = document.getElementById("name");
const addressInp = document.getElementById("address");
const ageInp = document.getElementById("age");
const idInp = document.getElementById("id");

let data = [];
let departmentList = [];

const getData = async () => {
  let url = `https://localhost:7097/api/Student?page=1&limit=13`;
  let response = await fetch(url);
  let res = await response.json();
  data = res;
  displayData(data);
  createSuperVisorOptions(data);
};

const displayData = (arr) => {
  let container = ``;
  arr.forEach((element) => {
    let tmp = `<tr>
            <td>
              ${element.st_Id}
            </td>
            <td>
                ${element.st_Fname}
            </td>
            <td>
                ${element.st_Address}
            </td>
            <td>
                ${element.st_Age}
            </td>
            <td>
                ${element.deptartment_Name}
            </td>
            <td>
                ${element.supervisor_Name}
            </td>
        </tr>`;
    container += tmp;
  });
  dataContainer.innerHTML = container;
};

const getDepartments = async () => {
  let response = await fetch("https://localhost:7097/api/Department");
  let data = await response.json();
  departmentList = data;
  createDepartmentOptiosn(departmentList);
};

const createDepartmentOptiosn = (arr) => {
  let container = ``;
  arr.forEach((element) => {
    let tmp = `<option value="${element.dept_Id}">${element.dept_Name}</option>`;
    container += tmp;
  });
  departmentsOptions.innerHTML = container;
};

const createSuperVisorOptions = (arr) => {
    let container = ``;
    arr.forEach((element) => {
      let tmp = `<option value="${element.st_Id}">${element.st_Fname}</option>`;
      container += tmp;
    });
    supervisorOptions.innerHTML = container;
}

sbmtBtn.addEventListener('click', async() =>{
    var obj = {
        St_Id:idInp.value,
        St_Fname:nameInp.value,
        St_Address:addressInp.value,
        St_Age:ageInp.value,
        Dept_Id:departmentsOptions.value,
        St_super:supervisorOptions.value
    }
    console.log(obj);
    const response = await fetch('https://localhost:7097/api/Student', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(obj),
    });
    if (!response.ok) {
        console.log(response);
    }
    const responseData = await response.json();
    console.log(responseData); 
})




getData();
getDepartments();
