using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Referee.Entities;

namespace Referee.EntitiesTests
{
    [TestClass]
    public class MatchTests
    {
        public Match CreateMatch()
        {
            var match = new Match
            {
                RedTeam = new Team
                {
                    FirstPlayer = new Player {Name = "FirstA"},
                    SecondPlayer = new Player {Name = "SecondA"}
                },
                BlueTeam = new Team
                {
                    FirstPlayer = new Player {Name = "FirstB"},
                    SecondPlayer = new Player {Name = "SecondB"}
                },
                Table = new Table {SideOneColor = Table.Color.Orange, SideTwoColor = Table.Color.Blue},
                Goals = new List<Goal>()
            };

            return match;
        }

        [TestMethod]
        public void CurrentTaunt_FirstGoal_PlayOn()
        {
            var match = CreateMatch();
            match.ScoreGoal(TableSide.Red);

            Assert.AreEqual(Taunt.PlayOn, match.CurrentTaunt());
        }

        [TestMethod]
        public void CurrentTaunt_ThreeInARow_Dominating()
        {
            var match = CreateMatch();

            match.ScoreGoal(TableSide.Red);
            match.ScoreGoal(TableSide.Red);
            match.ScoreGoal(TableSide.Red);

            Assert.AreEqual(Taunt.Dominating, match.CurrentTaunt());
        }

        [TestMethod]
        public void CurrentTaunt_NotEnoughInARow_PlayOn()
        {
            var match = CreateMatch();

            match.ScoreGoal(TableSide.Red);
            match.ScoreGoal(TableSide.Blue);
            match.ScoreGoal(TableSide.Red);
            match.ScoreGoal(TableSide.Red);

            Assert.AreEqual(Taunt.PlayOn, match.CurrentTaunt());
        }

        [TestMethod]
        public void ScoreGoal_TenthTeamGoal_GoalAddedMatchFinished()
        {
            const int goalsToWin = 10;

            var match = CreateMatch();

            for (var i = 1; i <= goalsToWin; i++)
            {
                match.ScoreGoal(TableSide.Red);
            }

            Assert.IsTrue(match.IsFinished);
            Assert.IsNotNull(match.EndTime);
            Assert.AreEqual(goalsToWin, match.Goals.Count(g => g.Side == TableSide.Red));
        }

        [TestMethod]
        public void Summary_BlueTeamSecondPlayerMissing_ValidResponse()
        {
            var match = CreateMatch();
            match.BlueTeam.SecondPlayer = null;

            var response = match.Summary();

            var jsonResponse = JsonConvert.SerializeObject(response);
            Assert.IsNotNull(response);
            Assert.IsNotNull(jsonResponse);
        }

        [TestMethod]
        public void Summary_RedTeamSecondPlayerMissing_ValidResponse()
        {
            var match = CreateMatch();
            match.RedTeam.SecondPlayer = null;

            var response = match.Summary();

            var jsonResponse = JsonConvert.SerializeObject(response);
            Assert.IsNotNull(response);
            Assert.IsNotNull(jsonResponse);
        }
    }
}