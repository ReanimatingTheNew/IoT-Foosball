using System.Collections.Generic;
using Referee.Entities;

namespace Statistics
{
    public interface IRatingRepository
    {
        double GetTeamRating(Team team);
        void SetTeamRating(Team team, double rating);
        List<TeamRatingEntry> GetTeamRatingList();
        double GetPlayerRating(Player player);
        void SetPlayerRating(Player player, double d);
        List<PlayerRatingEntry> GetPlayerRatingList();
    }
}