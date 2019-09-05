using System.Web.Configuration;

namespace Semantic.GlobalPayments.Hpp
{
    public static class Config
    {
        public static string MerchantId => WebConfigurationManager.AppSettings["Realex.MerchantId"];
        public static string AccountId => WebConfigurationManager.AppSettings["Realex.Account"];
        public static string Secret => WebConfigurationManager.AppSettings["Realex.Secret"];
        public static string SandboxUrl => "https://pay.sandbox.realexpayments.com/pay";
        public static string LiveUrl => "https://pay.realexpayments.com/pay";
    }
}
