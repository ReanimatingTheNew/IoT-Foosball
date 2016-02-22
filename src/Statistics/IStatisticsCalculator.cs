using Referee.Entities;

namespace Statistics
{
    public interface IStatisticsCalculator
    {
        void CalculateStatisticsForMatch(Match match);
    }
}