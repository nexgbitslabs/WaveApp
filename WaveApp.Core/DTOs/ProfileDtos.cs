namespace WaveApp.Core.DTOs;

public record ProfileReadDto(
    int Id, 
    string FullName, 
    string Email, 
    string Role
    );
