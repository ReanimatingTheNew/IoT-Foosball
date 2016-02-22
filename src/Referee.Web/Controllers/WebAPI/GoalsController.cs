using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Referee.Entities;
using Referee.Web.DataContexts;

namespace Referee.Web.Controllers.WebAPI
{
    public class GoalsController : ApiController
    {
        private readonly RefereeDbContext _db = new RefereeDbContext();
        // PUT: api/Goals/12/0
        [Route("api/Goals/{tableGuid}/{side}")]
        [ResponseType(typeof (object))]
        public IHttpActionResult PutGoal(Guid tableGuid, TableSide side)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var table = _db.Tables.Single(t => t.Guid == tableGuid);
            var match = table.ActiveMatch();

            if (match == null)
            {
                return Ok(Match.NoGameSummary());
            }

            if (match.ScoreGoal(side))
            {
                //HACK BEGINS: Force resolving RedTeam and BlueTeam
                var redName = match.RedTeam.Name;
                var blueName = match.BlueTeam.Name;
                var creator = match.Creator.Name;
                //HACK ENDS 

                _db.SaveChanges();
                return Ok(match.Summary());
            }
            return Ok(Match.NoGameSummary());
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