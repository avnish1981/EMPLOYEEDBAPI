using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using EmpDataAccess;

namespace EmpoyeeServiceAPI.Controllers
{
    [EnableCorsAttribute("http://localhost:54746", "*","*")]
    public class EmployeesController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage Employees(string gender="all")
        {
            using(AvnishDBEntities entities = new AvnishDBEntities())
            {
                switch (gender.ToLower())
                {
                    case "all":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender.ToLower() == "male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "please provide all,maleand female in parameter");

                }
            }
            
            
        }
        //[HttpGet]
        //public IEnumerable<Employee > Employees()
        //{
        //    using (AvnishDBEntities entities = new AvnishDBEntities())
        //    {
        //        return entities.Employees.ToList();
        //    }
        //}
        [HttpGet]
        [DisableCors ]
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
        public HttpResponseMessage InsertEmp([FromUri ] Employee employee)
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
