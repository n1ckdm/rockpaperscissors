namespace RPS.Domain;

public interface ICommand
{
    Guid AggregateId { get; }
}
public class Commands
{
    public readonly record struct CreateGameCommand(
        Guid GameId,
        string PlayerEmail
    ) : ICommand { public Guid AggregateId => GameId; }

    public readonly record struct MakeMoveCommand(
        Guid GameId,
        string PlayerEmail,
        Move Move
    ) : ICommand { public Guid AggregateId => GameId; }
}
