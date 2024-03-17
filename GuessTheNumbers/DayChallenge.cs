namespace GuessTheNumbers
{
    public class DayChallenge
    {
        public DateOnly Date { get; set; }
        public string Solution { get; set; } = string.Empty;
        public List<string> Attempts { get; set; } = new List<string> { };
        public int NumberAttempts { get; set; }
        public bool Solved { get; set; }
    }
}
