using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Semantic.GlobalPayments.Hpp
{
    public abstract class HPP
    {
        public enum ChallengeRequestIndicator
        {
            NO_PREFERENCE,
            NO_CHALLENGE_REQUESTED,
            CHALLENGE_PREFERRED,
            CHALLENGE_MANDATED
        }

        public enum Boolean
        {
            TRUE,
            FALSE
        }

        public static class AltPaymentMethods
        {
            public const string PayPal = "paypal";
        }

        protected static string GetHash(string input)
        {
            var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
        }
    }
}
