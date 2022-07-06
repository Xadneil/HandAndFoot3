namespace HAF.WebServer.Game
{
    public static class RoundUtils
    {
        public static int PointsToMeld(int r)
        {
            return r switch
            {
                1 => 50,
                2 => 90,
                3 => 120,
                4 => 150,
                _ => 0,
            };
        }
    }
}
