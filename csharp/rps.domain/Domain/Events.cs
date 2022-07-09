namespace RPS.Domain;
public interface IEvent {}
public class Events
{
    public readonly record struct GameCreatedEvent(
        Guid GameId,
        string PlayerEmail
    ) : IEvent;

    public readonly record struct GameTiedEvent(
        Guid GameId
    ) : IEvent;

    public readonly record struct GameWonEvent(
        Guid GameId,
        string WinnerEmail,
        string LoserEmail
    ) : IEvent;

    public readonly record struct MoveDecidedEvent(
        Guid GameId,
        string PlayerEmail,
        Move Move
    ) : IEvent;
}
