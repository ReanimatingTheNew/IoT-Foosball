using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Referee.Resources;

namespace Referee.Entities
{
    public enum Taunt
    {
        Equalizer,
        Dominating,
        CrushingWin,
        UnderTheTable,
        Finally,
        ComboBreaker,
        LiverpoolComeback,
        GameOver,
        PlayOn
    }

    public class Match
    {
        private const int GoalsToVictory = 10;
        private const int GoalsToDominate = 3;
        private const int GoalsToCrush = 5;

        private readonly Dictionary<Taunt, string> _tauntTexts = new Dictionary<Taunt, string>
        {
            {Taunt.Equalizer, "Equalizer!"},
            {Taunt.Dominating, "Dominating!"},
            {Taunt.CrushingWin, "Crushing win!"},
            {Taunt.UnderTheTable, "Under the table!"},
            {Taunt.Finally, "Finally! It's been a while!"},
            {Taunt.ComboBreaker, "C-c-c-combo breaker!"},
            {Taunt.LiverpoolComeback, "Liverpool comeback!"},
            {Taunt.GameOver, "Game over!"},
            {Taunt.PlayOn, "Play on!"}
        };

        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Match()
        {
        }

        [Key, Required]
        public int Id { get; set; }

        [Required, Display(ResourceType = typeof (Resource), Name = "Match_IsFinished_Finished")]
        public bool IsFinished { get; set; }

        [Required, Display(ResourceType = typeof (Resource), Name = "Match_StartTime_Start_Time")]
        public DateTime StartTime { get; set; }

        [Display(ResourceType = typeof (Resource), Name = "Match_EndTime_End_Time")]
        public DateTime? EndTime { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Goal> Goals { get; set; }

        [Required]
        public virtual Table Table { get; set; }

        [Required, InverseProperty("MatchesAsBlue")]
        public virtual Team RedTeam { get; set; }

        [Required, InverseProperty("MatchesAsRed")]
        public virtual Team BlueTeam { get; set; }

        [Required]
        public virtual Player Creator { get; set; }

        public bool ScoreGoal(TableSide side)
        {
            if (IsFinished)
            {
                return false;
            }

            var goal = new Goal
            {
                Side = side,
                Time = DateTime.Now
            };

            Goals.Add(goal);

            var lastGoal = Goals.Count(g => g.Side == goal.Side) >= GoalsToVictory;

            if (lastGoal)
            {
                IsFinished = true;
                EndTime = DateTime.Now;
            }

            return true;
        }

        public object Summary()
        {
            var redTeam = new
            {
                Name = RedTeam.GetNameOrDefault(),
                FirstPlayer = RedTeam.FirstPlayer.Name,
                SecondPlayer = RedTeam.SecondPlayer != null ? RedTeam.SecondPlayer.Name : "",
                Color = Table.SideOneColor.ToString()
            };

            var blueTeam = new
            {
                Name = BlueTeam.GetNameOrDefault(),
                FirstPlayer = BlueTeam.FirstPlayer.Name,
                SecondPlayer = BlueTeam.SecondPlayer != null ? BlueTeam.SecondPlayer.Name : "",
                Color = Table.SideTwoColor.ToString()
            };

            return new
            {
                Teams = new
                {
                    RedTeam = redTeam,
                    BlueTeam = blueTeam
                },
                Score = new
                {
                    RedGoals = Goals.Count(g => g.Side == TableSide.Red),
                    BlueGoals = Goals.Count(g => g.Side == TableSide.Blue)
                },
                VanityString = _tauntTexts[CurrentTaunt()]
            };
        }

        public Taunt CurrentTaunt()
        {
            var blueGoals = Goals.Count(g => g.Side == TableSide.Blue);
            var redGoals = Goals.Count(g => g.Side == TableSide.Red);

            if (IsFinished)
            {
                var redWinsNoBlueGoals = Goals.Count(g => g.Side == TableSide.Red) == GoalsToVictory
                                         && Goals.Count(g => g.Side == TableSide.Blue) == 0;
                var blueWinsNoRedGoals = Goals.Count(g => g.Side == TableSide.Blue) == GoalsToVictory
                                         && Goals.Count(g => g.Side == TableSide.Red) == 0;
                if (redWinsNoBlueGoals || blueWinsNoRedGoals)
                {
                    return Taunt.UnderTheTable;
                }

                if (Math.Abs(blueGoals - redGoals) >= GoalsToCrush)
                {
                    return Taunt.CrushingWin;
                }

                return Taunt.GameOver;
            }

            if (Goals.Count >= GoalsToDominate)
            {
                var lastGoals = LastGoals(GoalsToDominate);
                if (lastGoals.ElementAt(0).Side == lastGoals.ElementAt(1).Side
                    && lastGoals.ElementAt(1).Side == lastGoals.ElementAt(2).Side)
                {
                    return Taunt.Dominating;
                }
            }

            if (Goals.Count >= 2)
            {
                var lastTwoGoals = LastGoals(2);
                if (lastTwoGoals.ElementAt(0).Time - lastTwoGoals.ElementAt(1).Time >= TimeSpan.FromMinutes(2))
                {
                    return Taunt.Finally;
                }
            }

            if (redGoals == blueGoals)
            {
                return Taunt.Equalizer;
            }

            return Taunt.PlayOn;
        }

        private IEnumerable<Goal> LastGoals(int goalsToGet)
        {
            return Goals.OrderByDescending(g => g.Time).Take(goalsToGet);
        }

        public static object NoGameSummary()
        {
            return new
            {
                Teams = "",
                VanityString = "No current match on this table"
            };
        }
    }
}