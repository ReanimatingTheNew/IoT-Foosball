using Referee.Entities;

namespace Statistics
{
    public class PlayerRatingEntry
    {
        public PlayerRatingEntry(Player player, double rating)
        {
            Player = player;
            Rating = rating;
        }

        public Player Player { get; }
        public double Rating { get; }
    }
}