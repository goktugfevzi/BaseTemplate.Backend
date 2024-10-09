using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Shared.Abstractions
{
    public interface IUserClaimService
    {
        Guid GetCurrentUserId();
        string GetCurrentUserIpAddress();
    }
}
