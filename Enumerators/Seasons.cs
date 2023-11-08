namespace terraguardians
{
    public enum Seasons : byte
    {
        Summer = 0,
        Autumn = 1,
        Winter = 2,
        Spring = 3
    }

    public class SeasonTranslator
    {
        public static string Translate(Seasons season)
        {
            switch(season)
            {
                case Seasons.Summer:
                    return "Safari";
                case Seasons.Autumn:
                    return "Harvest";
                case Seasons.Winter:
                    return "Hibernate";
                case Seasons.Spring:
                    return "Blooming";
            }
            return "Unknown Season";
        }
    }
}