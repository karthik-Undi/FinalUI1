using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalUI1.Models;
using FinalUI1.Models.ViewModels;
using System.Data.Objects;
using System.Net;
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace FinalUI1.Controllers
{
    public class ExportController : Controller
    {
        // GET: Export
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public FileResult ExportVisitor()
        {
            Community db = new Community();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[10] { new DataColumn("VisitorID"),
                                            new DataColumn("Visitor Name"),
                                            new DataColumn("Resident ID"),
                                            new DataColumn("Resident Name"),
                                            new DataColumn("House number"),
                                            new DataColumn("Reason"),
                                            new DataColumn("In Time"),
                                            new DataColumn("Out Time"),
                                            new DataColumn("Approved by Employee"),
                                            new DataColumn("Approved Employee name")});

            List<Visitor> visitors = db.Visitors.ToList();

            foreach (var vis in visitors)
            {
                dt.Rows.Add(vis.VisitorId, vis.VisitorName, vis.ResidentID, vis.Resident.ResidentName,
                    vis.Resident.ResidentHouseNo,vis.VisitorResaon,vis.VisitStartTime,
                    vis.VisitorEndTime,vis.Employee1.EmployeeID,vis.Employee1.EmployeeName);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }
        }
    }


}