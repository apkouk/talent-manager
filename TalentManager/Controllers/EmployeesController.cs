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
using System.Security.Claims;
using System.Security.Principal;
using System.IdentityModel.Configuration;


namespace TalentManager.Controllers
{
    
    public class EmployeesController : ApiController
    {
        [EnableETag]
        [HttpsOnly]
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

        public HttpResponseMessage Delete(int id)
        {
            // Based on ID, retrieve employee details and create the list of resource claims
            var employeeClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Country, "US"),
                new Claim("http://badri/claims/department", "Engineering")
            };
            if (User.CheckAccess("Employee", "Delete", employeeClaims))
            {
                //repository.Remove(id);
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            else
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }
    }

    public static class PrincipalHelper
    {
        public static bool CheckAccess(this IPrincipal principal, string resource, string action, IList<Claim> resourceClaims)
        {
            var context = new System.Security.Claims.AuthorizationContext(principal as ClaimsPrincipal, resource, action);
            resourceClaims.ToList().ForEach(c => context.Resource.Add(c));
            var config = new IdentityConfiguration();
            return config.ClaimsAuthorizationManager.CheckAccess(context);
        }
    }
}
