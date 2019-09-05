using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;

namespace Semantic.GlobalPayments.Hpp
{
    public class HPPResponse : HPP
    {
        private static readonly Dictionary<string, PropertyInfo> _jsonEnabledProperties;

        private bool? _authentic;

        static HPPResponse()
        {
            _jsonEnabledProperties = new Dictionary<string, PropertyInfo>();
            foreach (var p in typeof(HPPResponse).GetProperties())
            {
                var ca = p.GetCustomAttribute<JsonPropertyAttribute>();
                if (ca != null)
                {
                    _jsonEnabledProperties.Add(ca.PropertyName, p);
                }
            }
        }

        [JsonProperty(PropertyName = "ACCOUNT")]
        public string Account { get; set; }

        public Dictionary<string, string> AdditionalFormFields { get; set; } = new Dictionary<string, string>();

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalJsonProperties { get; set; }

        [JsonProperty(PropertyName = "HPP_ADDRESS_MATCH_INDICATOR")]
        public Boolean AddressesMatch { get; set; }

        [JsonProperty(PropertyName = "AMOUNT")]
        public string Amount { get; set; }

        [JsonProperty(PropertyName = "AUTHCODE")]
        public string AuthCode { get; set; }

        [JsonProperty(PropertyName = "AVSADDRESSRESULT")]
        public string AvsAddressResult { get; set; }

        [JsonProperty(PropertyName = "AVSPOSTCODERESULT")]
        public string AvsPostcodeResult { get; set; }

        [JsonProperty(PropertyName = "BATCHID")]
        public string BatchId { get; set; }

        [JsonProperty(PropertyName = "HPP_BILLING_CITY")]
        public string BillingCity { get; set; }

        [JsonProperty(PropertyName = "HPP_BILLING_COUNTRY")]
        public string BillingCountry { get; set; }

        [JsonProperty(PropertyName = "HPP_BILLING_POSTALCODE")]
        public string BillingPostalCode { get; set; }

        [JsonProperty(PropertyName = "HPP_BILLING_STATE")]
        public string BillingState { get; set; }

        [JsonProperty(PropertyName = "HPP_BILLING_STREET1")]
        public string BillingStreet1 { get; set; }

        [JsonProperty(PropertyName = "HPP_BILLING_STREET2")]
        public string BillingStreet2 { get; set; }

        [JsonProperty(PropertyName = "HPP_BILLING_STREET3")]
        public string BillingStreet3 { get; set; }

        [JsonProperty(PropertyName = "CARD_PAYMENT_BUTTON")]
        public string CardPaymentButton { get; set; }

        [JsonProperty(PropertyName = "HPP_CHALLENGE_REQUEST_INDICATOR")]
        public ChallengeRequestIndicator ChallengeRequest { get; set; }

        [JsonProperty(PropertyName = "COMMENT1")]
        public string Comment1 { get; set; }

        [JsonProperty(PropertyName = "COMMENT2")]
        public string Comment2 { get; set; }

        [JsonProperty(PropertyName = "HPP_CUSTOMER_EMAIL")]
        public string CustomerEmail { get; set; }

        [JsonProperty(PropertyName = "HPP_CUSTOMER_PHONENUMBER_MOBILE")]
        public string CustomerPhoneNumber { get; set; }

        [JsonProperty(PropertyName = "CVNRESULT")]
        public string CvnResult { get; set; }

        public bool IsAuthentic
        {
            get
            {
                if (!_authentic.HasValue)
                {
                    _authentic = Authenticate();
                }
                return _authentic.Value;
            }
        }

        public bool IsPaypal => this.PaymentMethod == AltPaymentMethods.PayPal;

        [JsonProperty(PropertyName = "MERCHANT_ID")]
        public string MerchantId { get; set; }

        [JsonProperty(PropertyName = "MERCHANT_RESPONSE_URL")]
        public string MerchantResponseUrl { get; set; }

        [JsonProperty(PropertyName = "MESSAGE")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "ORDER_ID")]
        public string OrderId { get; set; }

        [JsonProperty(PropertyName = "PASREF")]
        public string PasRef { get; set; }

        [JsonProperty(PropertyName = "pas_uuid")]
        public string PasUuid { get; set; }

        [JsonProperty(PropertyName = "RESULT")]
        public string Result { get; set; }

        [JsonProperty(PropertyName = "SHA1HASH")]
        public string Sha1Hash { get; set; }

        [JsonProperty(PropertyName = "HPP_SHIPPING_CITY")]
        public string ShippingCity { get; set; }

        [JsonProperty(PropertyName = "HPP_SHIPPING_COUNTRY")]
        public string ShippingCountry { get; set; }

        [JsonProperty(PropertyName = "HPP_SHIPPING_POSTALCODE")]
        public string ShippingPostalCode { get; set; }

        [JsonProperty(PropertyName = "HPP_SHIPPING_STATE")]
        public string ShippingState { get; set; }

        [JsonProperty(PropertyName = "HPP_SHIPPING_STREET1")]
        public string ShippingStreet1 { get; set; }

        [JsonProperty(PropertyName = "HPP_SHIPPING_STREET2")]
        public string ShippingStreet2 { get; set; }

        [JsonProperty(PropertyName = "HPP_SHIPPING_STREET3")]
        public string ShippingStreet3 { get; set; }

        #region Paypal Only

        [JsonProperty(PropertyName = "PAYMENTMETHOD")]
        public string PaymentMethod { get; set; }

        [JsonProperty(PropertyName = "PM_OPTS")]
        public string PaypalMeta { get; set; }

        #endregion Paypal Only

        public string ShortOrderId
        {
            get
            {
                return this.OrderId.Split('-')[0];
            }
        }

        [JsonProperty(PropertyName = "SRD")]
        public string Srd { get; set; }

        #region 3DSecure / SCA

        [JsonProperty(PropertyName = "CAVV")]
        public string Cavv { get; set; }

        [JsonProperty(PropertyName = "ECI")]
        public int? Eci { get; set; }

        [JsonProperty(PropertyName = "XID")]
        public string Xid { get; set; }

        #endregion 3DSecure / SCA

        [JsonProperty(PropertyName = "TIMESTAMP")]
        public string TimeStamp { get; set; }

        public bool WasSuccessful => this.IsAuthentic && this.Result == "00";

        public static HPPResponse FromFormData(NameValueCollection formData)
        {
            var hr = new HPPResponse();

            foreach (var k in formData.AllKeys)
            {
                if (_jsonEnabledProperties.ContainsKey(k))
                {
                    _jsonEnabledProperties[k].SetValue(hr, formData[k]);
                }
                else
                {
                    hr.AdditionalFormFields[k] = formData[k];
                }
            }
            return hr;
        }
        public static HPPResponse FromJsonString(string responseJson)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
               NullValueHandling = NullValueHandling.Ignore
            };
            settings.Converters.Add(new Base64Converter());
            return JsonConvert.DeserializeObject<HPPResponse>(responseJson, settings);
        }

        private bool Authenticate()
        {
            var validationString = GetHash(GetHash(string.Concat(
                    this.TimeStamp, '.',
                    this.MerchantId, '.',
                    this.OrderId, '.',
                    this.Result, '.',
                    this.Message, '.',
                    this.PasRef, '.',
                    this.AuthCode
                )) + '.' + Config.Secret);

            if (validationString == this.Sha1Hash)
            {
                return true;
            }

            return false;
        }
    }
}