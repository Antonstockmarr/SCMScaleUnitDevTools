using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CLIFramework
{
    public class SingleSelectScreen : CLIScreen
    {
        public SingleSelectScreen(List<CLIOption> options, string selectionHistory, string infoBeforeOptions = "", string infoAfterOptions = "")
            : base(options, selectionHistory, infoBeforeOptions, infoAfterOptions) { }

        public override async Task PerformAction()
        {
            var allOptions = new List<CLIOption>();
            allOptions.AddRange(options);
            allOptions.AddRange(navigationOptions);
            int totalOptions = allOptions.Count;

            string input = Console.ReadLine();

            if (int.TryParse(input, out int enteredNumber))
            {
                if (enteredNumber >= 1 && enteredNumber <= totalOptions)
                {
                    state = CLIScreenState.Complete;
                    await RunCommand(allOptions, enteredNumber);
                }
                else
                {
                    inputValidationError = "Operation " + enteredNumber + " not found. Please enter the number for the operation you like to start.";
                    return;
                }
            }
            else
            {
                inputValidationError = "Invalid input. Please enter a number.";
                return;
            }
        }
    }
}
