using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Referee.Entities;
using Referee.Web.DataContexts;
using Statistics;

namespace Referee.Web.Controllers.MVC
{
    public class RankingController : Controller
    {
        private readonly RefereeDbContext _db = new RefereeDbContext();

        private IEnumerable<Match> GetMatchesInHistoricalOrder()
        {
            var matches = _db.Matches
                .Where(m => m.IsFinished)
                .Include(m => m.BlueTeam)
                .Include(m => m.BlueTeam.FirstPlayer)
                .Include(m => m.BlueTeam.SecondPlayer)
                .Include(m => m.RedTeam)
                .Include(m => m.RedTeam.FirstPlayer)
                .Include(m => m.RedTeam.SecondPlayer)
                .Include(m => m.Goals)
                .OrderBy(m => m.EndTime);

            return matches;
        }

        [AllowAnonymous]
        // GET: Ranking/Teams
        public ActionResult Teams()
        {
            var ratingCalculator = new RatingCalculator(new InMemoryRatingRepository());

            foreach (var match in GetMatchesInHistoricalOrder())
            {
                ratingCalculator.CalculateStatisticsForMatch(match);
            }

            var model = ratingCalculator.GetTeamRatingList();

            return View(model);
        }

        [AllowAnonymous]
        // GET: Ranking/Teams
        public ActionResult Players()
        {
            var ratingCalculator = new RatingCalculator(new InMemoryRatingRepository());

            foreach (var match in GetMatchesInHistoricalOrder())
            {
                ratingCalculator.CalculateStatisticsForMatch(match);
            }

            var model = ratingCalculator.GetPlayerRatingList();

            return View(model);
        }
    }
}