using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalUI1.Models;
using FinalUI1.Models.ViewModels;
using System.Net;
using System.Data.Entity;

namespace FinalUI1.Controllers
{
    public class EmployeeController : Controller
    {
        Community db = new Community();
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NoRecords()
        {
            return View();
        }

        public ActionResult EmployeeDashboard()
        {
            //Session["auth"] = 3;


            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Employee tempemp = db.Employees.FirstOrDefault(res => res.EmployeeID == userid);
                TempData["UserName"] = tempemp.EmployeeName;
                var tables = new OneForAll
                {
                    resident = db.Residents.ToList(),
                    employee = db.Employees.ToList(),
                    visitor = db.Visitors.ToList(),
                    service = db.Services.ToList(),
                    friendsAndFamily = db.FriendsAndFamilies.ToList(),
                    DashboardPost = db.DashboardPosts.OrderByDescending(post => post.PostID).ToList()

                };


                
                return View(tables);
            }
        }






        public ActionResult VieworDeleteServices()
        {

            //Session["auth"] = 3;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Employee tememp = db.Employees.FirstOrDefault(res => res.EmployeeID == userid);
                TempData["UserName"] = tememp.EmployeeName;
                if (db.Services.Count(ser => ser.ServiceApproval =="waiting") == 0)
                {
                    return RedirectToAction("NoRecords");
                }
                else
                {
                    return View(db.Services.Where(ser => ser.ServiceApproval == "waiting"));
                }
            }
        }

        public ActionResult AcceptRequest(int? id)
        {
            //Session["auth"] = 3;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Employee tememp = db.Employees.FirstOrDefault(res => res.EmployeeID == userid);
                TempData["UserName"] = tememp.EmployeeName;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Service service = db.Services.Find(id);
                if (service == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    service = db.Services.Single(ser => ser.ServiceID == id);
                    service.ServiceApproval = "Accepted";
                    service.EmployeeID = userid;
                    db.SaveChanges();
                }
                return RedirectToAction("ViewAcceptedServices");
            }
        }


        public ActionResult ViewAcceptedServices()
        {

            //Session["auth"] = 3;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Employee tememp = db.Employees.FirstOrDefault(res => res.EmployeeID == userid);
                TempData["UserName"] = tememp.EmployeeName;
                if (db.Services.Count(ser => ser.EmployeeID == userid) == 0)
                {
                    return RedirectToAction("NoRecords");
                }
                else
                {
                    return View(db.Services.Where(ser => ser.EmployeeID == userid && ser.ServiceApproval== "Accepted"));
                }
            }
        }


        public ActionResult RequestPayment(int? id)
        {
            //Session["auth"] = 3;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Employee tememp = db.Employees.FirstOrDefault(res => res.EmployeeID == userid);
                TempData["UserName"] = tememp.EmployeeName;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Service service = db.Services.Find(id);
                if (service == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    service = db.Services.Single(ser => ser.ServiceID == id);
                    service.ServiceApproval = "Completed";

                    TempData["residendid"] = service.ResidentID;
                    TempData["serviceid"] = service.ServiceID;
                    return View();
                }
            }
        }

        [HttpPost, ActionName("RequestPayment")]
        [ValidateAntiForgeryToken]

        public ActionResult RequestPaymentfromResident(Payment payment)
        {
            //Session["auth"] = 3;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Service temps = db.Services.Find(Convert.ToInt32(TempData.Peek("serviceid")));
                payment.ResidentID = temps.ResidentID;
                payment.AmountPaidTo = userid;
                payment.PaymentStatus = "requested";
                payment.ServiceID = temps.ServiceID;
                db.Payments.Add(payment);
                temps.ServiceApproval = "payment requested";
                db.SaveChanges();
                return RedirectToAction("ViewAllServices");
            }
        }

        public ActionResult ViewAllServices()
        {

            //Session["auth"] = 3;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Employee tememp = db.Employees.FirstOrDefault(res => res.EmployeeID == userid);
                TempData["UserName"] = tememp.EmployeeName;
                if (db.Services.Count(ser => ser.EmployeeID == userid) == 0)
                {
                    return RedirectToAction("NoRecords");
                }
                else
                {
                    return View(db.Services.Where(ser => ser.EmployeeID == userid));
                }
            }
        }

        public ActionResult PaymentHistory()
        {

            //Session["auth"] = 3;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Payment temppay = db.Payments.FirstOrDefault(pay => pay.AmountPaidTo == userid);
                if (db.Payments.Count(pay => pay.AmountPaidTo == userid) == 0)
                {
                    return RedirectToAction("NoRecords");
                }
                else
                {
                    return View(db.Payments.Where(pay => pay.AmountPaidTo == userid));
                }
            }
        }




    }
}