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
        [HttpGet]
        public HttpResponseMessage Employees(int id)
        {
            using (AvnishDBEntities entities = new AvnishDBEntities())
            {
                var entity =  entities.Employees.FirstOrDefault(e => e.ID == id);
                if(entity==null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Not Matching employee id ");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
            }
        }
        [HttpPost]
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
        [HttpDelete ]
        public HttpResponseMessage DeleteEmp(int id)
        {
            try
            {


                using (AvnishDBEntities entities = new AvnishDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Not Matching employee id ");
                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.OK);
                        return message;
                    }

                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest , ex);
            }

            
        }
        [HttpPut ] 
        public HttpResponseMessage UpdateEmp(int id,[FromBody ] Employee employee)
        {
            try
            {
                using (AvnishDBEntities enities = new AvnishDBEntities())
                {
                    var existingEmp = enities.Employees.FirstOrDefault(e => e.ID == id);
                    if (existingEmp == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "employee not found to update");
                    }
                    else
                    {
                        existingEmp.FirstName = employee.FirstName;
                        existingEmp.LastName = employee.LastName;
                        existingEmp.Gender = employee.Gender;
                        existingEmp.Salary = employee.Salary;
                        enities.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.OK, existingEmp);
                        return message;

                    }


                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            
            
        }
    }
}
