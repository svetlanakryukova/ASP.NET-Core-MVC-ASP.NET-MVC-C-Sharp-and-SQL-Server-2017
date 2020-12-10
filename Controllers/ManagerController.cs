using PropertyRentalManagementMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyRentalManagementMVC.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Manager managerModel)
        {
            using (PropertyRentalManagementDBEntities db = new PropertyRentalManagementDBEntities())
            {
                var userDetails = db.Managers.Where(x => x.Email == managerModel.Email && x.Password == managerModel.Password).FirstOrDefault();
                if (userDetails == null)
                {
                    managerModel.LoginErrorMessage = "Wrong Email or Password";
                    return View("Index", managerModel);
                }
                else
                {
                    Session["managerId"] = userDetails.ManagerId;
                    TempData["managerId"] = userDetails.ManagerId;
                    Session["managerFirstName"] = userDetails.FirstName;
                    Session["managerLastName"] = userDetails.LastName;
                    return RedirectToAction("Index", "Buildings");
                }
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon(); //make clear all fields

            return RedirectToAction("Index", "Home"); //redirect to Home page
        }
    }
}