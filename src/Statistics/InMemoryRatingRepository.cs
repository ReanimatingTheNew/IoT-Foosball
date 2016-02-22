using System.Collections.Generic;
using System.Linq;
using Referee.Entities;

namespace Statistics
{
    public class InMemoryRatingRepository : IRatingRepository
    {
        private const double InitialRating = 1000d;
        private readonly Dictionary<int, PlayerRatingEntry> _playerRatingDb = new Dictionary<int, PlayerRatingEntry>();
        private readonly Dictionary<int, TeamRatingEntry> _ratingDb = new Dictionary<int, TeamRatingEntry>();

        public double GetTeamRating(Team team)
        {
            EnsureKey(team);

            return _ratingDb[team.Id].Rating;
        }

        public void SetTeamRating(Team team, double rating)
        {
            EnsureKey(team);

            _ratingDb[team.Id] = new TeamRatingEntry(team, rating);
        }

        public List<TeamRatingEntry> GetTeamRatingList()
        {
            return _ratingDb.Values.OrderByDescending(tre => tre.Rating)
                .ThenBy(tre => tre.Team.Name)
                .ToList();
        }

        public double GetPlayerRating(Player player)
        {
            EnsureKey(player);

            return _playerRatingDb[player.Id].Rating;
        }

        public void SetPlayerRating(Player player, double rating)
        {
            EnsureKey(player);

            _playerRatingDb[player.Id] = new PlayerRatingEntry(player, rating);
        }

        public List<PlayerRatingEntry> GetPlayerRatingList()
        {
            return _playerRatingDb.Values.OrderByDescending(pre => pre.Rating)
                .ThenBy(pre => pre.Player.Name)
                .ToList();
        }

        private void EnsureKey(Player player)
        {
            if (!_playerRatingDb.ContainsKey(player.Id))
            {
                _playerRatingDb[player.Id] = new PlayerRatingEntry(player, InitialRating);
            }
        }

        private void EnsureKey(Team team)
        {
            if (!_ratingDb.ContainsKey(team.Id))
            {
                _ratingDb[team.Id] = new TeamRatingEntry(team, InitialRating);
            }
        }
    }
}