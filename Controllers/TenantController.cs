using PropertyRentalManagementMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PropertyRentalManagementMVC.Controllers
{
    public class TenantController : Controller
    {
        // GET: Tenant
        [HttpGet]
        public ActionResult Index()  //Index mane page for registration new tenant
        {
            Tenant tenantModel = new Tenant();
            return View(tenantModel);
            
        }

        [HttpPost]
        public ActionResult Register(Tenant tenantModel)  // Create an on-line account for Potential Tenants
        {
            using (PropertyRentalManagementDBEntities dbModel = new PropertyRentalManagementDBEntities())
            {
                if (dbModel.Tenants.Any(x => x.Email == tenantModel.Email))
                {
                    ViewBag.DublicateMessage = "Tenant with same email already exist.";
                    return View("Index", tenantModel);
                }
                dbModel.Tenants.Add(tenantModel); //save new tenant in table Tenants
                dbModel.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successful. Please login ...";
            return View("Index", new Tenant());
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Tenant tenantModel)
        {
            using (PropertyRentalManagementDBEntities db = new PropertyRentalManagementDBEntities())
            {
                var userDetails = db.Tenants.Where(x => x.Email == tenantModel.Email && x.Password == tenantModel.Password).FirstOrDefault();
                if (userDetails == null)
                {
                    tenantModel.LoginErrorMessage = "Wrong Email or Password";
                    return View("Login", tenantModel);
                }
                else
                {
                    Session["tenantId"] = userDetails.TenantId;
                    Session["tenantFirstName"] = userDetails.FirstName;
                    Session["tenantLastName"] = userDetails.LastName;

                    FormsAuthentication.SetAuthCookie(tenantModel.Email, false);
                    return RedirectToAction("Apartment", "Tenant");
                }
            }
        }

        private PropertyRentalManagementDBEntities db = new PropertyRentalManagementDBEntities();

        public ActionResult Apartment(string statusName, string numberOfRooms)   //For View any apartment suitable for their needs 
        {
            ViewBag.NumberOfRooms = new SelectList(db.Apartments, "NumberOfRooms", "NumberOfRooms");

            var status = from s in db.Apartments select s;

            if (!String.IsNullOrEmpty(statusName))
            {
                status = status.Where(x => x.Status.StartsWith(statusName));
            }
            
            if (!String.IsNullOrEmpty(numberOfRooms))
            {
                int num = Convert.ToInt32(numberOfRooms);
                return View(status.Where(x => x.NumberOfRooms == num));
            }
            else
            {
               return View(status);
            }
        }


        //GET: Appointment 

        [HttpGet]
        public ActionResult Appointment()  //Make an appointment with the property manager
        {
            Appointment appModel = new Appointment();
            return View(appModel);
        }

        [HttpPost]
        public ActionResult Appointment(Appointment appModel)  
        {
            using (PropertyRentalManagementDBEntities dbModel = new PropertyRentalManagementDBEntities())
            {
                if (dbModel.Appointments.Any(x => x.AppointmentDate == appModel.AppointmentDate && x.AppoitmentTime == appModel.AppoitmentTime))
                {
                    ViewBag.DublicateMessage = "This time is already taken";
                    return View("Index", appModel);
                }
                else if (dbModel.Managers.Find(appModel.ManagerId) == null)
                {
                    return View("Index", appModel);
                }
                dbModel.Appointments.Add(appModel); //save new appointment in table
                dbModel.SaveChanges();
                ViewBag.SuccessMessage = "Your appointment is sheduled.";
            }
            
            ModelState.Clear();
            
            return View("Appointment", new Appointment());
        }


        //GET: MESSAGE
        [HttpGet]
        public ActionResult Message()  // Schedule an appointment with the property manager
        {
            Message mesModel = new Message();
            return View(mesModel);

        }
        [HttpPost]
        public ActionResult Message(Message mesModel)  // Send necessary messages to the property manager
        {
            using (PropertyRentalManagementDBEntities dbModel = new PropertyRentalManagementDBEntities())
            {
                dbModel.Messages.Add(mesModel); //save new messages in table
                dbModel.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.SuccessMessage = "Your message send to manager";
            return View("Message", new Message());
        }

        public ActionResult LogOut()
        {
            Session.Abandon(); //make clear all fields

            return RedirectToAction("Index", "Home"); //redirect to Home page
        }





    }
}