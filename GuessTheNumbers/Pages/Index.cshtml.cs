using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GuessTheNumbers.Pages
{
    public class IndexModel(ILogger<IndexModel> logger, TimeProvider timeProvider, Evaluator evaluator) : PageModel
    {
        private const string ChallengeSessionKey = "challenge";
        private const int ChallengeSize = 4;

        private readonly ILogger<IndexModel> _logger = logger;
        private readonly TimeProvider timeProvider = timeProvider;
        private static readonly Random Random = new Random();

        public IEnumerable<AttemptedViewModel>  Attempts { get; private set; } = Enumerable.Empty<AttemptedViewModel>();
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
                    Solution = GenerateNumber(ChallengeSize)
                };

                this.HttpContext.Session.Set(ChallengeSessionKey, challenge);
            }

            Attempts = Attempts = challenge.Attempts.Select(x => new AttemptedViewModel
            {
                Attempt = x,
                Evaluation = evaluator.Evaluate(x, challenge.Solution)
            });
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

            Attempts = challenge.Attempts.Select(x => new AttemptedViewModel
            {
                Attempt = x,
                Evaluation = evaluator.Evaluate(x, challenge.Solution)
            });
            Solved = challenge.Solved;

            return Page();
        }

        private static string GenerateNumber(int size)
        {
            var digits = new HashSet<char>(size);
            do
            {
                var rand = Random.Next(10);
                var @char = (char)(rand + '0');
                if (!digits.Contains(@char))
                {
                    digits.Add(@char);
                }
            } while (digits.Count < size);

            return new string(digits.ToArray()); // find better allocation
        }
    }
}
