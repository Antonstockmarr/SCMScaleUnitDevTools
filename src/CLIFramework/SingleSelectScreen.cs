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
            int totalOptions = options.Count;
            string input = Console.ReadLine();
            if (int.TryParse(input, out int enteredNumber))
            {
                if (enteredNumber >= 1 && enteredNumber <= totalOptions)
                {
                    state = CLIScreenState.Complete;
                    await options[enteredNumber - 1].Command(enteredNumber, string.IsNullOrEmpty(selectionHistory) ? options[enteredNumber - 1].Name : selectionHistory + " -> " + options[enteredNumber - 1].Name);
                }

                else if (previousScreen != null && enteredNumber == totalOptions + 1)
                {
                    previousScreen.state = CLIScreenState.Incomplete; // show previous screen again.
                    state = CLIScreenState.Complete;
                    return;
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
