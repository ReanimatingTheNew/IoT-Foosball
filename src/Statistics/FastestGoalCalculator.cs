using System.Linq;
using Referee.Entities;

namespace Statistics
{
    public class FastestGoalCalculator : IStatisticsCalculator
    {
        public Match MatchWithFastestGoal { get; private set; }

        public void CalculateStatisticsForMatch(Match match)
        {
            if (MatchWithFastestGoal == null)
            {
                MatchWithFastestGoal = match;
                return;
            }

            var matchFirstGoalTimeSpan = match.Goals.ElementAt(0).Time - match.StartTime;
            var fastestGoalMatchFirstGoalTimeSpan = MatchWithFastestGoal.Goals.ElementAt(0).Time -
                                                    MatchWithFastestGoal.StartTime;

            MatchWithFastestGoal = matchFirstGoalTimeSpan < fastestGoalMatchFirstGoalTimeSpan
                ? match
                : MatchWithFastestGoal;
        }
    }
}