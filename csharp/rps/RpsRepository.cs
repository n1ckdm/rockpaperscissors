using RPS.Domain;
namespace RPS;

public interface IRpsRepository
{
    Task<List<IEvent>> Load(Guid gameId);
    Task Save(Guid gameId, List<IEvent> events);
}

public class RpsRepository : IRpsRepository
{
    private Dictionary<Guid, List<IEvent>> _store = new Dictionary<Guid, List<IEvent>>();
    public Task<List<IEvent>> Load(Guid gameId)
    {
        if (_store.TryGetValue(gameId, out var events))
        {
            return Task.FromResult(events);
        }
        return Task.FromResult(new List<IEvent>());
    }

    public Task Save(Guid gameId, List<IEvent> events)
    {
        if (_store.ContainsKey(gameId))
        {
            _store[gameId].AddRange(events);
        }
        else
        {
            _store.Add(gameId, events);
        }
        return Task.CompletedTask;
    }
}