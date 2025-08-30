using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace clinic_management_system_Bussiness
{
    public class CurrentUserSevice
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserSevice (IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? userId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User?.
                    FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (int.TryParse(userId , out int id))
                    return id;
                return null;
            }
        }
    }
}
