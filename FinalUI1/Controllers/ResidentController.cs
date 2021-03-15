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
    public class ResidentController : Controller
    {
        Community db = new Community();

        // GET: Resident
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NoRecords()
        {
            return View();
        }

        #region Resident Dashboard
        public ActionResult ResidentDashboard()
        {

            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                var tables = new OneForAll
                {
                    resident = db.Residents.ToList(),
                    employee = db.Employees.ToList(),
                    visitor = db.Visitors.ToList(),
                    service = db.Services.ToList(),
                    friendsAndFamily = db.FriendsAndFamilies.ToList(),
                    DashboardPost = db.DashboardPosts.OrderByDescending(post => post.PostID).ToList()

                };

                DateTime MyDate = DateTime.Now;

                int Year = MyDate.Year;
                int Month = MyDate.Month;
                int Day = MyDate.Day;

                TimeSpan t = (new DateTime(Year, Month, 24) - DateTime.Now);
                int days = (int)t.TotalDays + 1;

                TempData["LoginType"] = "Resident";

                TempData["property1"] = "Maintainance Fee Due in";
                TempData["quantity1"] = days;

                TempData["property2"] = "Unresolved Complaints";
                TempData["quantity2"] = db.Complaints.Count(comp => comp.ComplaintStatus == "Raised" && comp.ResidentID == userid);

                TempData["property3"] = "Services Scheduled Today";
                TempData["quantity3"] = db.Services.Count(ser => ser.AppointmentDateTime.Value.Year == DateTime.Now.Year
                                                                &&
                                                                ser.AppointmentDateTime.Value.Month == DateTime.Now.Month
                                                                &&
                                                                ser.AppointmentDateTime.Value.Day == DateTime.Now.Day
                                                                &&
                                                                ser.ResidentID == userid
                                                                &&
                                                                ser.ServiceApproval!="requested"
                                                                &&
                                                                ser.ServiceApproval!="paid"
                                                                );

                TempData["property4"] = "Total Visitors";
                TempData["quantity4"] = db.Visitors.Count(vis => vis.ResidentID == userid);
                return View(tables);
            }

        }
        #endregion


        #region Visitor
        public ActionResult AddVisitor()
        {
            //Session["auth"] = 12;
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

            //Session["auth"] = 12;

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

        public ActionResult VieworDeleteVisitor()
        {

            //Session["auth"] = 12;

            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {

                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                int count = db.Visitors.Count(vis => vis.ResidentID == userid);
                if (count > 0)
                {
                    var tables = new OneForAll
                    {
                        visitor = db.Visitors.Where(vis => vis.ResidentID == userid).ToList()
                    };
                    return View(tables);

                }
                else
                    return RedirectToAction("NoRecords");
            }
        }


        // GET: Visitors/Delete/5
        public ActionResult DeleteVisitor(int? id)
        {
            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Visitor visitor = db.Visitors.Find(id);
                if (visitor == null)
                {
                    return HttpNotFound();
                }
                return View(visitor);
            }
        }

        // POST: Visitors/Delete/5
        [HttpPost, ActionName("DeleteVisitor")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                Visitor visitor = db.Visitors.Find(id);
                db.Visitors.Remove(visitor);
                db.SaveChanges();
                return RedirectToAction("VieworDeleteVisitor");
            }
        }

        #endregion


        #region Noticeboard post
        public ActionResult CreatePost()
        {
            //Session["auth"] = 12;

            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {

                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                return View();
            }
        }

        // POST: DashboardPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost([Bind(Include = "PostID,PostPostedBy,PostTitle,PostType,PostDescription,PostIntendedFor,PostTime")] DashboardPost dashboardPost)
        {

            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;



                if (ModelState.IsValid)
                {
                    DashboardPost tempdashboardPost = new DashboardPost(userid, dashboardPost.PostTitle,
                    dashboardPost.PostType, dashboardPost.PostIntendedFor, dashboardPost.PostDescription, DateTime.Now);
                    db.DashboardPosts.Add(tempdashboardPost);
                    db.SaveChanges();
                    return RedirectToAction("VieworDeletePosts");
                }
                return View(dashboardPost);
            }
        }



        public ActionResult VieworDeletePosts()
        {

            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                if (db.DashboardPosts.Count(post => post.PostPostedBy == userid) == 0)
                {
                    return RedirectToAction("NoRecords");
                }
                else
                {
                    return View(db.DashboardPosts.Where(post => post.PostPostedBy == userid));
                }
            }
        }

        // GET: DashboardPosts/Delete/5
        public ActionResult DeletePost(int? id)
        {
            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
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
        }

        // POST: DashboardPosts/Delete/5
        [HttpPost, ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed_Post(int id)
        {
            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                DashboardPost dashboardPost = db.DashboardPosts.Find(id);
                db.DashboardPosts.Remove(dashboardPost);
                db.SaveChanges();
                return RedirectToAction("VieworDeletePosts");
            }
        }
        #endregion


        #region Service booking

        // GET: Services/Create
        public ActionResult BookService()
        {
            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                return View();
            }
        }

        // POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookService([Bind(Include = "ServiceID,ResidentID,ServiceType,AppointmentDateTime,ServiceApproval,ServiceMessage,EmployeeID")] Service service)
        {
            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                service.ResidentID = 12;
                service.ServiceApproval = "waiting";
                if (ModelState.IsValid)
                {
                    db.Services.Add(service);
                    db.SaveChanges();
                    return RedirectToAction("VieworDeleteServices");
                }
                return View();
            }
        }


        public ActionResult VieworDeleteServices()
        {
            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                if (db.Services.Count(ser => ser.ResidentID == userid) == 0)
                {
                    return RedirectToAction("NoRecords");
                }
                else
                {
                    return View(db.Services.Where(ser => ser.ResidentID == userid));
                }
            }
        }

        public ActionResult DeleteService(int? id)
        {
            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Service service = db.Services.Find(id);
                if (service == null)
                {
                    return HttpNotFound();
                }
                return View(service);
            }
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("DeleteService")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed_Service(int id)
        {
            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                Service service = db.Services.Find(id);
                db.Services.Remove(service);
                db.SaveChanges();
                return RedirectToAction("VieworDeleteServices");
            }
        }




        #endregion


        #region Complaints

        public ActionResult RaiseComplaint()
        {
            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RaiseComplaint([Bind(Include = "ComplaintID,ResidentID,ComplaintRegarding,ComplaintStatus")] Complaint complaint)
        {


            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;

                complaint.ResidentID = userid;
                complaint.ComplaintStatus = "raised";
                if (ModelState.IsValid)
                {
                    db.Complaints.Add(complaint);
                    db.SaveChanges();
                    return RedirectToAction("VieworDeleteComplaints");
                }

                return View(complaint);
            }
        }

        public ActionResult VieworDeleteComplaints()
        {
           // Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                if (db.Complaints.Count(com => com.ResidentID == userid) == 0)
                {
                    return RedirectToAction("NoRecords");
                }
                else
                {
                    return View(db.Complaints.Where(com => com.ResidentID == userid));
                }
            }
        }


        public ActionResult DeleteComplaints(int? id)
        {
            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Complaint complaint = db.Complaints.Find(id);
                if (complaint == null)
                {
                    return HttpNotFound();
                }
                return View(complaint);
            }
        }

        // POST: Complaints1/Delete/5
        [HttpPost, ActionName("DeleteComplaints")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed_Complaints(int id)
        {
            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                Complaint complaint = db.Complaints.Find(id);
                db.Complaints.Remove(complaint);
                db.SaveChanges();
                return RedirectToAction("VieworDeleteComplaints");
            }
        }





        #endregion


        #region Friends and family

        public ActionResult AddFamily()
        {
            return View();
        }

        // POST: FriendsAndFamilies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFamily([Bind(Include = "FaFID,FaFName,ResidentID,FaFRelation")] FriendsAndFamily friendsAndFamily)
        {
            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                friendsAndFamily.ResidentID = userid;
                if (ModelState.IsValid)
                {
                    db.FriendsAndFamilies.Add(friendsAndFamily);
                    db.SaveChanges();
                    return RedirectToAction("VieworDeleteFoF");
                }
                return View(friendsAndFamily);
            }
        }

        public ActionResult VieworDeleteFoF()
        {
            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                if (db.FriendsAndFamilies.Count(fof => fof.ResidentID == userid) == 0)
                {
                    return RedirectToAction("NoRecords");
                }
                else
                {
                    return View(db.FriendsAndFamilies.Where(fof => fof.ResidentID == userid));
                }
            }
        }
        public ActionResult DeleteFoF(int? id)
        {
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                FriendsAndFamily friendsAndFamily = db.FriendsAndFamilies.Find(id);
                if (friendsAndFamily == null)
                {
                    return HttpNotFound();
                }
                return View(friendsAndFamily);
            }
        }

        // POST: FriendsAndFamilies/Delete/5
        [HttpPost, ActionName("DeleteFoF")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed_FoF(int id)
        {
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                FriendsAndFamily friendsAndFamily = db.FriendsAndFamilies.Find(id);
                db.FriendsAndFamilies.Remove(friendsAndFamily);
                db.SaveChanges();
                return RedirectToAction("VieworDeleteFoF");
            }
        }




        #endregion


        #region Payments

        public ActionResult PaymentHistory()
        {

            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Payment temppay = db.Payments.FirstOrDefault(pay => pay.ResidentID == userid);
                if (db.Payments.Count(pay => pay.ResidentID == userid) == 0)
                {
                    return RedirectToAction("NoRecords");
                }
                else
                {
                    return View(db.Payments.Where(pay => pay.ResidentID == userid));
                }
            }
        }



        public ActionResult Pay()
        {

            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Payment temppay = db.Payments.FirstOrDefault(pay => pay.ResidentID == userid);
                if (db.Payments.Count(pay => pay.ResidentID == userid) == 0)
                {
                    return RedirectToAction("NoRecords");
                }
                else
                {
                    return View(db.Payments.Where(pay => pay.ResidentID == userid && pay.PaymentStatus== "requested"));
                }
            }
        }


        public ActionResult PayforService(int? id)
        {
            //Session["auth"] = 12;
            if (Session["auth"] == null)
            {
                return RedirectToAction("Logout", "Logout");
            }
            else
            {
                int userid = Convert.ToInt32(Session["auth"]);
                Resident tempr = db.Residents.FirstOrDefault(res => res.ResidentID == userid);
                TempData["UserName"] = tempr.ResidentName;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Payment payment = db.Payments.Find(id);
                if (payment == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    Payment p = db.Payments.Single(ser => ser.PaymentID == id);
                    p.PaymentStatus = "paid";
                    db.SaveChanges();

                    int serID = (int)p.ServiceID;
                    Service tempser = db.Services.Find(serID);
                    tempser.ServiceApproval = "paid";
                    db.SaveChanges();
                    return RedirectToAction("PaymentHistory");
                }
            }
        }



        #endregion

    }


}