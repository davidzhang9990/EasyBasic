using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using XDDEasy.Domain.AccountAggregates;

namespace XDDEasy.Domain.Identity
{
    public class IdentityUserValidation : IIdentityValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(User item)
        {
            if (string.IsNullOrEmpty(item.UserName.Trim()))
                return Task.FromResult(IdentityResult.Failed("UserName can"));

            //if (item.UserName.Length < 5)
            //    return Task.FromResult(IdentityResult.Failed("UserName Minimum is 5"));
            var phoneValidator = new PhoneAttribute();
            var regex = new Regex(@"^[a-zA-Z][a-zA-Z0-9_]*$");
            var emailValidtor = new EmailAddressAttribute();
            var isCorrect = regex.IsMatch(item.UserName) || emailValidtor.IsValid(item.UserName) || phoneValidator.IsValid(item.UserName);
            return Task.FromResult(!isCorrect ? IdentityResult.Failed("User Name can't include special char.") : IdentityResult.Success);
        }
    }
}
