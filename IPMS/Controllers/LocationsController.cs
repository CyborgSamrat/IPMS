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
    public class LocationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Locations
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Locations.ToList());
        }

        // GET: Locations/Details/5
        [Authorize]
        public ActionResult Details(int? id, int? id2)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            rm = location.Room;
            RedirectToAction("ShowDevice", new { id = location.Device});
            return View(location);
        }
        [Authorize(Roles = "Admin")]
        // GET: Locations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LocationId,Room,Row,Column,Device")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(location);
        }
        [Authorize]
        public ActionResult SelectRoom()
        {
            var RoomList = db.Locations.Select(x => x.Room).Distinct().ToList();
            ViewBag.RoomList = RoomList;

            return View();
        }
        public static int deviceid = 0;
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectRoom(int room)
        {
            
            
            return RedirectToAction("SelectDevice", new {id = room });
        }
        
        public static int rm = 405;
        [Authorize]
        public ActionResult SelectDevice(int? id = 405)
        {
            var locations = new List<Location>();
            var locs = new List<Location>();
            locations = db.Locations.ToList();
            foreach (var l in locations)
            {
                if (l.Room == id)
                    locs.Add(l);
            }
            
            return View(locs);
        }

        [Authorize]
        public ActionResult ShowDevice(int? id = 0, int? id2 = 405)
        {

            
            var room = id2;
            var locations = new List<Location>();
            var locs = new List<Location>();
            locations = db.Locations.ToList();
            foreach (var l in locations)
            {
                if (l.Room == room)
                    locs.Add(l);
            }
            ViewBag.device = id;
            ViewBag.room = room;

            return View(locs);
        }

        // GET: Locations/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LocationId,Room,Row,Column,Device")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(location);
        }

        // GET: Locations/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
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
