using Newtonsoft.Json;

namespace CapitalGain.Models
{
    public class TaxModel
    {
        [JsonProperty("tax")]
        public double Tax { get; private set; }
        public TaxModel(double tax)
        {
            Tax = tax;
        }
    }
}
