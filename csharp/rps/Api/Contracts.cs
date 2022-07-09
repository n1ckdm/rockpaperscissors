namespace RPS.Api;

public static class Rps 
{
    public static class V1
    {
        public readonly record struct Create(
            string Email
        );

        public readonly record struct Move(
            Guid GameId,
            string Email,
            string MoveType
        );
    }
}