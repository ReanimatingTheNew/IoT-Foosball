using System.Linq;
using Referee.Entities;
using Referee.Web.DataContexts;

namespace Referee.Web.DataServices
{
    public class TeamService
    {
        public static Team Get(RefereeDbContext db, int aPlayerId, int? anotherPlayerId)
        {
            Team team;
            if (anotherPlayerId != null)
            {
                team =
                    db.Teams.FirstOrDefault(t => t.FirstPlayer.Id == aPlayerId && t.SecondPlayer.Id == anotherPlayerId
                                                 ||
                                                 t.FirstPlayer.Id == anotherPlayerId && t.SecondPlayer.Id == aPlayerId);
            }
            else
            {
                team = db.Teams.FirstOrDefault(t => t.FirstPlayer.Id == aPlayerId);
            }

            if (team == null)
            {
                var firstPlayer = db.Players.Find(aPlayerId);
                if (anotherPlayerId == null)
                {
                    team = new Team {FirstPlayer = firstPlayer};
                }
                else
                {
                    var secondPlayer = db.Players.Find(anotherPlayerId);
                    team = new Team {FirstPlayer = firstPlayer, SecondPlayer = secondPlayer};
                }

                db.Teams.Add(team);
                db.SaveChanges();
            }

            return team;
        }
    }
}