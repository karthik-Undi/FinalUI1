using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalUI1.Models;
using FinalUI1.Models.ViewModels;
using System.Data.Objects;
using System.Net;

namespace FinalUI1.Controllers
{
    public class AdminController : Controller
    {
        Community db = new Community();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }


        #region Views of all tables


        public ActionResult AllTables()
        {
            TempData["LoginType"] = "the Chief Security Officer";

            var tables = new OneForAll
            {
                resident = db.Residents.ToList(),
                employee = db.Employees.ToList(),
                visitor = db.Visitors.ToList(),
                service = db.Services.ToList(),
                friendsAndFamily=db.FriendsAndFamilies.ToList()
            };

            return View(tables);
        }
        public ActionResult ResidentView()
        {
            TempData["LoginType"] = "the Chief Security Officer";

            var tables = new OneForAll
            {
                resident = db.Residents.ToList()
            };

            return View(tables);
        }
        public ActionResult EmployeeView()
        {
            TempData["LoginType"] = "the Chief Security Officer";

            var tables = new OneForAll
            {
                employee = db.Employees.ToList()
            };

            return View(tables);
        }
        public ActionResult ComplaintView()
        {
            TempData["LoginType"] = "the Chief Security Officer";

            var tables = new OneForAll
            {
                complaint = db.Complaints.ToList()

            };
            return View(tables);
        }
        public ActionResult VisitorView()
        {
            TempData["LoginType"] = "the Chief Security Officer";

            var tables = new OneForAll
            {
                visitor = db.Visitors.ToList()
            };
            return View(tables);
        }
        public ActionResult FriendsandFamilyView()
        {
            TempData["LoginType"] = "the Chief Security Officer";

            var tables = new OneForAll
            {
                friendsAndFamily = db.FriendsAndFamilies.ToList()
            };
            return View(tables);
        }
        #endregion


        #region Admin Dashboard
        public ActionResult AdminDashboard()
        {
            TempData["LoginType"] = "the Chief Security Officer";

            Session["auth"] = "username";

            if (Session["auth"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                var tables = new OneForAll
                {
                    resident = db.Residents.ToList(),
                    employee = db.Employees.ToList(),
                    visitor = db.Visitors.ToList(),
                    service = db.Services.ToList(),
                    friendsAndFamily = db.FriendsAndFamilies.ToList(),
                    DashboardPost = db.DashboardPosts.OrderByDescending(post => post.PostID).ToList()

                };
                TempData["LoginType"] = "the Chief Security Officer";

                TempData["property1"] = "Visitors Inside in ";
                TempData["quantity1"] = db.Visitors.Count(vis => vis.VisitorEntryStatus == "Inside Premises");

                TempData["property2"] = "Unresolved Complaints";
                TempData["quantity2"] = db.Complaints.Count(comp => comp.ComplaintStatus == "Raised");

                TempData["property3"] = "Services Scheduled Today";
                TempData["quantity3"] = db.Services.Count(vis => vis.AppointmentDateTime.Value.Year == DateTime.Now.Year
                                                                &&
                                                                vis.AppointmentDateTime.Value.Month == DateTime.Now.Month
                                                                &&
                                                                vis.AppointmentDateTime.Value.Day == DateTime.Now.Day
                                                                );
                TempData["property4"] = "Total Visitors";
                TempData["quantity4"] = db.Visitors.Count();
                return View(tables);
            }

        }
        #endregion

        public ActionResult ApproveRegistrations()
        {
            TempData["LoginType"] = "the Chief Security Officer";
            if ((db.Residents.Count(res => res.isApproved == null) == 0) && (db.Employees.Count(emp => emp.isApproved == null) == 0))
            {
                return RedirectToAction("NoApprovals");
            }
            else
            {
                var tables = new OneForAll
                {
                    resident = db.Residents.Where(res => res.isApproved == null).ToList(),
                    employee = db.Employees.Where(emp => emp.isApproved == null).ToList()
                };

                return View(tables);
            }
        }
        [HttpPost]
        public ActionResult ApproveRegistrations(OneForAll oneForAll)
        {
            TempData["LoginType"] = "the Chief Security Officer";


            return View();
        }

        public ActionResult ApproveResident(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resident resident = db.Residents.Find(id);
            if (resident == null)
            {
                return HttpNotFound();
            }
            resident.isApproved = "Approved";
            db.SaveChanges();
            return RedirectToAction("ApproveRegistrations");
        }

        public ActionResult ApproveEmployee(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            employee.isApproved = "Approved";
            db.SaveChanges();
            return RedirectToAction("ApproveRegistrations");
        }
        public ActionResult NoApprovals()
        {
            return View();
        }



    }
}