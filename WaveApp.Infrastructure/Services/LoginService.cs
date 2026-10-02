using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WaveApp.Application.Interfaces;
using WaveApp.Core.DTOs;
using WaveApp.Core.Entities;
using WaveApp.Infrastructure.Data;

namespace WaveApp.Infrastructure.Services;

public class LoginService : ILoginService
{
    private readonly WaveDbContext _db;
    private readonly PasswordHasher<Login> _passwordHasher;

    public LoginService(WaveDbContext db)
    {
        _db = db;
        _passwordHasher = new PasswordHasher<Login>();
    }


    // =========================================================
    // GET ALL
    // =========================================================

    public async Task<IReadOnlyList<LoginReadDto>> GetAllAsync()
    {
        return await _db.Logins
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new LoginReadDto
            {
                Id = x.Id,
                Username = x.Username,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }


    // =========================================================
    // GET BY ID
    // =========================================================

    public async Task<LoginReadDto?> GetByIdAsync(int id)
    {
        return await _db.Logins
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new LoginReadDto
            {
                Id = x.Id,
                Username = x.Username,
                CreatedAt = x.CreatedAt
            })
            .FirstOrDefaultAsync();
    }


    // =========================================================
    // CREATE
    // =========================================================

    public async Task<LoginReadDto> CreateAsync(
        LoginCreateDto dto)
    {
        var username = dto.Username.Trim();

        var usernameExists = await _db.Logins
            .AnyAsync(x => x.Username == username);

        if (usernameExists)
        {
            throw new InvalidOperationException(
                "A login with this username already exists.");
        }


        var login = new Login
        {
            Username = username,
            CreatedAt = DateTime.UtcNow
        };


        // -----------------------------------------------------
        // HASH PASSWORD
        // -----------------------------------------------------

        login.PasswordHash =
            _passwordHasher.HashPassword(
                login,
                dto.Password);


        _db.Logins.Add(login);

        await _db.SaveChangesAsync();


        return new LoginReadDto
        {
            Id = login.Id,
            Username = login.Username,
            CreatedAt = login.CreatedAt
        };
    }


    // =========================================================
    // UPDATE
    // =========================================================

    public async Task<LoginReadDto?> UpdateAsync(
        int id,
        LoginUpdateDto dto)
    {
        var login = await _db.Logins
            .FirstOrDefaultAsync(x => x.Id == id);

        if (login == null)
            return null;


        var username = dto.Username.Trim();


        // -----------------------------------------------------
        // CHECK USERNAME
        // -----------------------------------------------------

        var usernameExists = await _db.Logins
            .AnyAsync(x =>
                x.Id != id &&
                x.Username == username);

        if (usernameExists)
        {
            throw new InvalidOperationException(
                "A login with this username already exists.");
        }


        login.Username = username;


        // -----------------------------------------------------
        // CHANGE PASSWORD ONLY IF ONE WAS PROVIDED
        // -----------------------------------------------------

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            login.PasswordHash =
                _passwordHasher.HashPassword(
                    login,
                    dto.Password);
        }


        await _db.SaveChangesAsync();


        return new LoginReadDto
        {
            Id = login.Id,
            Username = login.Username,
            CreatedAt = login.CreatedAt
        };
    }


    // =========================================================
    // DELETE
    // =========================================================

    public async Task<bool> DeleteAsync(int id)
    {
        var login = await _db.Logins
            .FirstOrDefaultAsync(x => x.Id == id);

        if (login == null)
            return false;


        _db.Logins.Remove(login);

        await _db.SaveChangesAsync();

        return true;
    }
}