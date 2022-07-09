using RPS.Domain;
using static RPS.Domain.Commands;
using static RPS.Api.Rps;

namespace RPS;

public interface IAppService
{
    Task Handle(object command);
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

    // TODO: can we make this more DRY?
    public async Task HandleCreate(CreateGameCommand cmd)
    {
        Game game = await GetCurrentState(cmd.GameId);
        var events = game.handle(cmd);
        await _repository.Save(cmd.GameId, events);
    }

    public async Task HandleMove(MakeMoveCommand cmd)
    {
        Game game = await GetCurrentState(cmd.GameId);
        var events = game.handle(cmd);
        await _repository.Save(cmd.GameId, events);
    }

    public Task Handle(object command) =>
        command switch
        {
            V1.Create cmd => HandleCreate(new CreateGameCommand(new Guid(), cmd.Email)),

            V1.Move cmd => HandleMove(new MakeMoveCommand(
                cmd.GameId,
                cmd.Email,
                Move.FromString(cmd.MoveType)
            )),

            _ => Task.CompletedTask
        };
}