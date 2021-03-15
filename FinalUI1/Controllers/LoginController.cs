using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalUI1.Models;
using FinalUI1.Models.ViewModels;

namespace FinalUI1.Controllers
{
    public class LoginController : Controller
    {
        Community db = new Community();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotApproved()
        {
            Session.Clear();
            return View();
        }


        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginAll loginAll)
        {
            if(loginAll.Email=="admin@admin.com" && loginAll.Password == "11111111q!")
            {
                Session["auth"] = 12345;
                return RedirectToAction("AdminDashboard", "Admin");
            }
            if (loginAll.Role == "Resident")
            {
                Resident tempResident = db.Residents.Where(res => res.ResidentEmail == loginAll.Email && res.ResidentPassword == loginAll.Password).SingleOrDefault();


                if (tempResident != null)
                {
                    if (tempResident.isApproved != "Approved")
                        return RedirectToAction("Notapproved");
                    Session["auth"] = tempResident.ResidentID;
                    TempData["username"] = tempResident.ResidentName;
                    TempData.Keep();
                    return RedirectToAction("ResidentDashboard", "Resident");
                }
                else
                {
                    TempData["auth"] = "Invalid login Credentials";
                }
                return View();

            }
            else
            {
                Employee tempEmployee = db.Employees.Where(res => res.EmployeeEmail == loginAll.Email && res.EmployeePassword == loginAll.Password).SingleOrDefault();

                if (tempEmployee != null)
                {
                    if (tempEmployee.isApproved != "Approved")
                        return RedirectToAction("Notapproved");
                    Session["auth"] = tempEmployee.EmployeeID;
                    TempData["username"] = tempEmployee.EmployeeName;
                    TempData.Keep();
                    return RedirectToAction("EmployeeDashboard", "Employee");
                }
                else
                {
                    TempData["auth"] = "Invalid login Credentials";
                }
                return View();

            }

            
        }
        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Registration registration)
        {

            try
            {
                db.Residents.Add(new Resident(
                registration.Name,
                registration.Password,
                registration.Email,
                registration.ResidenceType,
                registration.MobileNo,
                registration.HouseNo
                ));
                HouseList tempHouse = db.HouseLists.Single(house => house.HouseID == registration.HouseNo);
                tempHouse.HouseIsFree = "occupied";

                db.SaveChanges();
                //TempData["reg_status_res"] = "Registration Successful";
                return RedirectToAction("Login");



            }
            catch(Exception e)
            {
                TempData["reg_status_res"] = "Something went wrong. "+e.Message;
            }

            return View();
        }


        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>

        public ActionResult RegisterEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterEmployee(Registration registration)
        {
            try
            {
                db.Employees.Add(new Employee(
                registration.Name,
                registration.Password,
                registration.Email_Emp,
                registration.MobileNo,
                registration.Role
                ));

                db.SaveChanges();
                //TempData["reg_status_emp"] = "Registration Successful";
                return RedirectToAction("Login");


            }
            catch (Exception e)
            {
                TempData["reg_status_emp"] = "Something went wrong. " + e.Message;
            }

            return View();
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>
        /// 
        public JsonResult DoesEmailExist(string Email)
        {
            return Json(!db.Residents.Any(res => res.ResidentEmail == Email), JsonRequestBehavior.AllowGet);
        }


        public JsonResult DoesEmailExistinEmployees(string Email_Emp)
        {
            return Json(!db.Employees.Any(emp => emp.EmployeeEmail == Email_Emp), JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsHouseFree(int HouseNo)
        {
            HouseList tempHouse = db.HouseLists.SingleOrDefault(h => h.HouseID == HouseNo);
            if (tempHouse.HouseIsFree == "free")
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }


    }




}