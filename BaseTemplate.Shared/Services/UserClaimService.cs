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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserClaimService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type.Equals("UserId"));
            if (userId is not null)
                return Guid.Parse(userId.Value);
            else
                return Guid.Empty;
        }

        public string GetCurrentUserIpAddress()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                return "HttpContext mevcut değil";
            }

            var remoteIpAddress = context.Connection?.RemoteIpAddress?.ToString();
            return remoteIpAddress ?? "IP adresi alınamadı";
        }

    }
}
