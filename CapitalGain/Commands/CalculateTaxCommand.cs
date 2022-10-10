using CapitalGain.Enums;
using CapitalGain.Models;
using CapitalGain.Settings;

namespace CapitalGain.Commands
{
    public class CalculateTaxCommand : ICommand<List<OperationModel>, List<TaxModel>>
    {
        private readonly double _taxRate = 0.2;
        private double _weightedMean = -1;
        private int _quantity = 0;
        private double _profits = 0;
        private double _losses = 0;
        public CalculateTaxCommand(ApplicationSettings settings)
        {
            _taxRate = settings.TaxRate;
        }
        public List<TaxModel> Execute(List<OperationModel> operations)
        {
            var taxes = new List<TaxModel>();

            foreach(var operation in operations)
            {             
                RegisterOperation(operation);

                taxes.Add(CalculateTax(operation));                
            }

            return taxes;
        }
        private void RegisterOperation(OperationModel operation)
        {
            if (IsBuyOperation(operation))
                RegisterBuyOperation(operation);
            else
                RegisterSellOperation(operation);
        }
        private bool IsBuyOperation(OperationModel operation) => operation.Operation == OperationType.Buy;
        private void RegisterBuyOperation(OperationModel operation)
        {
            _weightedMean = CalculateWeightedMean(operation);
            _quantity += operation.Quantity;
        }
        private double CalculateWeightedMean(OperationModel operation)
        {
            if (_weightedMean == -1)
                return operation.UnitCost;

            return Math.Round(((_quantity * _weightedMean) + (operation.Quantity * operation.UnitCost)) / (_quantity + operation.Quantity), 2);
        }
        private void RegisterSellOperation(OperationModel operation)
        {
            var salesAmount = CalculateSalesAmount(operation);

            if (IsAProfit(salesAmount))
            {
                _profits += (salesAmount - _losses);
                _losses -= _losses;
            }
            else
                _losses += salesAmount * (-1);

            _quantity -= operation.Quantity;
        }

        private double CalculateSalesAmount(OperationModel operation) => (operation.UnitCost - _weightedMean) * operation.Quantity;

        private bool IsAProfit(double total) => total > 0;
        private TaxModel CalculateTax(OperationModel operation)
        {
            if (!ShouldCalculateTax(operation))            
                return new TaxModel(0);

            var tax = new TaxModel(Math.Round(_profits * _taxRate, 2));
            
            _profits = 0;

            return tax;
        }

        private bool ShouldCalculateTax(OperationModel operation)
        {
            if (operation.Operation == OperationType.Buy)
                return false;

            if ((operation.UnitCost * operation.Quantity) < 20000)
                return false;

            if (_profits <= 0 || _losses > 0)
                return false;

            return true;
        }
    }
}
