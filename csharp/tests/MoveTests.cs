using RPS.Domain;
namespace cs_tests;

public class TestMove
{
    [Fact]
    public void TestDefeatsLogic()
    {
        Assert.True(Move.Rock.Defeats(Move.Scissors));
        Assert.True(Move.Paper.Defeats(Move.Rock));
        Assert.True(Move.Scissors.Defeats(Move.Paper));

        Assert.False(Move.Rock.Defeats(Move.Paper));
        Assert.False(Move.Paper.Defeats(Move.Scissors));
        Assert.False(Move.Scissors.Defeats(Move.Rock));

        Assert.False(Move.Rock.Defeats(Move.Rock));
        Assert.False(Move.Paper.Defeats(Move.Paper));
        Assert.False(Move.Scissors.Defeats(Move.Scissors));
    }
}