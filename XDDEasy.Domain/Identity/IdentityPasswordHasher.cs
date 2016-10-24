using Microsoft.AspNet.Identity;

namespace XDDEasy.Domain.Identity
{
    public class IdentityPasswordHasher: IPasswordHasher
    {
        public string HashPassword(string password)
        {
            //return System.Web.Helpers.Crypto.HashPassword(password);
            
            return password;
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            //return System.Web.Helpers.Crypto.VerifyHashedPassword(hashedPassword, providedPassword) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
            
            return hashedPassword == providedPassword ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}
