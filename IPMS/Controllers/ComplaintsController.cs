using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;

namespace IdentitySample.Controllers
{
    public class ComplaintsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Complaints
            [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Complaints.ToList());
        }

        // GET: Complaints/Details/5
            [Authorize(Roles = "Technician")]
        public ActionResult Details(int? id)
        {
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
            [Authorize(Roles = "Technician")]
        public ActionResult TechViewPending(string id)
        {
            var user = db.Users.Find(id);
            var complaints = from c in db.Complaints select c;
            complaints = complaints.Where(c => c.AssignedTo.Equals(id));
            return View(complaints.ToList());

        }

            [Authorize]
        public ActionResult MarkAsSolved(int id)
        {
            db.Complaints.Find(id).IsSolved = true;
            db.SaveChanges();
            var complaint = db.Complaints.Find(id);
            var notification = new NotificationsController();
            notification.SolvedComplaintNotificationsAdmin(complaint);


            return View("Index", "Home");
        }
            [Authorize(Roles = "Technician")]
        public ActionResult TechViewSolved(string id)
        {
            var user = db.Users.Find(id);
            var complaints = from c in db.Complaints select c;
            complaints = complaints.Where(c => c.AssignedTo.Equals(id));
            return View(complaints.ToList());

        }
            [Authorize(Roles = "Admin")]
        public ActionResult AdminViewPending(string id)
        {
            var user = db.Users.Find(id);
            var complaints = from c in db.Complaints select c;
            complaints = complaints.Where(c => c.IsAssigned.Equals(true));
            return View(complaints.ToList());

        }
            [Authorize(Roles = "Admin")]
        public ActionResult PrintReport(int? id = 30)
        {
            var fromDate = System.DateTime.Now;

            if (id == 7)
            {
                fromDate = DateTime.Today.AddDays(-7);
            }

            else if (id == 15)
            {
                fromDate = DateTime.Today.AddDays(-15);
            }

            else if (id == 30)
            {
                fromDate = DateTime.Today.AddDays(-30);
            }


            List<Complaint> complaintList = new List<Complaint>();
            var complaints = db.Complaints;

            foreach (Complaint complaint in complaints)
            {
                if (complaint.LodgedOn >= fromDate)
                {
                    complaintList.Add(complaint);
                }
            }




            return View(complaintList);
            
        }
        public ActionResult GeneratePDF()
        {
            return new Rotativa.ActionAsPdf("PrintReport");
        }
            [Authorize(Roles = "Admin")]
        public ActionResult AdminViewSolved(string id)
        {
            var user = db.Users.Find(id);
            var complaints = from c in db.Complaints select c;
            complaints = complaints.Where(c => c.IsSolved.Equals(true));
            return View(complaints.ToList());

        }
            [Authorize(Roles = "Admin")]
        public ActionResult AdminViewNew(string id)
        {
            var user = db.Users.Find(id);
            var complaints = from c in db.Complaints select c;
            complaints = complaints.Where(c => c.IsAssigned.Equals(false));
            return View(complaints.ToList());

        }
            [Authorize]
        public ActionResult Feedback(String id = null)
        {
            var user = db.Users.Find(id);
            var complaints = from c in db.Complaints select c;
            complaints = complaints.Where(c => c.LodgedBy.Equals(id));
            return View(complaints.ToList());
        }
            [Authorize]
        public ActionResult GiveFeedback(String id = null)
        {
            var user = db.Users.Find(id);
            var complaints = from c in db.Complaints select c;
            complaints = complaints.Where(c => c.LodgedBy.Equals(id));
            return View(complaints.ToList());
        }
        [Authorize]
        public ActionResult ViewPending(String id = null)
        {
            if (id == null)
            {
                id =User.Identity.Name;
            }
            var user = db.Users.Find(id);
            var complaints = from c in db.Complaints select c;
            complaints = complaints.Where(c => c.LodgedBy.Equals(id));
            return View(complaints.ToList());
        }
        [Authorize]
        public ActionResult ViewSolved(String id = null)
        {
            var user = db.Users.Find(id);
            var complaints = from c in db.Complaints select c;
            complaints = complaints.Where(c => c.LodgedBy.Equals(id));
            return View(complaints.ToList());
        }


        // GET: Complaints/Create
        [Authorize]
        public ActionResult Create(int? id =0)
        {
            LocationsController.deviceid = (int)id;
            
            return View();
        }
        [Authorize]
        // POST: Complaints/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ComplaintId,LodgedOn,Issue,Description,LodgedBy,DeviceId")] Complaint complaint)
        {
            if (ModelState.IsValid)
            {
                complaint.DeviceId = LocationsController.deviceid;
                db.Complaints.Add(complaint);
                db.SaveChanges();
                var notification = new NotificationsController();
                notification.LodgeComplaintNotification(complaint);

                return RedirectToAction("ViewPending", new {id = complaint.LodgedBy });
            }

            return View(complaint);
        }

        [Authorize(Roles="Admin")]
        public ActionResult AssignTechnician(int? id)
        {
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

        // POST: Complaints/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignTechnician([Bind(Include = "ComplaintId,LodgedOn,IsSolved,IsAssigned,Status,Issue,Description,LodgedBy,DeviceId,AssignedTo")] Complaint complaint)
        {
            if (ModelState.IsValid)
            {
                var techId = complaint.AssignedTo;

                complaint.IsAssigned = true;
                db.Entry(complaint).State = EntityState.Modified;
                db.SaveChanges();
                var notifications = new NotificationsController();
                notifications.AssignTechnicianNotification(complaint);
                return RedirectToAction("AdminViewNew");
            }
            return View(complaint);
        }







       
        
        // GET: Complaints/Edit/5
        [Authorize(Roles="Admin")]
        public ActionResult Edit(int? id)
        {
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

        // POST: Complaints/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ComplaintId,LodgedOn,IsSolved,IsAssigned,Status,Issue,Description,LodgedBy,DeviceId,AssignedTo")] Complaint complaint)
        {
            if (ModelState.IsValid)
            {
                db.Entry(complaint).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(complaint);
        }

        // GET: Complaints/Delete/5
        [Authorize(Roles="Admin")]
        public ActionResult Delete(int? id)
        {
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

        // POST: Complaints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Complaint complaint = db.Complaints.Find(id);
            db.Complaints.Remove(complaint);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
