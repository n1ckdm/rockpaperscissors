using static RPS.Domain.Commands;
using static RPS.Domain.Events;

namespace RPS.Domain
{
    public class Game
    {
        enum EState 
        {
            Constructed, Created, Waiting, Tied, Won
        }

        private EState State { get; init; }
        private String? Player { get; init; }
        private Move? Move { get; init; }

        public List<IEvent> handle(CreateGameCommand cmd)
        {
            if (State != EState.Constructed)
                throw new InvalidOperationException($"Game already created: {State}");

            return new List<IEvent>
            {
                new GameCreatedEvent(cmd.GameId, cmd.PlayerEmail)
            };
        }

        public List<IEvent> handle(MakeMoveCommand cmd)
        {
            if (EState.Created == State)
            {
                return new List<IEvent>
                {
                    new MoveDecidedEvent(cmd.GameId, cmd.PlayerEmail, cmd.Move)
                };
            }
            else if (EState.Waiting == State)
            {
                if (Player == cmd.PlayerEmail) throw new ArgumentException($"Player already in game: {cmd.PlayerEmail}");
                return new List<IEvent>
                {
                    new MoveDecidedEvent(cmd.GameId, cmd.PlayerEmail, cmd.Move),
                    MakeEndGameEvent(cmd.GameId, cmd.PlayerEmail, cmd.Move)
                };
            }
            else
            {
                throw new InvalidOperationException($"Invalud game state: {State}");
            }
        }

        private IEvent MakeEndGameEvent(Guid gameId, string opponentEmail, Move opponentMove)
        {
            ArgumentNullException.ThrowIfNull(this.Move);
            ArgumentNullException.ThrowIfNull(this.Player);

            if (this.Move.Defeats(opponentMove))
            {
                return new GameWonEvent(gameId, this.Player, opponentEmail);
            }
            else if (opponentMove.Defeats(Move))
            {
                return new GameWonEvent(gameId, opponentEmail, this.Player);
            }
            else
            {
                return new GameTiedEvent(gameId);
            }
        }

        public static Game Apply(Game game, IEvent evt)
        {
            switch (evt)
            {
                case GameCreatedEvent e:
                    return new Game
                    {
                        State = EState.Created,
                        Player = e.PlayerEmail,
                    };
                case MoveDecidedEvent e:
                    return new Game
                    {
                        State = EState.Waiting,
                        Player = e.PlayerEmail,
                        Move = e.Move,
                    };
                case GameWonEvent e:
                    return new Game
                    {
                        State = EState.Won,
                        Player = game.Player,
                        Move = game.Move,
                    };
                case GameTiedEvent e:
                    return new Game
                    {
                        State = EState.Tied,
                        Player = game.Player,
                        Move = game.Move,
                    };
                default:
                    throw new InvalidOperationException($"Unknown event: {evt}");
            }
        }
    }
}