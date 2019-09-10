using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Semantic.GlobalPayments.Hpp
{
    public class HPPRequest : HPP
    {
        public HPPRequest()
        {

        }

        /// <summary>
        /// Create a new HPP Request
        /// </summary>
        /// <param name="amount">Amount to charge</param>
        /// <param name="orderId">The order ID to use, will be appended with a timestamp</param>
        public static HPPRequest Create(string orderId)
        {
            var timeStamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var request = new HPPRequest
            {
                TimeStamp = timeStamp,
                OrderId = orderId + '-' + timeStamp.Substring(8),
            };

            return request;
        }

        [JsonProperty(PropertyName = "TIMESTAMP")]
        public string TimeStamp { get; set; }

        [JsonProperty(PropertyName = "MERCHANT_ID")]
        public string MerchantId => Config.MerchantId;

        [JsonProperty(PropertyName = "ACCOUNT")]
        public string Account => Config.AccountId;

        [JsonProperty(PropertyName = "ORDER_ID")]
        public string OrderId { get; set; }

        [JsonIgnore]
        public decimal Amount { get; set; }

        [JsonProperty(PropertyName = "AMOUNT")]
        public string AmountString => ((long)(Amount * 100)).ToString();

        [JsonProperty(PropertyName = "CURRENCY")]
        public string Currency { get; set; } = "GBP";

        [JsonProperty(PropertyName = "AUTO_SETTLE_FLAG")]
        public string AutoSettle { get; } = "1"; // 0: No, 1: Yes, MULTI: Who knows? 

        [JsonProperty(PropertyName = "COMMENT1")]
        public string CommentOne { get; set; }

        [JsonProperty(PropertyName = "COMMENT2")]
        public string CommentTwo { get; set; }

        [JsonProperty(PropertyName = "HPP_VERSION")]
        public string HppVersion => "2";

        [JsonProperty(PropertyName = "HPP_CHANNEL")]
        public string HppChannel => "ECOM";

        [JsonProperty(PropertyName = "HPP_CUSTOMER_EMAIL")]
        public string CustomerEmail { get; set; }

        [JsonProperty(PropertyName = "HPP_CUSTOMER_PHONENUMBER_MOBILE")]
        public string CustomerMobileNumber { get; set; }

        [JsonProperty(PropertyName = "HPP_CUSTOMER_PHONENUMBER_HOME")]
        public string CustomerHomeNumber { get; set; }

        [JsonProperty(PropertyName = "HPP_BILLING_STREET1")]
        public string BillingStreet1 { get; set; }
        [JsonProperty(PropertyName = "HPP_BILLING_STREET2")]
        public string BillingStreet2 { get; set; } = "";
        [JsonProperty(PropertyName = "HPP_BILLING_STREET3")]
        public string BillingStreet3 { get; set; } = "";
        [JsonProperty(PropertyName = "HPP_BILLING_CITY")]
        public string BillingCity { get; set; }
        [JsonProperty(PropertyName = "HPP_BILLING_STATE")]
        public string BillingState { get; set; }
        [JsonProperty(PropertyName = "HPP_BILLING_POSTALCODE")]
        public string BillingPostalCode { get; set; } 
        [JsonProperty(PropertyName = "HPP_BILLING_COUNTRY")]
        public string BillingCountry { get; set; }

        [JsonProperty(PropertyName = "HPP_SHIPPING_STREET1")]
        public string ShippingStreet1 { get; set; }
        [JsonProperty(PropertyName = "HPP_SHIPPING_STREET2")]
        public string ShippingStreet2 { get; set; }
        [JsonProperty(PropertyName = "HPP_SHIPPING_STREET3")]
        public string ShippingStreet3 { get; set; }
        [JsonProperty(PropertyName = "HPP_SHIPPING_CITY")]
        public string ShippingCity { get; set; }
        [JsonProperty(PropertyName = "HPP_SHIPPING_STATE")]
        public string ShippingState { get; set; }
        [JsonProperty(PropertyName = "HPP_SHIPPING_POSTALCODE")]
        public string ShippingPostalCode { get; set; }
        [JsonProperty(PropertyName = "HPP_SHIPPING_COUNTRY")]
        public string ShippingCountry { get; set; }

        [JsonProperty(PropertyName = "HPP_ADDRESS_MATCH_INDICATOR")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Boolean AddressesMatch { get; set; }

        [JsonProperty(PropertyName = "HPP_CHALLENGE_REQUEST_INDICATOR")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ChallengeRequestIndicator ChallengeRequest { get; set; } = ChallengeRequestIndicator.NO_PREFERENCE;

        [JsonProperty(PropertyName = "MERCHANT_RESPONSE_URL")]
        public string MerchantResponseUrl { get; set; }

        [JsonProperty(PropertyName = "SHA1HASH")]
        public string Sha1Hash => this.GetHash();

        // Removed in API docs as of August 2019 but retained in example code: https://developer.globalpay.com/#!/hpp/3d-secure-two
        [JsonProperty(PropertyName = "CARD_PAYMENT_BUTTON")]
        public string CardPaymentButtonText { get; set; }

        private string GetHash()
        {
            // Double hashing with secret as the salt 
            return GetHash(GetHash($"{TimeStamp}.{MerchantId}.{OrderId}.{AmountString}.{Currency}") + '.' + Config.Secret);
        }

        public string GetJson()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(this, settings);
        }
    }
}
