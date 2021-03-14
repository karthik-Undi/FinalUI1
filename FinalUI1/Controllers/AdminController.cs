using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalUI1.Models;
using FinalUI1.Models.ViewModels;
using System.Data.Objects;
using System.Net;
using System.Data.Entity;

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


        public ActionResult CreatePost()
        {
            ViewBag.PostPostedBy = new SelectList(db.Residents, "ResidentID", "ResidentName");
            return View();
        }

        // POST: DashboardPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost([Bind(Include = "PostID,PostPostedBy,PostTitle,PostType,PostDescription,PostIntendedFor,PostTime")] DashboardPost dashboardPost)
        {

            TempData["auth"] = 12;
            int userid = Convert.ToInt32(TempData.Peek("auth"));

            

            if (ModelState.IsValid)
            {
                DashboardPost tempdashboardPost = new DashboardPost(userid, dashboardPost.PostTitle,
                dashboardPost.PostType, dashboardPost.PostIntendedFor, dashboardPost.PostDescription, DateTime.Now);
                db.DashboardPosts.Add(tempdashboardPost);
                db.SaveChanges();
                return RedirectToAction("AdminDashboard");
            }

            ViewBag.PostPostedBy = new SelectList(db.Residents, "ResidentID", "ResidentName", dashboardPost.PostPostedBy);
            return View(dashboardPost);
        }



        public ActionResult DeletePosts()
        {
            TempData["auth"] = 12;
            int userid = Convert.ToInt32(TempData.Peek("auth"));
            if (db.DashboardPosts.Count(post => post.PostPostedBy == userid) == 0)
            {
                return RedirectToAction("NoApprovals");
            }
            else
            {
                return View(db.DashboardPosts.Where(post=>post.PostPostedBy==userid));
            }
        }

        // GET: DashboardPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DashboardPost dashboardPost = db.DashboardPosts.Find(id);
            if (dashboardPost == null)
            {
                return HttpNotFound();
            }
            return View(dashboardPost);
        }

        // POST: DashboardPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DashboardPost dashboardPost = db.DashboardPosts.Find(id);
            db.DashboardPosts.Remove(dashboardPost);
            db.SaveChanges();
            return RedirectToAction("DeletePosts");
        }

    }
}