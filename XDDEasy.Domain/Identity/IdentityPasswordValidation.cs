using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace XDDEasy.Domain.Identity
{
    public class IdentityPasswordValidation: IIdentityValidator<string>
    {
        public Task<IdentityResult> ValidateAsync(string item)
        {
            if (item.ToLower().Contains("111111"))
                return Task.FromResult(IdentityResult.Failed("Password Cannot contain 6 consecutive digits"));
            else
                return Task.FromResult(IdentityResult.Success);
        }
    }
}
