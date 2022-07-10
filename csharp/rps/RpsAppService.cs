using RPS.Domain;
using RPS.Api;
using static RPS.Domain.Commands;
using static RPS.Api.Rps;

namespace RPS;

public interface IAppService
{
    Task HandleContract(IContract command);
    Task Handle(ICommand command);
}

public class RpsAppService : IAppService
{
    private readonly IRpsRepository _repository;
    public RpsAppService(IRpsRepository repository) => _repository = repository;

    private async Task<Game> GetCurrentState(Guid gameId)
    {
        // Get events from the repository
        var events = await _repository.Load(gameId);
        // This is a left fold over the events to return the game state:
        return events.Aggregate(new Game(), (g, e) => Game.Apply(g, e));
    }

    public async Task Handle(ICommand cmd)
    {
        Game game = await GetCurrentState(cmd.GameId);
        List<IEvent> events;
        switch (cmd)
        {
            case CreateGameCommand c:
                events = game.Handle(c);
                break;
            case MakeMoveCommand c:
                events = game.Handle(c);
                break;
            default:
                throw new NotImplementedException();
        }
        await _repository.Save(cmd.GameId, events);
    }

    public Task HandleContract(IContract command) =>
        command switch
        {
            V1.Create cmd => Handle(new CreateGameCommand(Guid.NewGuid(), cmd.Email)),

            V1.Move cmd => Handle(new MakeMoveCommand(
                cmd.GameId,
                cmd.Email,
                Move.FromString(cmd.MoveType)
            )),

            _ => Task.CompletedTask
        };
}