using RPS.Domain;
namespace RPS;

public interface IRpsRepository
{
    Task<List<IEvent>> Load(Guid gameId);
    Task Save(Guid gameId, List<IEvent> events);
}

public class RpsRepository : IRpsRepository
{
    public Task<List<IEvent>> Load(Guid gameId)
    {
        throw new NotImplementedException();
    }

    public Task Save(Guid gameId, List<IEvent> events)
    {
        throw new NotImplementedException();
    }
}