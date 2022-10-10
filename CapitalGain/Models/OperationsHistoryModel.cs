namespace CapitalGain.Models
{
    public class OperationsHistoryModel
    {
        public List<OperationModel> Operations { get; private set; }
        public OperationsHistoryModel(List<OperationModel> operations)
        {
            Operations = operations;
        }
    }
}
