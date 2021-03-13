using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalUI1.Models;
using FinalUI1.Models.ViewModels;

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
        #region
        public ActionResult AllTables()
        {
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
            var tables = new OneForAll
            {
                resident = db.Residents.ToList()
            };

            return View(tables);
        }
        public ActionResult EmployeeView()
        {
            var tables = new OneForAll
            {
                employee = db.Employees.ToList()
            };

            return View(tables);
        }
        public ActionResult ComplaintView()
        {
            var tables = new OneForAll
            {
                complaint = db.Complaints.ToList()

            };
            return View(tables);
        }
        public ActionResult VisitorView()
        {
            var tables = new OneForAll
            {
                visitor = db.Visitors.ToList()
            };
            return View(tables);
        }
        public ActionResult FriendsandFamilyView()
        {

            //var ajoin = from f in db.FriendsAndFamilies
            //            join hid in db.Residents
            //            on f.ResidentID equals hid.ResidentID
            //            into fafres
            //            from fr in fafres.DefaultIfEmpty()
            //            select new
            //            {
            //                fname= f.FaFName,
            //                frel= f.FaFRelation,
            //                rid=f.ResidentID,
            //                rname=fr.ResidentName,
            //                hno=fr.ResidentHouseNo
            //            };



            var tables = new OneForAll
            {
                friendsAndFamily = db.FriendsAndFamilies.ToList()
            };
            return View(tables);
        }
        #endregion



    }
}