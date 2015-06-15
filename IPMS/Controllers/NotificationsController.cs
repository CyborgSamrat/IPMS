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
    public class NotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Notifications
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Notifications.ToList());
        }

        // GET: Notifications/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }
        public int NewNotiAtSession(string id)
        {
            System.Web.HttpContext.Current.Session.Clear();
            var notifications = from n in db.Notifications select n;
            notifications = notifications.Where(n => n.Receiver == id);
            var notilist = notifications.ToList();
            int i = 0;
            for (int j = 0; j < 5; j++)
            {
                System.Web.HttpContext.Current.Session[j.ToString()] = null;
            }

                foreach (var noti in notilist)
                {
                    i++;
                    String issue = noti.Issue;
                    string index = i.ToString();

                    System.Web.HttpContext.Current.Session[index] = issue;
                    if (i > 4)
                        break;
                }
            return  0;
        }

        public ActionResult _NotificationPartial(string id)
        {
            var notifications = from n in db.Notifications select n;
            notifications = notifications.Where(n => n.Receiver == id);
            var notilist = notifications.ToList();
            return PartialView(notilist);

        }
        [Authorize]
        public ActionResult UnseenNotification(String id)
        {
            var notifications = from n in db.Notifications select n;
            notifications = notifications.Where(n => n.Receiver == id);
            return View(notifications.ToList());
        }
        [Authorize]
        public ActionResult AllNotifications(String id)
        {
            var notifications = from n in db.Notifications select n;
            notifications = notifications.Where(n => n.Receiver == id);
            return View(notifications.ToList());
        }
        [Authorize]
        public ActionResult View(int? id)
        {
            if (ModelState.IsValid)
            {
                db.Notifications.Find(id).IsSeen = true;
                db.SaveChanges();
                var userid = db.Notifications.Find(id).Receiver;
                NewNotiAtSession(userid);
                return RedirectToAction("UnseenNotification", new {@id = userid });
            }
            return View();

        }

        // GET: Notifications/Create
        [Authorize]
        public ActionResult Create()
        {

            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NotificationId,SendTime,Issue,IsSeen,Sender,Receiver")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                db.Notifications.Add(notification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(notification);
        }
        [Authorize(Roles="Admin")]
        public ActionResult DeleteNoti(int id, string userid)
        {
            Notification notification = db.Notifications.Find(id);
            db.Notifications.Remove(notification);
            db.SaveChanges();
            return RedirectToAction("AllNotifications", new { UserId = userid});
        }
        public int SolvedComplaintNotificationsAdmin(Complaint complaint)
        {
            var notification = new Notification();
            notification.Sender = "IPMS";
            notification.Issue = "Complaint " + complaint.ComplaintId + " on device " + complaint.DeviceId
                + " has been marked as solved by " + complaint.LodgedBy + ".";
            notification.Receiver = "adminipms";
            notification.SendTime = System.DateTime.Now;
            db.Notifications.Add(notification);
            db.SaveChanges();
            SolvedComplaintNotificationsTechnician(complaint);

            return 0;
        }

        public int SolvedComplaintNotificationsTechnician(Complaint complaint)
        {
            var notification = new Notification();
            notification.Sender = "IPMS";
            notification.Issue = "Complaint " + complaint.ComplaintId + " on device " + complaint.DeviceId
                + " has been marked as solved by " + complaint.LodgedBy + ".";
            if (complaint.IsAssigned)
            {
                notification.Receiver = complaint.AssignedTo;
            }
            else
            {
                notification.Receiver = "adminipms";
            }
            
            notification.SendTime = System.DateTime.Now;
            db.Notifications.Add(notification);
            db.SaveChanges();

            return 0;
        }

        public int LodgeComplaintNotification(Complaint complaint)
        {
            var notification = new Notification();
            notification.Sender = "IPMS";
            notification.Issue = "Complaint Lodged on device " + complaint.DeviceId + " about " + complaint.Issue + " problem";
            notification.Receiver = "adminipms";
            notification.SendTime = System.DateTime.Now;
            db.Notifications.Add(notification);
            db.SaveChanges();
            return 0;
        }

        public int GiveFeedbackNotification(Feedback feedback)
        {
            var notification = new Notification();
            var cid = feedback.ComplaintId;
            var complaint = db.Complaints.Find(cid);
            notification.Issue = "Feedback given by " + feedback.GivenBy + " on complaint " + complaint.ComplaintId + " of " + complaint.Issue +" issue." ;
            notification.Receiver = complaint.LodgedBy;
            notification.Sender = "IPMS";
            notification.SendTime = System.DateTime.Now;
            db.Notifications.Add(notification);
            db.SaveChanges();
            if (complaint.IsAssigned)
            {
                GiveFeedbackNotificationTechnician(feedback);
            }
            return 0;
        }
        public int GiveFeedbackNotificationTechnician(Feedback feedback)
        {
            var notification = new Notification();
            var cid = feedback.ComplaintId;
            var complaint = db.Complaints.Find(cid);
            notification.Issue = "Feedback given by " + feedback.GivenBy + " on complaint " + complaint.ComplaintId + " of " + complaint.Issue + " issue.";
            notification.Receiver = complaint.AssignedTo;
            notification.Sender = "IPMS";
            notification.SendTime = System.DateTime.Now;
            db.Notifications.Add(notification);
            db.SaveChanges();
            return 0;
        }
        public int AssignTechnicianNotification(Complaint complaint)
        {
            var notification = new Notification();
            notification.Issue = "Yor are assigned to complaint " + complaint.ComplaintId + 
                " lodged by " + complaint.LodgedBy + " on device " + complaint.DeviceId;
            notification.Sender = "IPMS";
            notification.SendTime = System.DateTime.Now;
            notification.Receiver = complaint.AssignedTo;
            db.Notifications.Add(notification);
            db.SaveChanges();

            return 0;
        }
        private int NotificationToTechnician(Feedback feedback)
        {
            var notification = new Notification();
            var cid = feedback.ComplaintId;
            var complaint = db.Complaints.Find(cid);
            if (complaint.AssignedTo != null)
            {
                notification.Issue = "Feedback given by " + feedback.GivenBy + " on complaint of " + complaint.Issue + " issue.";
                notification.Receiver = complaint.AssignedTo;
                notification.Sender = "IPMS";
                notification.SendTime = System.DateTime.Now;
                db.Notifications.Add(notification);
                db.SaveChanges();
            }
            
            return 0;
        }
        // GET: Notifications/Edit/5
        [Authorize(Roles="Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NotificationId,SendTime,SeenTime,Issue,IsSeen,Sender,Receiver")] Notification notification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(notification);
        }

        // GET: Notifications/Delete/5
        [Authorize(Roles="Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notification notification = db.Notifications.Find(id);
            if (notification == null)
            {
                return HttpNotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Notification notification = db.Notifications.Find(id);
            var userid = notification.Receiver;
            db.Notifications.Remove(notification);
            db.SaveChanges();
            return RedirectToAction("AllNotifications", new { id =userid });
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
