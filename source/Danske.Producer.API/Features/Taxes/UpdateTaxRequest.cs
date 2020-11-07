using Danske.Producer.Enums;
using Newtonsoft.Json;

namespace Danske.Producer.API.Features.Taxes
{
    public class UpdateTaxRequest
    {
        [JsonProperty(Required = Required.Always)]
        public string Municipality { get; set; }

        [JsonProperty(Required = Required.Always)]
        public PeriodType PeriodType { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string PeriodStart { get; set; }

        public string PeriodEnd { get; set; }

        [JsonProperty(Required = Required.Always)]
        public decimal Tax { get; set; }
    }
}