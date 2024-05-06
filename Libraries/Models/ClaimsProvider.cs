using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Facilitate.Libraries.Models
{
    public class ClaimsProvider : IClaimsProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClaimsProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal? GetClaimsPrincipal()
        {
            return _httpContextAccessor.HttpContext?.User;
        }
    }

    public interface IClaimsProvider
    {
        ClaimsPrincipal? GetClaimsPrincipal();
    }
}
