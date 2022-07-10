using System.ComponentModel.DataAnnotations;

namespace RPS.Api;

public interface IContract {}

public static class Rps 
{
    public static class V1
    {
        public record Create(
            [Required]string Email
        ) : IContract;

        public record Move(
            [Required]Guid GameId,
            [Required]string Email,
            [Required]string MoveType
        ) : IContract;
    }
}