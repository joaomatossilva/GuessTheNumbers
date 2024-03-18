namespace GuessTheNumbers
{
    public class Evaluator
    {
        public Evaluation Evaluate(string attempt, string answer)
        {
            int numbersInTheRightPlace = 0, numbersInWrongPlace = 0;
            for (int attemptIndex = 0; attemptIndex < attempt.Length; attemptIndex++)
            {
                char attemptChar = attempt[attemptIndex];
                if (attemptChar == answer[attemptIndex])
                {
                    numbersInTheRightPlace++;
                }
                else if(answer.Any(c => attemptChar == c))
                {
                    numbersInWrongPlace++;
                }
            }

            return new Evaluation
            {
                NumbersInRightPlace = numbersInTheRightPlace,
                NumbersInWrongPlace = numbersInWrongPlace
            };
        }
    }

    public struct Evaluation
    {
        public int NumbersInRightPlace { get; init; }
        public int NumbersInWrongPlace { get; init; }
    }
}
