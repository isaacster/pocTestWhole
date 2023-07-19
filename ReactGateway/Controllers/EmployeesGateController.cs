using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PocHomeAssignmentRestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class EmployeesGateController : ControllerBase
    {

        private readonly ILogger<EmployeesGateController> _logger;


        private readonly HttpClient _httpClient;

        public EmployeesGateController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44375"); // Replace with the base URL of the second API
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "xd4f!dfsd@sdf");
        }

        // GetAllEmployees
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            // https://localhost:44375/employees
            // Make a GET request to the second API's endpoint
            HttpResponseMessage response = await _httpClient.GetAsync("/employees");

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content
                var responseData = await response.Content.ReadAsStringAsync();

                // Process the data or return it as needed
                return Ok(responseData);
            }
            else
            {
                // Handle the error response
                return StatusCode((int)response.StatusCode);
            }
        }


        [HttpPost]
        public async Task<int> AddEmployee(Employee employee)
        {

            // Serialize the employee object to JSON
            //string employeeJson = Newtonsoft.Json.JsonConvert.SerializeObject(employee);
            //var content = new StringContent(employeeJson, Encoding.UTF8, "application/json");

            // Make a POST request to the AddEmployee endpoint
            HttpResponseMessage response = await _httpClient.PostAsync("/employees", SerializeEmployee(employee));

            if (response.IsSuccessStatusCode)
            {
                // Retrieve the employee ID from the response content
                string employeeId = await response.Content.ReadAsStringAsync();

                return int.Parse(employeeId);
            }
            else
            {
                // Handle the error response
                throw new Exception($"Failed to add employee. StatusCode: {response.StatusCode}");
            }
        }

        [HttpPut("{id}")]
        public async Task UpdateEmployee(int id, Employee employee)
        {
            // Make a PUT request to the UpdateEmployee endpoint
            HttpResponseMessage response = await _httpClient.PutAsync($"/employees/{id}", SerializeEmployee(employee));

            if (!response.IsSuccessStatusCode)
            {
                // Handle the error response
                throw new Exception($"Failed to update employee. StatusCode: {response.StatusCode}");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetEmployeeById(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"/employees/{id}");

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content
                var responseData = await response.Content.ReadAsStringAsync();

                // Process the data or return it as needed
                return Ok(responseData);
            }
            else
            {
                // Handle the error response
                return StatusCode((int)response.StatusCode);
            }
        }


        [HttpDelete("{id}")]
        public async Task DeleteEmployee(int id)
        {

            // Make a PUT request to the UpdateEmployee endpoint
            HttpResponseMessage response = await _httpClient.DeleteAsync($"/employees/{id}");

            if (!response.IsSuccessStatusCode)
            {
                // Handle the error response
                throw new Exception($"Failed to delete employee. StatusCode: {response.StatusCode}");
            }

        }


        private static StringContent SerializeEmployee(Employee employee)
        {
            // Serialize the employee object to JSON
            string employeeJson = Newtonsoft.Json.JsonConvert.SerializeObject(employee);
            var content = new StringContent(employeeJson, Encoding.UTF8, "application/json");
            return content;
        }




        /*
       


       

        

        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            Employee employee = _employeeRepository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            _employeeRepository.DeleteEmployee(id);
            return NoContent();
        }

        */

    }


    //todo if you have time move to DataAcess project and reuse with the API - currenty have 2 copies... 
    public class Employee
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public string Title { get; set; }
        public int Age { get; set; }
        public string Company { get; set; }
        public string WorkstationNo { get; set; }
        public string Site { get; set; }

    }


}
