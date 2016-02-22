namespace EmbeddedApp
{
    public class Teams
    {
        public Team BlueTeam;
        public Team RedTeam;
    }

    public class Team
    {
        public string FirstPlayer;
        public string Name;
        public string SecondPlayer;
    }

    public class Scores
    {
        public int BlueGoals;
        public int RedGoals;
    }

    public class RootObject
    {
        public Scores Score;
        public Teams Teams;
        public string VanityString;
    }
}