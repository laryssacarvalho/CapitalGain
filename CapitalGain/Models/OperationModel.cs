using CapitalGain.Enums;
using Newtonsoft.Json;

namespace CapitalGain.Models
{
    public class OperationModel
    {
        public OperationType Operation { get; set; }
        [JsonProperty("unit-cost")]
        public double UnitCost { get; set; }
        public int Quantity { get; set; }
        public OperationModel(OperationType operation, double unitCost, int quantity)
        {
            Operation = operation;
            UnitCost = unitCost;
            Quantity = quantity;
        }
    }
}
