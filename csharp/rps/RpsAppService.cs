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

    private async Task<Game> GetCurrentState(Guid gameId) =>
        (await _repository.Load(gameId)).Aggregate(new Game(), (g, e) => Game.Apply(g, e));

    private List<IEvent> SendToAggregate(ICommand command, Game game) =>
        command switch
        {
            CreateGameCommand c => game.Handle(c),
            MakeMoveCommand c => game.Handle(c),
            _ => throw new NotImplementedException()
        };

    public async Task Handle(ICommand cmd)
    {
        Game game = await GetCurrentState(cmd.GameId);
        List<IEvent> events = SendToAggregate(cmd, game);
        await _repository.Save(cmd.GameId, events);
    }

    public Task HandleContract(IContract contract) =>
        contract switch
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