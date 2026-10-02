namespace WaveApp.Core.Entities;

public class Profile
{
    public int Id { get; set; }

    public int LoginId { get; set; }

    public string FullName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Role { get; set; } = "User";

    public Login Login { get; set; } = default!;
}