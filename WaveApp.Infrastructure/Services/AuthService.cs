using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using WaveApp.Core.DTOs;
using WaveApp.Core.Entities;
using WaveApp.Core.Interfaces;
using WaveApp.Infrastructure.Data;

namespace WaveApp.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly WaveDbContext _db;
    private readonly IPasswordHasher<Login> _passwordHasher;
    private readonly IConfiguration _configuration;


    // =========================================================
    // CONSTRUCTOR
    // =========================================================

    public AuthService(
        WaveDbContext db,
        IPasswordHasher<Login> passwordHasher,
        IConfiguration configuration)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }


    // =========================================================
    // LOGIN
    // =========================================================

    public async Task<AuthResponseDto?> LoginAsync(
        string username,
        string password)
    {
        // -----------------------------------------------------
        // VALIDATE INPUT
        // -----------------------------------------------------

        if (string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        username = username.Trim();


        // -----------------------------------------------------
        // FIND LOGIN + PROFILE
        // -----------------------------------------------------

        var account = await _db.Logins
            .Where(login =>
                login.Username == username)
            .Join(
                _db.Profiles,

                login => login.Id,

                profile => profile.LoginId,

                (login, profile) => new
                {
                    Login = login,
                    Profile = profile
                })
            .FirstOrDefaultAsync();


        if (account == null)
        {
            return null;
        }


        // -----------------------------------------------------
        // VERIFY PASSWORD
        // -----------------------------------------------------

        var passwordResult =
            _passwordHasher.VerifyHashedPassword(
                account.Login,
                account.Login.PasswordHash,
                password);


        if (passwordResult ==
            PasswordVerificationResult.Failed)
        {
            return null;
        }


        // -----------------------------------------------------
        // OPTIONAL PASSWORD REHASH
        // -----------------------------------------------------

        if (passwordResult ==
            PasswordVerificationResult.SuccessRehashNeeded)
        {
            account.Login.PasswordHash =
                _passwordHasher.HashPassword(
                    account.Login,
                    password);

            await _db.SaveChangesAsync();
        }


        // -----------------------------------------------------
        // GENERATE ACCESS TOKEN
        // -----------------------------------------------------

        var accessToken =
            GenerateAccessToken(
                account.Login,
                account.Profile);


        // -----------------------------------------------------
        // GENERATE REFRESH TOKEN
        // -----------------------------------------------------

        var rawRefreshToken =
            GenerateRefreshToken();

        var refreshTokenHash =
            HashRefreshToken(
                rawRefreshToken);


        var refreshTokenExpiresAt =
            GetRefreshTokenExpiration();


        // -----------------------------------------------------
        // STORE REFRESH TOKEN HASH
        // -----------------------------------------------------

        var refreshTokenEntity =
            new RefreshToken
            {
                LoginId =
                    account.Login.Id,

                TokenHash =
                    refreshTokenHash,

                CreatedAt =
                    DateTime.UtcNow,

                ExpiresAt =
                    refreshTokenExpiresAt,

                RevokedAt =
                    null,

                ReplacedByTokenHash =
                    null
            };


        _db.RefreshTokens.Add(
            refreshTokenEntity);


        await _db.SaveChangesAsync();


        // -----------------------------------------------------
        // RESPONSE
        // -----------------------------------------------------

        return CreateAuthResponse(
            account.Login,
            account.Profile,
            accessToken.Token,
            accessToken.ExpiresAt,
            rawRefreshToken,
            refreshTokenExpiresAt);
    }


    // =========================================================
    // REFRESH ACCESS TOKEN
    // =========================================================

    public async Task<AuthResponseDto?> RefreshAsync(
        string refreshToken)
    {
        // -----------------------------------------------------
        // VALIDATE INPUT
        // -----------------------------------------------------

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return null;
        }


        // -----------------------------------------------------
        // HASH RECEIVED TOKEN
        // -----------------------------------------------------

        var tokenHash =
            HashRefreshToken(
                refreshToken);


        // -----------------------------------------------------
        // FIND STORED REFRESH TOKEN
        // -----------------------------------------------------

        var storedToken =
            await _db.RefreshTokens
                .Include(x => x.Login)
                .ThenInclude(x => x.Profile)
                .FirstOrDefaultAsync(
                    x => x.TokenHash == tokenHash);


        if (storedToken == null)
        {
            return null;
        }


        // -----------------------------------------------------
        // VALIDATE REFRESH TOKEN
        // -----------------------------------------------------

        if (storedToken.RevokedAt != null)
        {
            return null;
        }


        if (storedToken.ExpiresAt <= DateTime.UtcNow)
        {
            return null;
        }


        var login =
            storedToken.Login;


        if (login == null)
        {
            return null;
        }


        var profile =
            login.Profile;


        if (profile == null)
        {
            return null;
        }


        // -----------------------------------------------------
        // GENERATE NEW REFRESH TOKEN
        //
        // Refresh tokens are rotated. The old token becomes
        // invalid immediately after successful refresh.
        // -----------------------------------------------------

        var newRawRefreshToken =
            GenerateRefreshToken();


        var newRefreshTokenHash =
            HashRefreshToken(
                newRawRefreshToken);


        var newRefreshTokenExpiresAt =
            GetRefreshTokenExpiration();


        // -----------------------------------------------------
        // REVOKE OLD TOKEN
        // -----------------------------------------------------

        storedToken.RevokedAt =
            DateTime.UtcNow;


        storedToken.ReplacedByTokenHash =
            newRefreshTokenHash;


        // -----------------------------------------------------
        // STORE NEW REFRESH TOKEN
        // -----------------------------------------------------

        var newRefreshToken =
            new RefreshToken
            {
                LoginId =
                    login.Id,

                TokenHash =
                    newRefreshTokenHash,

                CreatedAt =
                    DateTime.UtcNow,

                ExpiresAt =
                    newRefreshTokenExpiresAt,

                RevokedAt =
                    null,

                ReplacedByTokenHash =
                    null
            };


        _db.RefreshTokens.Add(
            newRefreshToken);


        // -----------------------------------------------------
        // GENERATE NEW ACCESS TOKEN
        // -----------------------------------------------------

        var newAccessToken =
            GenerateAccessToken(
                login,
                profile);


        // -----------------------------------------------------
        // SAVE TOKEN ROTATION
        // -----------------------------------------------------

        await _db.SaveChangesAsync();


        // -----------------------------------------------------
        // RESPONSE
        // -----------------------------------------------------

        return CreateAuthResponse(
            login,
            profile,
            newAccessToken.Token,
            newAccessToken.ExpiresAt,
            newRawRefreshToken,
            newRefreshTokenExpiresAt);
    }


    // =========================================================
    // LOGOUT
    // =========================================================

    public async Task<bool> LogoutAsync(
        string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return false;
        }


        // -----------------------------------------------------
        // HASH TOKEN
        // -----------------------------------------------------

        var tokenHash =
            HashRefreshToken(
                refreshToken);


        // -----------------------------------------------------
        // FIND TOKEN
        // -----------------------------------------------------

        var storedToken =
            await _db.RefreshTokens
                .FirstOrDefaultAsync(
                    x => x.TokenHash == tokenHash);


        if (storedToken == null)
        {
            return false;
        }


        // -----------------------------------------------------
        // REVOKE
        // -----------------------------------------------------

        if (storedToken.RevokedAt == null)
        {
            storedToken.RevokedAt =
                DateTime.UtcNow;


            await _db.SaveChangesAsync();
        }


        return true;
    }


    // =========================================================
    // GET PROFILE
    // =========================================================

    public async Task<ProfileReadDto?> GetProfileAsync(
        string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return null;
        }


        username =
            username.Trim();


        // -----------------------------------------------------
        // FIND PROFILE
        // -----------------------------------------------------

        var result =
            await _db.Profiles
                .Join(
                    _db.Logins,

                    profile =>
                        profile.LoginId,

                    login =>
                        login.Id,

                    (profile, login) =>
                        new
                        {
                            Profile = profile,
                            Login = login
                        })
                .FirstOrDefaultAsync(
                    x =>
                        x.Login.Username ==
                        username);


        if (result == null)
        {
            return null;
        }


        // -----------------------------------------------------
        // RESPONSE
        // -----------------------------------------------------

        return new ProfileReadDto(
            result.Profile.Id,
            result.Profile.FullName,
            result.Profile.Email,
            result.Profile.Role
        );
    }


    // =========================================================
    // REGISTER
    // =========================================================

    public async Task<RegisterReadDto> RegisterAsync(
        RegisterDto dto)
    {
        // -----------------------------------------------------
        // NORMALIZE INPUT
        // -----------------------------------------------------

        var username =
            dto.Username.Trim();


        var email =
            dto.Email
                .Trim()
                .ToLowerInvariant();


        var fullName =
            dto.FullName.Trim();


        // -----------------------------------------------------
        // CHECK USERNAME
        // -----------------------------------------------------

        var usernameExists =
            await _db.Logins
                .AnyAsync(
                    x =>
                        x.Username ==
                        username);


        if (usernameExists)
        {
            throw new InvalidOperationException(
                "A login with this username already exists.");
        }


        // -----------------------------------------------------
        // CHECK EMAIL
        // -----------------------------------------------------

        var emailExists =
            await _db.Profiles
                .AnyAsync(
                    x =>
                        x.Email ==
                        email);


        if (emailExists)
        {
            throw new InvalidOperationException(
                "A profile with this email address already exists.");
        }


        // -----------------------------------------------------
        // DATABASE TRANSACTION
        // -----------------------------------------------------

        await using var transaction =
            await _db.Database
                .BeginTransactionAsync();


        try
        {
            // =================================================
            // CREATE LOGIN
            // =================================================

            var login =
                new Login
                {
                    Username =
                        username,

                    CreatedAt =
                        DateTime.UtcNow
                };


            // -------------------------------------------------
            // HASH PASSWORD
            // -------------------------------------------------

            login.PasswordHash =
                _passwordHasher
                    .HashPassword(
                        login,
                        dto.Password);


            _db.Logins.Add(
                login);


            // -------------------------------------------------
            // SAVE LOGIN
            //
            // Required so PostgreSQL generates Login.Id.
            // -------------------------------------------------

            await _db.SaveChangesAsync();


            // =================================================
            // CREATE PROFILE
            // =================================================

            var profile =
                new Profile
                {
                    LoginId =
                        login.Id,

                    FullName =
                        fullName,

                    Email =
                        email,

                    // Public registration always creates
                    // a normal User.
                    Role =
                        "User"
                };


            _db.Profiles.Add(
                profile);


            await _db.SaveChangesAsync();


            // -------------------------------------------------
            // COMMIT
            // -------------------------------------------------

            await transaction
                .CommitAsync();


            // -------------------------------------------------
            // RESPONSE
            // -------------------------------------------------

            return new RegisterReadDto
            {
                LoginId =
                    login.Id,

                ProfileId =
                    profile.Id,

                Username =
                    login.Username,

                FullName =
                    profile.FullName,

                Email =
                    profile.Email,

                Role =
                    profile.Role,

                CreatedAt =
                    login.CreatedAt
            };
        }
        catch
        {
            // -------------------------------------------------
            // ROLLBACK
            // -------------------------------------------------

            await transaction
                .RollbackAsync();

            throw;
        }
    }


    // =========================================================
    // GENERATE ACCESS TOKEN
    // =========================================================

    private (
        string Token,
        DateTime ExpiresAt
    ) GenerateAccessToken(
        Login login,
        Profile profile)
    {
        // -----------------------------------------------------
        // JWT KEY
        // -----------------------------------------------------

        var jwtKey =
            _configuration["Jwt:Key"];


        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            throw new InvalidOperationException(
                "JWT signing key is not configured.");
        }


        // -----------------------------------------------------
        // ISSUER
        // -----------------------------------------------------

        var issuer =
            _configuration["Jwt:Issuer"];


        if (string.IsNullOrWhiteSpace(issuer))
        {
            throw new InvalidOperationException(
                "JWT issuer is not configured.");
        }


        // -----------------------------------------------------
        // AUDIENCE
        // -----------------------------------------------------

        var audience =
            _configuration["Jwt:Audience"];


        if (string.IsNullOrWhiteSpace(audience))
        {
            throw new InvalidOperationException(
                "JWT audience is not configured.");
        }


        // -----------------------------------------------------
        // ACCESS TOKEN EXPIRATION
        // -----------------------------------------------------

        var expiryMinutes =
            _configuration.GetValue<int>(
                "Jwt:AccessTokenExpiryMinutes");


        if (expiryMinutes <= 0)
        {
            expiryMinutes = 15;
        }


        var expiresAt =
            DateTime.UtcNow
                .AddMinutes(
                    expiryMinutes);


        // -----------------------------------------------------
        // CLAIMS
        // -----------------------------------------------------

        var claims =
            new List<Claim>
            {
                new(
                    ClaimTypes.NameIdentifier,
                    login.Id.ToString()),

                new(
                    ClaimTypes.Name,
                    login.Username),

                new(
                    ClaimTypes.Email,
                    profile.Email),

                new(
                    ClaimTypes.Role,
                    profile.Role),

                new(
                    "profileId",
                    profile.Id.ToString()),

                new(
                    "jti",
                    Guid.NewGuid().ToString())
            };


        // -----------------------------------------------------
        // SIGNING KEY
        // -----------------------------------------------------

        var securityKey =
            new SymmetricSecurityKey(
                Encoding.UTF8
                    .GetBytes(
                        jwtKey));


        var signingCredentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);


        // -----------------------------------------------------
        // CREATE JWT
        // -----------------------------------------------------

        var jwt =
            new JwtSecurityToken(
                issuer:
                    issuer,

                audience:
                    audience,

                claims:
                    claims,

                notBefore:
                    DateTime.UtcNow,

                expires:
                    expiresAt,

                signingCredentials:
                    signingCredentials);


        // -----------------------------------------------------
        // SERIALIZE JWT
        // -----------------------------------------------------

        var token =
            new JwtSecurityTokenHandler()
                .WriteToken(
                    jwt);


        return (
            token,
            expiresAt
        );
    }


    // =========================================================
    // GENERATE REFRESH TOKEN
    // =========================================================

    private static string GenerateRefreshToken()
    {
        // 64 bytes = 512 bits of cryptographically
        // secure random data.
        var randomBytes =
            RandomNumberGenerator
                .GetBytes(64);


        return Convert
            .ToBase64String(
                randomBytes);
    }


    // =========================================================
    // HASH REFRESH TOKEN
    // =========================================================

    private static string HashRefreshToken(
        string refreshToken)
    {
        var tokenBytes =
            Encoding.UTF8
                .GetBytes(
                    refreshToken);


        var hash =
            SHA256.HashData(
                tokenBytes);


        return Convert
            .ToHexString(
                hash);
    }


    // =========================================================
    // REFRESH TOKEN EXPIRATION
    // =========================================================

    private DateTime GetRefreshTokenExpiration()
    {
        var refreshDays =
            _configuration.GetValue<int>(
                "Jwt:RefreshTokenExpiryDays");


        if (refreshDays <= 0)
        {
            refreshDays = 7;
        }


        return DateTime.UtcNow
            .AddDays(
                refreshDays);
    }


    // =========================================================
    // CREATE AUTH RESPONSE
    // =========================================================

    private static AuthResponseDto CreateAuthResponse(
        Login login,
        Profile profile,
        string accessToken,
        DateTime accessTokenExpiresAt,
        string refreshToken,
        DateTime refreshTokenExpiresAt)
    {
        return new AuthResponseDto
        {
            AccessToken =
                accessToken,

            RefreshToken =
                refreshToken,

            AccessTokenExpiresAt =
                accessTokenExpiresAt,

            RefreshTokenExpiresAt =
                refreshTokenExpiresAt,

            LoginId =
                login.Id,

            Username =
                login.Username,

            FullName =
                profile.FullName,

            Email =
                profile.Email,

            Role =
                profile.Role
        };
    }
}