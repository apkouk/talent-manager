using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TalentManager.Models;
using TalentManager.Filters;

namespace TalentManager.Controllers
{
    
    public class EmployeesController : ApiController
    {
        [EnableETag]
        public Employee Get(int id)
        {
            if (id == 0)
                throw new ArgumentException("Invalid Employee Id");

            return new Employee()
            {
                Id = id,
                Name = "John Q Law",
                Department = "Enforcement"
            };
        }
        public HttpResponseMessage GetAllEmployees()
        {
            var employees = new Employee[]
            {
                new Employee()
                {
                    Id = 12345,
                    Name = "John Q Law",
                    Department = "Enforcement"
                },
                new Employee()
                {
                    Id = 45678,
                    Name = "Jane Q Taxpayer",
                    Department = "Revenue"
                }
            };

            var response = Request.CreateResponse<IEnumerable<Employee>>(HttpStatusCode.OK, employees);
            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromSeconds(6),
                MustRevalidate = true,
                //This private allows caching only to the browser of the final user, it does not assure security. PAG 52
                Private = true
            };
            return response;
        }

        //With the ConcurrencyChecker we make sure that we cannot change data by two user at the same time using ETags. PAG 55
        [ConcurrencyChecker]
        public void Put(Employee employee) { }
    }
}
