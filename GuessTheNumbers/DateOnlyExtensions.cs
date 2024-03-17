namespace GuessTheNumbers
{
    public static class DateOnlyExtensions
    {
        public static bool IsToday(this DateOnly dateOnly)
        {
            return dateOnly.IsToday(TimeProvider.System);
        }

        public static bool IsToday(this DateOnly dateOnly, TimeProvider timeProvider)
        {
            timeProvider.GetUtcNow();
            var today = DateTime.Today;
            return dateOnly.Year == today.Year &&
                dateOnly.Month == today.Month &&
                dateOnly.Day == today.Day;
        }
    }
}
