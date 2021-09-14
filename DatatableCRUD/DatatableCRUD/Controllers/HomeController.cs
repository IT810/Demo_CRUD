using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatatableCRUD.Models;

namespace DatatableCRUD.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEmployees()
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var employees = dc.Employees.OrderBy(a => a.FirstName).ToList();
                return Json(new { data = employees }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Save(int id)
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = dc.Employees.Where(a => a.EmployeeID == id).FirstOrDefault();
                if (v == null)
                {
                    ViewBag.tt = "Create Employee";
                }
                else
                {
                    ViewBag.tt = "Edit Employee";
                }
                return View(v);
            }
        }

        [HttpPost]
        public ActionResult Save(Employee emp)
        {
            bool status = false;
            string message = "";
            if (ModelState.IsValid)
            {
                using (MyDatabaseEntities dc = new MyDatabaseEntities())
                {
                    if (emp.EmployeeID > 0)
                    {
                        //Edit 
                        var v = dc.Employees.Where(a => a.EmployeeID == emp.EmployeeID).FirstOrDefault();
                        if (v != null)
                        {
                            v.FirstName = emp.FirstName;
                            v.LastName = emp.LastName;
                            v.Email = emp.Email;
                            v.City = emp.City;
                            v.Country = emp.Country;
                            message = "Updated Successfully!";
                        }
                    }
                    else
                    {
                        //Create
                        dc.Employees.Add(emp);
                        message = "Added Successfully!";
                    }
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status, tt = message } };
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = dc.Employees.Where(a => a.EmployeeID == id).FirstOrDefault();
                if (v != null)
                {
                    return View(v);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteEmployee(int id)
        {
            bool status = false;
            string message = "";
            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var v = dc.Employees.Where(a => a.EmployeeID == id).FirstOrDefault();
                if (v != null)
                {
                    dc.Employees.Remove(v);
                    dc.SaveChanges();
                    status = true;
                    message = "Deleted Successfully!";
                }
            }
            return new JsonResult { Data = new { status = status, tt = message } };
        }
    }
}