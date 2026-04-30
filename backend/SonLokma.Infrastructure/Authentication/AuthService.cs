using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SonLokma.Application.Auth;
using SonLokma.Domain.Entities;
using SonLokma.Domain.Enums;
using SonLokma.Infrastructure.Persistence;

namespace SonLokma.Infrastructure.Authentication;

public sealed class AuthService(
    SonLokmaDbContext dbContext,
    IPasswordHasher<User> passwordHasher,
    IOptions<JwtOptions> jwtOptions) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = NormalizeEmail(request.Email);
        var emailExists = await dbContext.Users.AnyAsync(user => user.NormalizedEmail == normalizedEmail, cancellationToken);

        if (emailExists)
        {
            throw new InvalidOperationException("Bu e-posta adresi zaten kayıtlı.");
        }

        var role = request.Role == UserRole.Admin ? UserRole.Customer : request.Role;
        var user = new User
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim(),
            NormalizedEmail = normalizedEmail,
            Phone = string.IsNullOrWhiteSpace(request.Phone) ? null : request.Phone.Trim(),
            Role = role,
            CreatedAt = DateTimeOffset.UtcNow
        };

        user.PasswordHash = passwordHasher.HashPassword(user, request.Password);
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return CreateAuthResponse(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = NormalizeEmail(request.Email);
        var user = await dbContext.Users.FirstOrDefaultAsync(
            item => item.NormalizedEmail == normalizedEmail,
            cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedAccessException("E-posta veya şifre hatalı.");
        }

        var verification = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verification == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("E-posta veya şifre hatalı.");
        }

        return CreateAuthResponse(user);
    }

    public async Task<UserProfileResponse?> GetProfileAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Where(user => user.Id == userId)
            .Select(user => new UserProfileResponse(
                user.Id,
                user.FullName,
                user.Email,
                user.Phone,
                user.Role.ToString()))
            .FirstOrDefaultAsync(cancellationToken);
    }

    private AuthResponse CreateAuthResponse(User user)
    {
        var options = jwtOptions.Value;
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(options.AccessTokenMinutes);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);

        var profile = new UserProfileResponse(user.Id, user.FullName, user.Email, user.Phone, user.Role.ToString());
        return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token), expiresAt, profile);
    }

    private static string NormalizeEmail(string email)
    {
        return email.Trim().ToUpperInvariant();
    }

    public static string CreateReservationCode()
    {
        return RandomNumberGenerator.GetString("ABCDEFGHJKLMNPQRSTUVWXYZ23456789", 8);
    }
}
