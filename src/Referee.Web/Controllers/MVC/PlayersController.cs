using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Referee.Entities;
using Referee.Web.DataContexts;

namespace Referee.Web.Controllers.MVC
{
    public class PlayersController : Controller
    {
        private readonly RefereeDbContext _db = new RefereeDbContext();
        // GET: Players
        public ActionResult Index()
        {
            return View(_db.Players.ToList());
        }

        // GET: Players/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var player = _db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Player player)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(player).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(player);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}