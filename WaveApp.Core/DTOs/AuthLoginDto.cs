using System.ComponentModel.DataAnnotations;

namespace WaveApp.Core.DTOs;

public class AuthLoginDto
{
    [Required]
    public string Username { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}