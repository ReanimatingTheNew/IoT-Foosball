using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Referee.Entities;
using Referee.Web.DataContexts;
using Referee.Web.DataServices;
using Referee.Web.Models;
using Match = Referee.Web.Models.Match;

namespace Referee.Web.Controllers.WebAPI
{
    public class TablesController : ApiController
    {
        private readonly RefereeDbContext _db = new RefereeDbContext();


        // GET: api/Tables
        public HttpResponseMessage GetTables()
        {
            var modelList = new List<TableApiViewModel>();
            var tables = _db.Tables.ToList();
            foreach (var table in tables)
            {
                var model = new TableApiViewModel
                {
                    Guid = table.Guid,
                    Location = table.Location,
                    Name = table.Name,
                    Match = MatchService.HasOngoingMatch(_db, table) ? new Match() : null
                };

                var ongoingMatch = MatchService.HasOngoingMatch(_db, table)
                    ? MatchService.GetOngoingMatch(_db, table)
                    : null;

                if (ongoingMatch != null)
                {
                    model.Match.SideOneScore = ongoingMatch.Goals.Count(g => g.Side == TableSide.Red);
                    model.Match.SideTwoScore = ongoingMatch.Goals.Count(g => g.Side == TableSide.Blue);
                    model.Match.OngoingMatchId = ongoingMatch.Id;
                    model.Match.SideOneTeamName = ongoingMatch.RedTeam.GetNameOrDefault();
                    model.Match.SideTwoTeamName = ongoingMatch.BlueTeam.GetNameOrDefault();
                }

                modelList.Add(model);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK, modelList);

            return response;
        }

        // GET: api/Tables/5
        [ResponseType(typeof (Table))]
        public async Task<IHttpActionResult> GetTable(int id)
        {
            var table = await _db.Tables.FindAsync(id);
            if (table == null)
            {
                return NotFound();
            }

            return Ok(table);
        }

        // PUT: api/Tables/5
        [ResponseType(typeof (void))]
        public async Task<IHttpActionResult> PutTable(int id, Table table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != table.Id)
            {
                return BadRequest();
            }

            _db.Entry(table).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TableExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Tables
        [ResponseType(typeof (Table))]
        public async Task<IHttpActionResult> PostTable(Table table)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Tables.Add(table);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new {id = table.Id}, table);
        }

        // DELETE: api/Tables/5
        [ResponseType(typeof (Table))]
        public async Task<IHttpActionResult> DeleteTable(int id)
        {
            var table = await _db.Tables.FindAsync(id);
            if (table == null)
            {
                return NotFound();
            }

            _db.Tables.Remove(table);
            await _db.SaveChangesAsync();

            return Ok(table);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TableExists(int id)
        {
            return _db.Tables.Count(e => e.Id == id) > 0;
        }
    }
}