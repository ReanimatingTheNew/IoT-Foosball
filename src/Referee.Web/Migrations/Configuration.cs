using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using Referee.Entities;
using Referee.Web.DataContexts;

namespace Referee.Web.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<RefereeDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        private List<Goal> CreateRandomGoals()
        {
            var random = new Random();
            var goals = new List<Goal>();
            for (var i = 0; i < 10; i++)
            {
                goals.Add(new Goal
                {
                    Side = random.NextDouble() < 0.5 ? TableSide.Blue : TableSide.Red,
                    Time = DateTime.Now.AddMinutes(-i)
                });
            }

            return goals;
        }

        protected override void Seed(RefereeDbContext context)
        {
            var tables = GetTables();
            context.Tables.AddOrUpdate(tables.ToArray());

            var players = GetPlayers();
            context.Players.AddOrUpdate(players.ToArray());

            var teams = CreateTeams(players);
            context.Teams.AddOrUpdate(teams.ToArray());

            var matches = GetMatches(tables, teams);
            context.Matches.AddOrUpdate(matches.ToArray());
        }

        private static List<Team> CreateTeams(List<Player> players)
        {
            return new List<Team>
            {
                new Team {FirstPlayer = players[0], SecondPlayer = players[1], Name = "TEAM WRECKING BALL"},
                new Team {FirstPlayer = players[2], SecondPlayer = players[3], Name = "TEAM AWSUM"},
                new Team {FirstPlayer = players[4], SecondPlayer = players[5], Name = "LOCAL SPORTS TEAM #5"},
                new Team {FirstPlayer = players[6], SecondPlayer = players[7], Name = "The Last Team"}
            };
        }

        private static List<Table> GetTables()
        {
            return new List<Table>
            {
                new Table {Name = "Old Faithful", Location = "B1.01"},
                new Table {Name = "New Faithful", Location = "C2.02"},
                new Table {Name = "Back UP table", Location = "D3.03"},
                new Table {Name = "Broken table", Location = "E4.04"},
                new Table
                {
                    Name = "Hackathon table",
                    Location = "The Hub",
                    Guid = Guid.Parse("250cebea-416c-47c6-900c-14b1df1ad4bf")
                }
            };
        }

        private static List<Player> GetPlayers()
        {
            return new List<Player>
            {
                new Player {Name = "Jeroen", AccountEmail = "jeroen@iosfoosball.com"},
                new Player {Name = "Vaidas", AccountEmail = "vaidas@iosfoosball.com"},
                new Player {Name = "Martin", AccountEmail = "martin@iosfoosball.com"},
                new Player {Name = "Sam", AccountEmail = "sam@iosfoosball.com"},
                new Player {Name = "Jamie", AccountEmail = "jamie@iosfoosball.com"},
                new Player {Name = "Michal", AccountEmail = "michal@iosfoosball.com"},
                new Player {Name = "Anders", AccountEmail = "anders@iosfoosball.com"},
                new Player {Name = "Extra Guy", AccountEmail = "extraguy@iosfoosball.com"}
            };
        }

        private List<Match> GetMatches(List<Table> tables, List<Team> teams)
        {
            return new List<Match>
            {
                //3 matches for table 0
                new Match
                {
                    BlueTeam = teams[0],
                    RedTeam = teams[1],
                    StartTime = DateTime.Now.AddMinutes(-20),
                    EndTime = DateTime.Now.AddMinutes(-10),
                    Goals = CreateRandomGoals(),
                    Table = tables[0],
                    IsFinished = true,
                    Creator = teams[1].FirstPlayer
                },
                new Match
                {
                    BlueTeam = teams[0],
                    RedTeam = teams[1],
                    StartTime = DateTime.Now.AddMinutes(-40),
                    EndTime = DateTime.Now.AddMinutes(-20),
                    Goals = CreateRandomGoals(),
                    Table = tables[0],
                    IsFinished = true,
                    Creator = teams[1].FirstPlayer
                },
                new Match
                {
                    BlueTeam = teams[2],
                    RedTeam = teams[1],
                    StartTime = DateTime.Now.AddMinutes(-60),
                    EndTime = DateTime.Now.AddMinutes(-40),
                    Goals = CreateRandomGoals(),
                    Table = tables[0],
                    IsFinished = true,
                    Creator = teams[2].FirstPlayer
                },
                //3 matches for table 1
                new Match
                {
                    BlueTeam = teams[0],
                    RedTeam = teams[1],
                    StartTime = DateTime.Now.AddMinutes(-20),
                    EndTime = DateTime.Now.AddMinutes(-10),
                    Goals = CreateRandomGoals(),
                    Table = tables[1],
                    IsFinished = true,
                    Creator = teams[2].FirstPlayer
                },
                new Match
                {
                    BlueTeam = teams[0],
                    RedTeam = teams[1],
                    StartTime = DateTime.Now.AddMinutes(-40),
                    EndTime = DateTime.Now.AddMinutes(-20),
                    Goals = CreateRandomGoals(),
                    Table = tables[1],
                    IsFinished = true,
                    Creator = teams[2].FirstPlayer
                },
                new Match
                {
                    BlueTeam = teams[2],
                    RedTeam = teams[1],
                    StartTime = DateTime.Now.AddMinutes(-60),
                    EndTime = DateTime.Now.AddMinutes(-40),
                    Goals = CreateRandomGoals(),
                    Table = tables[1],
                    IsFinished = true,
                    Creator = teams[2].FirstPlayer
                },
                //Ongoing game for table 2
                new Match
                {
                    BlueTeam = teams[0],
                    RedTeam = teams[1],
                    StartTime = DateTime.Now.AddMinutes(-10),
                    Goals = CreateRandomGoals(),
                    Table = tables[2],
                    IsFinished = false,
                    Creator = teams[2].FirstPlayer
                },
                //Ongoing and ended game for table 3
                new Match
                {
                    BlueTeam = teams[1],
                    RedTeam = teams[3],
                    StartTime = DateTime.Now.AddMinutes(-10),
                    EndTime = DateTime.Now.AddMinutes(-20),
                    Goals = CreateRandomGoals(),
                    Table = tables[3],
                    IsFinished = true,
                    Creator = teams[2].FirstPlayer
                }
            };
        }
    }
}