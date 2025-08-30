using Microsoft.IdentityModel.Tokens;
using SharedClasses;
using SharedClasses.DTOS.UserRoles;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
public static class Utility
{
    static public string securityKey
    {
        get
        {
            return "THIS_IS_A_DEMO_SECRET_KEY_1234567890";
        }
    }
    static public string ComputeHash(string input)
    {
        
        //SHA is Secutred Hash Algorithm.
        // Create an instance of the SHA-256 algorithm
        using (SHA256 sha256 = SHA256.Create())
        {
            // Compute the hash value from the UTF-8 encoded input string
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Convert the byte array to a lowercase hexadecimal string
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }

    static public string GenerateJwtToken(UserDTO user)
    {
        var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
        new Claim(ClaimTypes.Name, user.userName)
    };

        // Add multiple role claims
        foreach (UserRoleInfoDTO role in user.roles)
        {
            if (role.isActive)
                authClaims.Add(new Claim(ClaimTypes.Role, role.roleName));
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: authClaims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
