using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GuessTheNumbers.Pages
{
    public class IndexModel(ILogger<IndexModel> logger, TimeProvider timeProvider) : PageModel
    {
        private const string ChallengeSessionKey = "challenge";

        private readonly ILogger<IndexModel> _logger = logger;
        private readonly TimeProvider timeProvider = timeProvider;
        private static readonly Random Random = new Random();

        public IEnumerable<string>  Attempts { get; private set; } = Enumerable.Empty<string>();
        public bool Solved { get; private set; }

        public void OnGet()
        {
            var challenge = this.HttpContext.Session.Get<DayChallenge>(ChallengeSessionKey);
            if (challenge is null || !challenge.Date.IsToday(timeProvider))
            {
                challenge = new DayChallenge
                {
                    Date = DateOnly.FromDateTime(timeProvider.GetLocalNow().Date),
                    NumberAttempts = 0,
                    Solved = false,
                    Solution = Random.Next(10_000).ToString() // Make this a better function no numbers repeated 
                };

                this.HttpContext.Session.Set(ChallengeSessionKey, challenge);
            }

            Attempts = challenge.Attempts;
            Solved = challenge.Solved;
        }

        public ActionResult OnPost(string attempt)
        {
            var challenge = this.HttpContext.Session.Get<DayChallenge>(ChallengeSessionKey);
            if (challenge is null || !challenge.Date.IsToday(timeProvider) || challenge.Solved)
            {
                // No challenge - redirect to get inded to create a new challenge
                return RedirectToPage();
            }

            challenge.Attempts.Add(attempt);
            challenge.NumberAttempts++;

            if (challenge.Solution == attempt)
            {
                challenge.Solved = true;
            }

            this.HttpContext.Session.Set(ChallengeSessionKey, challenge);

            Attempts = challenge.Attempts;
            Solved = challenge.Solved;

            return Page();
        }
    }
}
