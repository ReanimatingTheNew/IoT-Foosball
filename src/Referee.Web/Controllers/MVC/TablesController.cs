using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Referee.Entities;
using Referee.Web.DataContexts;
using Referee.Web.DataServices;
using Referee.Web.Models;

namespace Referee.Web.Controllers.MVC
{
    public class TablesController : Controller
    {
        private readonly RefereeDbContext _db = new RefereeDbContext();
        // GET: Tables
        [AllowAnonymous]
        public ActionResult Index()
        {
            var modelList = new List<TableIndexViewModel>();
            var tables = _db.Tables.ToList();
            foreach (var table in tables)
            {
                var model = new TableIndexViewModel
                {
                    Id = table.Id,
                    Location = table.Location,
                    Name = table.Name,
                    MatchOngoing = MatchService.HasOngoingMatch(_db, table),
                    SideOneColor = table.SideOneColor,
                    SideTwoColor = table.SideTwoColor
                };
                var ongoingMatch = model.MatchOngoing ? MatchService.GetOngoingMatch(_db, table) : null;

                if (ongoingMatch != null)
                {
                    model.SideOneScore = ongoingMatch.Goals.Count(g => g.Side == TableSide.Red);
                    model.SideTwoScore = ongoingMatch.Goals.Count(g => g.Side == TableSide.Blue);
                    model.OngoingMatchId = ongoingMatch.Id;
                    model.SideOneTeamName = ongoingMatch.RedTeam.GetNameOrDefault();
                    model.SideTwoTeamName = ongoingMatch.BlueTeam.GetNameOrDefault();
                }

                modelList.Add(model);
            }
            return View(modelList);
        }

        // GET: Tables/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var table = _db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // GET: Tables/Create
        public ActionResult Create()
        {
            var model = new TableCreateViewModel
            {
                Colors = new SelectList(Enum.GetValues(typeof (Table.Color)))
            };
            return View(model);
        }

        // POST: Tables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TableCreateViewModel model)
        {
            model.Validate(ModelState);
            if (ModelState.IsValid)
            {
                var table = new Table
                {
                    Name = model.Name,
                    Location = model.Location,
                    SideOneColor = model.SideOneColor,
                    SideTwoColor = model.SideTwoColor
                };
                _db.Tables.Add(table);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            model.Colors = new SelectList(Enum.GetValues(typeof (Table.Color)));
            return View(model);
        }

        // GET: Tables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var table = _db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            var model = new TableCreateViewModel
            {
                Id = table.Id,
                Location = table.Location,
                Name = table.Location,
                Guid = table.Guid,
                SideOneColor = table.SideOneColor,
                SideTwoColor = table.SideTwoColor,
                Colors = new SelectList(Enum.GetValues(typeof (Table.Color)))
            };

            return View(model);
        }

        // POST: Tables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TableCreateViewModel model)
        {
            model.Validate(ModelState);
            if (ModelState.IsValid)
            {
                var table = _db.Tables.Find(model.Id);
                table.Name = model.Name;
                table.Location = model.Location;
                table.SideOneColor = model.SideOneColor;
                table.SideTwoColor = model.SideTwoColor;
                _db.Entry(table).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            model.Colors = new SelectList(Enum.GetValues(typeof (Table.Color)));
            return View(model);
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