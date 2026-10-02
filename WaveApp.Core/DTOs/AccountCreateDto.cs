using System.ComponentModel.DataAnnotations;

namespace WaveApp.Core.DTOs
{
    
    public class LoginReadDto
    {
        public int Id { get; set; }

        public string Username { get; set; } = default!;

        public DateTime CreatedAt { get; set; }
    }

    public class LoginCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = default!;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = default!;
    }

    public class LoginUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = default!;

        [MinLength(8)]
        public string? Password { get; set; }
    }

    public class RegisterDto
{
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = default!;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = default!;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = default!;
}

public class RegisterReadDto
{
    public int LoginId { get; set; }

    public int ProfileId { get; set; }

    public string Username { get; set; } = default!;

    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Role { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
}
}