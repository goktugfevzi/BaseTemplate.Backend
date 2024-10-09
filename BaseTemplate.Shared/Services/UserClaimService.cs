using BaseTemplate.Shared.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Shared.Services
{
    public class UserClaimService : IUserClaimService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserClaimService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Guid GetCurrentUserId()
        {
            var userId = httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type.Equals("UserId"));
            if (userId is not null)
                return Guid.Parse(userId.Value);
            else
                return Guid.Empty;
        }

        public string GetCurrentUserIpAddress()
        {

            return httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        }
    }
}
