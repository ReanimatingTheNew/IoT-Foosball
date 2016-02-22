using Referee.Entities;

namespace Statistics
{
    public class ShortestMatchCalculator : IStatisticsCalculator
    {
        public Match ShortestMatch { get; private set; }

        public void CalculateStatisticsForMatch(Match match)
        {
            if (ShortestMatch == null)
            {
                ShortestMatch = match;
                return;
            }

            var matchTimeSpan = match.EndTime.Value - match.StartTime;
            var shortestMatchTimeSpan = ShortestMatch.EndTime.Value - ShortestMatch.StartTime;

            ShortestMatch = matchTimeSpan < shortestMatchTimeSpan ? match : ShortestMatch;
        }
    }
}