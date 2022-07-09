using RPS.Domain;
using static RPS.Domain.Commands;
using static RPS.Api.Rps;

public interface IAppService
{
    Task Handle(object command);
}

public class RpsAppService : IAppService
{
    public Task HandleCreate(CreateGameCommand cmd)
    {
        throw new NotImplementedException();
    }

    public Task HandleMove(MakeMoveCommand cmd)
    {
        throw new NotImplementedException();
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