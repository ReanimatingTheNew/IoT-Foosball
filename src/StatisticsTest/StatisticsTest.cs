using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Referee.Entities;
using Statistics;

namespace StatisticsTest
{
    [TestClass]
    public class StatisticsTest
    {
        private readonly double _delta = .00001d;
        private int _playerId = 1;

        [TestMethod]
        public void ShortestMatchCalculatorShouldReportTheGameThatEndedFastest()
        {
            var calc = new ShortestMatchCalculator();

            var testMatches = CreateTestMatchScenario();
            foreach (var match in testMatches)
            {
                calc.CalculateStatisticsForMatch(match);
            }

            Assert.AreEqual(testMatches[1], calc.ShortestMatch);
        }

        private List<Match> CreateTestMatchScenario()
        {
            var oneHourMatch = CreateValidTestMatch(DateTime.Now, DateTime.Now.AddHours(1), DateTime.Now.AddMinutes(1));
            var oneMinuteMatch = CreateValidTestMatch(DateTime.Now, DateTime.Now.AddMinutes(1),
                DateTime.Now.AddSeconds(3));
            var oneDayMatch = CreateValidTestMatch(DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddHours(1));

            return new List<Match> {oneHourMatch, oneMinuteMatch, oneDayMatch};
        }

        private Match CreateValidTestMatch(DateTime startTime, DateTime endTime, DateTime firstGoalTime)
        {
            var m = new Match
            {
                Goals = new List<Goal>(),
                StartTime = startTime,
                EndTime = endTime
            };

            m.Goals.Add(CreateValidTestGoal(firstGoalTime));

            return m;
        }

        private Goal CreateValidTestGoal(DateTime firstGoalTime)
        {
            return new Goal
            {
                Time = firstGoalTime
            };
        }

        [TestMethod]
        public void FastestGoalCalculatorShouldReportGameWithFastestGoal()
        {
            var calc = new FastestGoalCalculator();

            var testMatches = CreateTestMatchScenario();
            foreach (var match in testMatches)
            {
                calc.CalculateStatisticsForMatch(match);
            }

            Assert.AreEqual(testMatches[1], calc.MatchWithFastestGoal);
        }

        [TestMethod]
        public void WinExpectancyShouldBeFiftyPercentIfPlayersHaveEqualRating()
        {
            var calc = CreateValidTestRatingCalculator();
            var ratingDifference = 0d;

            Assert.AreEqual(.5d, calc.GetWinExpectancy(ratingDifference), _delta);
        }

        private static RatingCalculator CreateValidTestRatingCalculator()
        {
            return new RatingCalculator(new InMemoryRatingRepository());
        }

        [TestMethod]
        public void EquallyRatedPlayersShouldAwardTwentyFivePointsForAFullWin()
        {
            var calc = CreateValidTestRatingCalculator();
            var ratingDifference = 0d;
            var expectedRatingChange = 25d;
            var score = 1d;

            Assert.AreEqual(expectedRatingChange, calc.CalculateRatingChange(ratingDifference, score), _delta);
        }

        [TestMethod]
        public void ShouldAwardLessPointsForACloseGame()
        {
            var calc = CreateValidTestRatingCalculator();
            var ratingDifference = 0d;
            var score = .6d;

            Assert.IsTrue(calc.CalculateRatingChange(ratingDifference, score) < 25d);
        }

        [TestMethod]
        public void ShouldAwardZeroPointsForATie()
        {
            var calc = CreateValidTestRatingCalculator();
            var ratingDifference = 0d;
            var score = .5d;

            Assert.IsTrue(calc.CalculateRatingChange(ratingDifference, score).CompareTo(0d) < _delta);
        }

        [TestMethod]
        public void ShouldGiveZeroAsScoreFactorForFullLoss()
        {
            var calc = CreateValidTestRatingCalculator();

            var match = CreateValidTestMatchWithGoals(0, 10);

            Assert.IsTrue(calc.CalculateScoreFactor(match, TableSide.Blue).CompareTo(0) < _delta);
        }

        [TestMethod]
        public void ShouldGiveOneAsScoreFactorForFullWinForRed()
        {
            var calc = CreateValidTestRatingCalculator();

            var match = CreateValidTestMatchWithGoals(0, 10);

            Assert.IsTrue(calc.CalculateScoreFactor(match, TableSide.Red).CompareTo(1) < _delta);
        }

        [TestMethod]
        public void ShouldGiveScoreFactorBetweenPointFiveAndPointSixForSmallestPossibleWin()
        {
            var calc = CreateValidTestRatingCalculator();

            var match = CreateValidTestMatchWithGoals(10, 9);

            var scoreFactor = calc.CalculateScoreFactor(match, TableSide.Blue);

            Assert.IsTrue(scoreFactor < .6);
            Assert.IsTrue(scoreFactor > .5);
        }

        [TestMethod]
        public void ShouldReturnOneThousandForTeamWithNoGames()
        {
            Assert.AreEqual(CreateValidTestRatingCalculator().GetTeamRating(CreateValidTestTeam(1)), 1000d);
        }

        [TestMethod]
        public void AfterRatingFullWinWinningTeamShouldHaveGainedTwentyFiveRating()
        {
            var winningTeam = CreateValidTestTeam(1);
            var losingTeam = CreateValidTestTeam(2);
            var match = CreateValidTestMatchWithGoalsAndTeams(10, 0, winningTeam, losingTeam);

            var calc = CreateValidTestRatingCalculator();
            calc.CalculateStatisticsForMatch(match);

            Assert.AreEqual(calc.GetTeamRating(winningTeam), 1000 + 25, _delta);
        }

