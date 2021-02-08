using EmpDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmpoyeeServiceAPI
{
    public class EmployeesSecurity
    {
        public static bool Login(string username,string password)
        {
            using(AvnishDBEntities entities = new AvnishDBEntities())
            {
                return entities.Users.Any(User => User.username==username && User.password == password);
            }
        }
    }
}