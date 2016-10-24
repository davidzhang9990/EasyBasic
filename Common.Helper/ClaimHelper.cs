using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;

namespace Common.Helper
{
    public static class ClaimHelper
    {
        public static string GetClaimValue(string type)
        {
            if (HttpContext.Current == null || HttpContext.Current.User == null) return string.Empty;

            var claimsPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            if (claimsPrincipal == null) return string.Empty;
            var claimIdnIdentity = claimsPrincipal.Identity as ClaimsIdentity;
            if (claimIdnIdentity == null) return string.Empty;
            return GetClaimValue(claimIdnIdentity.Claims, type);
        }

        public static string GetClaimValue(IEnumerable<Claim> claims, string type)
        {
            foreach (var claim in claims)
            {
                if (String.Equals(claim.Type, type, StringComparison.CurrentCultureIgnoreCase))
                    return claim.Value;
            }
            return string.Empty;
        }
    }
}
