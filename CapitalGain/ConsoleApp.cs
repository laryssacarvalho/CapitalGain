using CapitalGain.Commands;
using CapitalGain.Models;
using CapitalGain.Settings;
using Newtonsoft.Json;

namespace CapitalGain
{
    public class ConsoleApp
    {
        private readonly ApplicationSettings _settings;
        public ConsoleApp(ApplicationSettings settings)
        {
            _settings = settings;
        }
        
        public void Run()
        {
            Console.WriteLine("----------------- GANHO DE CAPITAL -----------------\n");

            while (true)
            {
                try
                {
                    ProcessInput(ReadInput());
                }
                catch
                {
                    Console.WriteLine("Ocorreu um erro!\n");
                }
            }
        }
        private List<OperationsHistoryModel> ReadInput()
        {
            var operations = new List<OperationsHistoryModel>();

            while (true)
            {
                string input = Console.ReadLine();
                
                if (string.IsNullOrEmpty(input))
                    break;
                
                var operationsList = JsonConvert.DeserializeObject<List<OperationModel>>(input);
                operations.Add(new OperationsHistoryModel(operationsList));
            }
            return operations;
        }
        private void ProcessInput(List<OperationsHistoryModel> operations)
        {
            foreach (var operation in operations)
            {
                var result = new CalculateTaxCommand(_settings)
                    .Execute(operation.Operations);

                Console.WriteLine($"{JsonConvert.SerializeObject(result)}\n");
            }
        }
    }
}
