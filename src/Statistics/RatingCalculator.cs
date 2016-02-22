using System;
using System.Collections.Generic;
using System.Linq;
using Referee.Entities;

namespace Statistics
{
    public class RatingCalculator : IStatisticsCalculator
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingCalculator(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public void CalculateStatisticsForMatch(Match match)
        {
            CalculateTeamRating(match);
            CalculatePlayerRating(match);
        }

        private void CalculatePlayerRating(Match match)
        {
            var redTeamAverageRating = GetAveragePlayerRating(match.RedTeam);
            var blueTeamAverageRating = GetAveragePlayerRating(match.BlueTeam);

            var ratingDifference = Math.Abs(redTeamAverageRating - blueTeamAverageRating);

            //rate blue
            var scoreFactorBlue = CalculateScoreFactor(match, TableSide.Blue);
            var ratingChangeBlue = CalculateRatingChange(ratingDifference, scoreFactorBlue);

            foreach (var player in match.BlueTeam.GetPlayers())
            {
                var currentPlayerRating = _ratingRepository.GetPlayerRating(player);
                _ratingRepository.SetPlayerRating(player, currentPlayerRating + ratingChangeBlue);
            }

            //rate red
            var scoreFactorRed = CalculateScoreFactor(match, TableSide.Red);
            var ratingChangeRed = CalculateRatingChange(ratingDifference, scoreFactorRed);

            foreach (var player in match.RedTeam.GetPlayers())
            {
                var currentPlayerRating = _ratingRepository.GetPlayerRating(player);
                _ratingRepository.SetPlayerRating(player, currentPlayerRating + ratingChangeRed);
            }
        }

        private double GetAveragePlayerRating(Team team)
        {
            return team.GetPlayers().Average(p => _ratingRepository.GetPlayerRating(p));
        }

        private void CalculateTeamRating(Match match)
        {
            var redTeamCurrentRating = _ratingRepository.GetTeamRating(match.RedTeam);
            var blueTeamCurrentRating = _ratingRepository.GetTeamRating(match.BlueTeam);

            var ratingDifference = Math.Abs(redTeamCurrentRating - blueTeamCurrentRating);

            //rate blue
            var scoreFactorBlue = CalculateScoreFactor(match, TableSide.Blue);
            var ratingChangeBlue = CalculateRatingChange(ratingDifference, scoreFactorBlue);
            _ratingRepository.SetTeamRating(match.BlueTeam, blueTeamCurrentRating + ratingChangeBlue);

            //rate red
            var scoreFactorRed = CalculateScoreFactor(match, TableSide.Red);
            var ratingChangeRed = CalculateRatingChange(ratingDifference, scoreFactorRed);
            _ratingRepository.SetTeamRating(match.RedTeam, redTeamCurrentRating + ratingChangeRed);
        }

        /// <summary>
        /// </summary>
        /// <param name="ratingDifference"></param>
        /// <param name="score">positive 1 if the favorites won, negative 1 otherwise</param>
        /// <returns></returns>
        public double CalculateRatingChange(double ratingDifference, double score)
        {
            double k = 50;
            //http://www.bonziniusa.com/foosball/tournament/TournamentRankingSystem.html

            var winExpectancy = GetWinExpectancy(ratingDifference);
            return k*(score - winExpectancy);
        }

        public double GetWinExpectancy(double ratingDifference)
        {
            double f = 1000;
            var d = ratingDifference;
            var winExpectancy = 1.0/(Math.Pow(10, -d/f) + 1);
            return winExpectancy;
        }

        public double CalculateScoreFactor(Match match, TableSide side)
        {
            var goalDelta = match.Goals.Sum(goal => goal.Side == side ? 1 : -1);

            return (goalDelta + 10.0)/20.0;
        }

        public double GetTeamRating(Team team)
        {
            return _ratingRepository.GetTeamRating(team);
        }

        public List<TeamRatingEntry> GetTeamRatingList()
        {
            return _ratingRepository.GetTeamRatingList();
        }

        public double GetPlayerRating(Player player)
        {
            return _ratingRepository.GetPlayerRating(player);
        }

        public List<PlayerRatingEntry> GetPlayerRatingList()
        {
            return _ratingRepository.GetPlayerRatingList();
        }
    }
}