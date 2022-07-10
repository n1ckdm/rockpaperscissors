using RPS;
using RPS.Domain;
using static RPS.Domain.Commands;

namespace cs_tests;

public class GameIntegrationTest
{

	private IRpsRepository _repository;
	private IAppService _appService;
    private Guid _gameId;
    private string _player1;
    private string _player2;

    public GameIntegrationTest()
    {
        _repository = new RpsRepository();
        _appService = new RpsAppService(_repository);
        _player1 = "player1@test.com";
        _player2 = "player2@test.com";
        _gameId = Guid.NewGuid();
    }

    [Fact]
    public async Task GameIsTied()
    {
        await _appService.Handle(new CreateGameCommand(_gameId, _player1));
        await _appService.Handle(new MakeMoveCommand(_gameId, _player1, Move.Rock));
        await _appService.Handle(new MakeMoveCommand(_gameId, _player2, Move.Rock));

        var events = await _repository.Load(_gameId);
        Assert.True(events.Contains(new Events.GameTiedEvent(_gameId)));

        var game = events.Aggregate(new Game(), (g, e) => Game.Apply(g, e));
        Assert.True(game.GameTied);
    }

    [Fact]
    public async Task GameVictoryPlayer1()
    {
        await _appService.Handle(new CreateGameCommand(_gameId, _player1));
        await _appService.Handle(new MakeMoveCommand(_gameId, _player1, Move.Paper));
        await _appService.Handle(new MakeMoveCommand(_gameId, _player2, Move.Rock));

        var events = await _repository.Load(_gameId);
        Assert.True(events.Contains(new Events.GameWonEvent(_gameId, _player1, _player2)));

        var game = events.Aggregate(new Game(), (g, e) => Game.Apply(g, e));
        Assert.True(game.GameWon);
    }

    [Fact]
    public async Task GameVictoryPlayer2()
    {
        await _appService.Handle(new CreateGameCommand(_gameId, _player1));
        await _appService.Handle(new MakeMoveCommand(_gameId, _player1, Move.Scissors));
        await _appService.Handle(new MakeMoveCommand(_gameId, _player2, Move.Rock));

        var events = await _repository.Load(_gameId);
        Assert.True(events.Contains(new Events.GameWonEvent(_gameId, _player2, _player1)));

        var game = events.Aggregate(new Game(), (g, e) => Game.Apply(g, e));
        Assert.True(game.GameWon);
    }

    [Fact]
    public async Task SamePlayerShouldFail()
    {
        await _appService.Handle(new CreateGameCommand(_gameId, _player1));
        await _appService.Handle(new MakeMoveCommand(_gameId, _player1, Move.Scissors));
        await Assert.ThrowsAsync<ArgumentException>(() => _appService.Handle(new MakeMoveCommand(_gameId, _player1, Move.Rock)));
    }

    [Fact]
    public async Task InvalidStateShouldFail()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() => _appService.Handle(new MakeMoveCommand(_gameId, _player1, Move.Rock)));
    }

    [Fact]
    public async Task MovePlayedAfterEndShouldFail()
    {
        await _appService.Handle(new CreateGameCommand(_gameId, _player1));
        await _appService.Handle(new MakeMoveCommand(_gameId, _player1, Move.Scissors));
        await _appService.Handle(new MakeMoveCommand(_gameId, _player2, Move.Rock));
        await Assert.ThrowsAsync<InvalidOperationException>(() => _appService.Handle(new MakeMoveCommand(_gameId, "someoneelse", Move.Rock)));
    }

}