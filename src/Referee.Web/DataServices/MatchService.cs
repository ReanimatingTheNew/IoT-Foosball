using System.Linq;
using Referee.Entities;
using Referee.Web.DataContexts;

namespace Referee.Web.DataServices
{
    public class MatchService
    {
        public static Match GetOngoingMatch(RefereeDbContext db, Table table)
        {
            return db.Matches.FirstOrDefault(m => !m.IsFinished && m.Table.Id == table.Id);
        }

        public static bool HasOngoingMatch(RefereeDbContext db, Table table)
        {
            return GetOngoingMatch(db, table) != null;
        }
    }
}