        [TestMethod]
        public void ShouldReturnOneThousandForPlayerWithNoGames()
        {
            Assert.AreEqual(CreateValidTestRatingCalculator().GetPlayerRating(CreateValidTestPlayer(1)), 1000d);
        }

        [TestMethod]
        public void AfterRatingFullWinWinningPlayersShouldHaveGainedTwentyFiveRating()
        {
            var winningPlayer1 = CreateValidTestPlayer(1);
            var winningPlayer2 = CreateValidTestPlayer(2);
            var winningTeam = CreateValidTestTeam(1, winningPlayer1, winningPlayer2);
            var losingTeam = CreateValidTestTeam(2, new Player {Id = 3});
            var match = CreateValidTestMatchWithGoalsAndTeams(10, 0, winningTeam, losingTeam);

            var calc = CreateValidTestRatingCalculator();
            calc.CalculateStatisticsForMatch(match);

            Assert.AreEqual(calc.GetPlayerRating(winningPlayer1), 1000 + 25, _delta);
            Assert.AreEqual(calc.GetPlayerRating(winningPlayer2), 1000 + 25, _delta);
        }

        [TestMethod]
        public void AfterRatingFullWinLosingPlayerShouldHaveLostTwentyFiveRating()
        {
            var winningPlayer1 = CreateValidTestPlayer(1);
            var winningPlayer2 = CreateValidTestPlayer(2);
            var losingPlayer = CreateValidTestPlayer(3);
            var winningTeam = CreateValidTestTeam(1, winningPlayer1, winningPlayer2);
            var losingTeam = CreateValidTestTeam(2, losingPlayer);
            var match = CreateValidTestMatchWithGoalsAndTeams(10, 0, winningTeam, losingTeam);

            var calc = CreateValidTestRatingCalculator();
            calc.CalculateStatisticsForMatch(match);

            Assert.AreEqual(975d, calc.GetPlayerRating(losingPlayer), _delta);
        }

        [TestMethod]
        public void GetTeamRatingListShouldReturnTheListOfAllTeamsSortedByRating()
        {
            var winningTeam = CreateValidTestTeam(1);
            var losingTeam = CreateValidTestTeam(2);
            var match = CreateValidTestMatchWithGoalsAndTeams(10, 0, winningTeam, losingTeam);

            var calc = CreateValidTestRatingCalculator();
            calc.CalculateStatisticsForMatch(match);

            var ratingList = calc.GetTeamRatingList();

            Assert.IsTrue(ratingList.Count == 2);
            Assert.AreEqual(winningTeam.Id, ratingList[0].Team.Id);
            Assert.AreEqual(losingTeam.Id, ratingList[1].Team.Id);
        }

        [TestMethod]
        public void GetPlayerRatingListShouldReturnTheLostOfAllPlayersSortedByRating()
        {
            var winningPlayer1 = CreateValidTestPlayer(1);
            var winningPlayer2 = CreateValidTestPlayer(2);
            var losingPlayer = CreateValidTestPlayer(3);
            var winningTeam = CreateValidTestTeam(1, winningPlayer1, winningPlayer2);
            var losingTeam = CreateValidTestTeam(2, losingPlayer);
            var match = CreateValidTestMatchWithGoalsAndTeams(10, 0, winningTeam, losingTeam);

            var calc = CreateValidTestRatingCalculator();
            calc.CalculateStatisticsForMatch(match);

            var ratingList = calc.GetPlayerRatingList();

            Assert.IsTrue(ratingList.Count == 3);
            Assert.AreEqual(winningPlayer1.Id, ratingList[0].Player.Id);
            Assert.AreEqual(losingPlayer.Id, ratingList[2].Player.Id);
        }

        private Player CreateValidTestPlayer(int id)
        {
            return new Player {Id = id};
        }

        private Match CreateValidTestMatchWithGoalsAndTeams(int redGoals, int blueGoals, Team redTeam, Team blueTeam)
        {
            var match = CreateValidTestMatchWithGoals(redGoals: redGoals, blueGoals: blueGoals);
            match.BlueTeam = blueTeam;
            match.RedTeam = redTeam;

            return match;
        }

        private Team CreateValidTestTeam(int id, Player firstPlayer = null, Player secondPlayer = null)
        {
            if (firstPlayer == null)
            {
                firstPlayer = new Player {Id = _playerId++};
            }

            var t = new Team
            {
                Id = id,
                FirstPlayer = firstPlayer,
                SecondPlayer = secondPlayer
            };

            return t;
        }

        private Match CreateValidTestMatchWithGoals(int blueGoals, int redGoals)
        {
            var m = CreateValidTestMatch(DateTime.Now, DateTime.Now.AddMinutes(20), DateTime.Now.AddMinutes(1));
            m.Goals.Clear();

            for (var i = 0; i < blueGoals; i++)
            {
                var g = CreateValidTestGoal(DateTime.Now.AddMinutes(i));
                g.Side = TableSide.Blue;
                m.Goals.Add(g);
            }

            for (var i = 0; i < redGoals; i++)
            {
                var g = CreateValidTestGoal(DateTime.Now.AddMinutes(i));
                g.Side = TableSide.Red;
                m.Goals.Add(g);
            }

            return m;
        }
    }
}