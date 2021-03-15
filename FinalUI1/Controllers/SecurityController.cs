using FinalUI1.Models;
using FinalUI1.Models.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace FinalUI1.Controllers
{
    public class SecurityController : Controller
    {
        private Community db = new Community();

        // GET: Security
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Login", "Login");
        }

        #region Views of all tables

        public ActionResult SecurityDashboard()
        {
            Session["auth"] = 3;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Employee emp = db.Employees.Single(x => x.EmployeeID == userid);
                TempData["UserName"] = emp.EmployeeName;



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

                    TempData["property1"] = "Visitors Inside";
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
        }

        public ActionResult AllTables()
        {
            Session["auth"] = 3;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Employee emp = db.Employees.Single(x => x.EmployeeID == userid);
                TempData["UserName"] = emp.EmployeeName;
                var tables = new OneForAll
                {
                    resident = db.Residents.ToList(),
                    employee = db.Employees.ToList(),
                    visitor = db.Visitors.ToList(),
                    service = db.Services.ToList(),
                    friendsAndFamily = db.FriendsAndFamilies.ToList()
                };

                return View(tables);
            }
        }

        public ActionResult ResidentView()
        {
            Session["auth"] = 3;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Employee emp = db.Employees.Single(x => x.EmployeeID == userid);
                TempData["UserName"] = emp.EmployeeName;
                var tables = new OneForAll
                {
                    resident = db.Residents.ToList()
                };

                return View(tables);
            }
        }

        public ActionResult EmployeeView()
        {
            Session["auth"] = 3;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Employee emp = db.Employees.Single(x => x.EmployeeID == userid);
                TempData["UserName"] = emp.EmployeeName;
                var tables = new OneForAll
                {
                    employee = db.Employees.ToList()
                };

                return View(tables);
            }
        }

       

        public ActionResult VisitorView()
        {
            Session["auth"] = 3;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Employee emp = db.Employees.Single(x => x.EmployeeID == userid);
                TempData["UserName"] = emp.EmployeeName;
                var tables = new OneForAll
                {
                    visitor = db.Visitors.ToList()
                };
                return View(tables);
            }
        }

        public ActionResult FriendsandFamilyView()
        {
            Session["auth"] = 3;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Employee emp = db.Employees.Single(x => x.EmployeeID == userid);
                TempData["UserName"] = emp.EmployeeName;
                var tables = new OneForAll
                {
                    friendsAndFamily = db.FriendsAndFamilies.ToList()
                };
                return View(tables);
            }
        }

        public ActionResult AddVisitor()
        {
            Session["auth"] = 3;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                ViewBag.ResidentID = new SelectList(db.Employees, "EmployeeID", "EmployeeName");
                return View();
            }
        }

        // POST: Visitors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVisitor([Bind(Include = "VisitorId,VisitorName,VisitorResaon,VisitStartTime,VisitorEndTime,ResidentID,VisitorEntryStatus,VisitorApprovedBy")] Visitor visitor)
        {

            Session["auth"] = 3;

            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;

                visitor.ResidentID = userid;
                if (ModelState.IsValid)
                {
                    db.Visitors.Add(visitor);
                    db.SaveChanges();
                    return RedirectToAction("VieworDeleteVisitor");
                }
                return View();
            }
        }




    }





}

#endregion Views of all tables