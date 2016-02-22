using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Referee.Entities;
using Referee.Web.DataContexts;
using Referee.Web.DataServices;
using Referee.Web.Models;
using Match = Referee.Entities.Match;

namespace Referee.Web.Controllers.MVC
{
    public class MatchesController : Controller
    {
        private readonly RefereeDbContext _db = new RefereeDbContext();
        // GET: Matches
        [AllowAnonymous]
        public ActionResult Index()
        {
            var matches = _db.Matches.ToList();
            var modelList = matches.Select(match => new MatchIndexViewModel
            {
                Id = match.Id,
                TeamOneName = match.RedTeam.GetNameOrDefault(),
                TeamTwoName = match.BlueTeam.GetNameOrDefault(),
                Location = match.Table.Location,
                IsFinished = match.IsFinished,
                StartTime = match.StartTime,
                EndTime = match.EndTime,
                CreatorName = match.Creator.Name
            }).ToList();
            return View(modelList);
        }

        // GET: Matches/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var match = _db.Matches.Find(id);
            if (match == null)
            {
                return HttpNotFound();
            }
            return View(match);
        }

        // GET: Matches/Create
        public ActionResult Create(int? tableId)
        {
            var table = _db.Tables.Find(tableId);
            if (table == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new MatchCreateViewModel
            {
                TableId = table.Id,
                SideOneColorName = Enum.GetName(typeof (Table.Color), table.SideOneColor),
                SideTwoColorName = Enum.GetName(typeof (Table.Color), table.SideTwoColor),
                Players = new SelectList(_db.Players, "Id", "Name"),
                Tables = new SelectList(_db.Tables, "Id", "Name")
            };
            return View(model);
        }

        // POST: /Matches/Delete/5
        public ActionResult Delete(int id)
        {
            var match = _db.Matches.Find(id);
            _db.Matches.Remove(match);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MatchCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var creator = _db.Players.First(p => p.AccountEmail == User.Identity.Name);
            var redTeam = TeamService.Get(_db, model.RedOneId, model.RedTwoId);
            var blueTeam = TeamService.Get(_db, model.BlueOneId, model.BlueTwoId);
            var table = _db.Tables.Find(model.TableId);
            var match = new Match
            {
                BlueTeam = blueTeam,
                RedTeam = redTeam,
                Table = table,
                StartTime = DateTime.Now,
                Creator = creator
            };

            _db.Matches.Add(match);
            _db.SaveChanges();
            return RedirectToAction("Details", "Matches", new {id = match.Id});
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