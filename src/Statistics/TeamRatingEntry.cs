using Referee.Entities;

namespace Statistics
{
    public class TeamRatingEntry
    {
        public TeamRatingEntry(Team team, double rating)
        {
            Team = team;
            Rating = rating;
        }

        public Team Team { get; }
        public double Rating { get; }
    }
}