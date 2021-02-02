using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmpDataAccess;

namespace EmpoyeeServiceAPI.Controllers
{
    public class EmployeesController : ApiController
    {
        [HttpGet]
        public IEnumerable<Employee > Employees()
        {
            using (AvnishDBEntities entities = new AvnishDBEntities())
            {
                return entities.Employees.ToList();
            }
        }

        public Employee Employees(int id)
        {
            using (AvnishDBEntities entities = new AvnishDBEntities())
            {
                return entities.Employees.FirstOrDefault(e => e.ID == id);
                
            }
        }
        public HttpResponseMessage InsertEmp([FromBody] Employee employee)
        {
            try
            {
                using (AvnishDBEntities entities = new AvnishDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();
                }
                var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                return message;
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
