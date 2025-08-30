using Microsoft.AspNetCore.Identity;
using SharedClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clinic_management_system_Bussiness
{
    public class PasswordSerivce
    {
        private readonly PasswordHasher<object> _hasher = new PasswordHasher<object>();
        
        public string HashPaword(string password)
        {
            return _hasher.HashPassword(null, password);
        }
        public bool VerifyPassword(string hashedPassword, string enterdPassword)
        {
            var result = _hasher.VerifyHashedPassword(null, hashedPassword, enterdPassword);

            return result == PasswordVerificationResult.Success;
        }
    }
}
