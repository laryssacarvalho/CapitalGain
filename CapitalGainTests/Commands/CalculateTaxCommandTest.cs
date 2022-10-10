using CapitalGain.Commands;
using CapitalGain.Enums;
using CapitalGain.Models;
using CapitalGain.Settings;
using Xunit;

namespace CapitalGainTests.Commands
{
    public class CalculateTaxCommandTest
    {
        private CalculateTaxCommand _sut;
        public CalculateTaxCommandTest()
        {
            var settings = new ApplicationSettings
            {
                TaxRate = 0.2
            };
            _sut = new(settings);
        }

        [Fact]
        public void Execute_ShouldReturnNoTax_WhenThereAreJustBuyOperations()
        {
            var operations = new List<OperationModel>
            {
                new OperationModel(OperationType.Buy, 10, 100),
                new OperationModel(OperationType.Buy, 10, 100)
            };
            
            var result = _sut.Execute(operations);
            
            Assert.Equal(operations.Count, result.Count);
            Assert.All(result, t => Assert.Equal(0, t.Tax));
        }

        [Fact]
        public void Execute_ShouldReturnNoTax_WhenSellOperationIsLessThanMinimum()
        {
            var operations = new List<OperationModel>
            {
                new OperationModel(OperationType.Sell, 10, 100)                
            };

            var result = _sut.Execute(operations);

            Assert.Single(result);
            Assert.All(result, t => Assert.Equal(0, t.Tax));
        }

        [Fact]
        public void Execute_ShouldReturnNoTax_WhenThereIsALossInTheOperation()
        {
            var operations = new List<OperationModel>
            {
                new OperationModel(OperationType.Buy, 10, 10000),
                new OperationModel(OperationType.Sell, 5, 5000)
            };

            var result = _sut.Execute(operations);

            Assert.Equal(operations.Count, result.Count);
            Assert.All(result, t => Assert.Equal(0, t.Tax));
        }

        [Fact]
        public void Execute_ShouldReturnNoTax_WhenLossOperationIsTheLastOne()
        {
            var operations = new List<OperationModel>
            {
                new OperationModel(OperationType.Buy, 10, 10000),
                new OperationModel(OperationType.Sell, 20, 5000),
                new OperationModel(OperationType.Sell, 5, 5000)
            };

            var result = _sut.Execute(operations);

            Assert.Equal(operations.Count, result.Count);
            Assert.Equal(0, result.ElementAt(0).Tax);
            Assert.Equal(10000, result.ElementAt(1).Tax);
            Assert.Equal(0, result.ElementAt(2).Tax);
        }

        [Fact]
        public void Execute_ShouldDeductLossFromProfit_WhenThereIsALossFollowedByAProfit()
        {
            var operations = new List<OperationModel>
            {
                new OperationModel(OperationType.Buy, 10, 10000),
                new OperationModel(OperationType.Sell, 5, 5000),
                new OperationModel(OperationType.Sell, 20, 3000)
            };

            var result = _sut.Execute(operations);

            Assert.Equal(operations.Count, result.Count);
            Assert.Equal(0, result.ElementAt(0).Tax);
            Assert.Equal(0, result.ElementAt(1).Tax);
            Assert.Equal(1000, result.ElementAt(2).Tax);
        }

        [Fact]
        public void Execute_ShouldReturnNoTax_WhenThereIsNoLossOrProfit()
        {
            var operations = new List<OperationModel>
            {
                new OperationModel(OperationType.Buy, 10, 10000),
                new OperationModel(OperationType.Buy, 25, 5000),
                new OperationModel(OperationType.Sell, 15, 10000)
            };

            var result = _sut.Execute(operations);

            Assert.Equal(operations.Count, result.Count);
            Assert.All(result, t => Assert.Equal(0, t.Tax));
        }

        [Fact]
        public void Execute_ShouldDeductAllLossFromProfits_WhenLossAmountCanNotBeDeductJustFromOneProfit()
        {
            var operations = new List<OperationModel>
            {
                new OperationModel(OperationType.Buy, 10, 10000),
                new OperationModel(OperationType.Sell, 2, 5000),
                new OperationModel(OperationType.Sell, 20, 2000),
                new OperationModel(OperationType.Sell, 20, 2000),
                new OperationModel(OperationType.Sell, 25, 1000)
            };

            var result = _sut.Execute(operations);

            Assert.Equal(operations.Count, result.Count);
            Assert.Equal(0, result.ElementAt(0).Tax);
            Assert.Equal(0, result.ElementAt(1).Tax);
            Assert.Equal(0, result.ElementAt(2).Tax);
            Assert.Equal(0, result.ElementAt(3).Tax);
            Assert.Equal(3000, result.ElementAt(4).Tax);
        }
    }
}
