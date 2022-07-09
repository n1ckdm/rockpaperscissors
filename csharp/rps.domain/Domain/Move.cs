namespace RPS.Domain
{
    public class Move
    {
        private EMove _move;
        private enum EMove { Rock, Paper, Scissors }

        public static readonly Move Rock = new Move() { _move = EMove.Rock };
        public static readonly Move Paper = new Move() { _move = EMove.Paper };
        public static readonly Move Scissors = new Move() { _move = EMove.Scissors };

        public bool Defeats(Move other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            switch (_move)
            {
                case EMove.Rock:
                    return other._move == EMove.Scissors;
                case EMove.Paper:
                    return other._move == EMove.Rock;
                case EMove.Scissors:
                    return other._move == EMove.Paper;
                default:
                    return false;
            }
            throw new InvalidOperationException("Unknown move");
        }
    }
}