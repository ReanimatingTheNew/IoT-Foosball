using System.Linq;
using System.Net;
using System.Web.Mvc;
using Referee.Entities;
using Referee.Web.DataContexts;
using Referee.Web.Models;

namespace Referee.Web.Controllers.MVC
{
    public class TeamsController : Controller
    {
        private readonly RefereeDbContext _db = new RefereeDbContext();
        // GET: Teams
        public ActionResult Index()
        {
            var teams = _db.Teams.ToList();
            var modelList = teams.Select(team => new TeamViewModel
            {
                Id = team.Id,
                Name = team.GetNameOrDefault(),
                PlayerOneId = team.FirstPlayer.Id,
                PlayerTwoId = team.SecondPlayer.Id,
                PlayerOneName = team.FirstPlayer.Name,
                PlayerTwoName = team.SecondPlayer.Name
            }).ToList();
            return View(modelList);
        }

        // GET: Teams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var team = _db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            var model = new TeamViewModel
            {
                Id = team.Id,
                Name = team.Name,
                PlayerOneName = team.FirstPlayer.Name,
                PlayerTwoName = team.SecondPlayer.Name,
                PlayerOneId = team.FirstPlayer.Id,
                PlayerTwoId = team.SecondPlayer.Id
            };
            return View(model);
        }

        // GET: Teams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var team = _db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Team team)
        {
            var oldTeam = _db.Teams.Find(team.Id);
            //HACK BEGINS
            var unused = oldTeam.FirstPlayer.Name;
            unused = oldTeam.SecondPlayer.Name;
            //HACK ENDS
            oldTeam.Name = team.Name;
            _db.SaveChanges();
            return RedirectToAction("Index");
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