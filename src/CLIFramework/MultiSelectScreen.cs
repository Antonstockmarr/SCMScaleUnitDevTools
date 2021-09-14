using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CLIFramework
{
    public class MultiSelectScreen : CLIScreen
    {
        public MultiSelectScreen(List<CLIOption> options, string selectionHistory, string infoBeforeOptions = "", string infoAfterOptions = "")
            : base(options, selectionHistory, infoBeforeOptions, infoAfterOptions) { }

        public override async Task PerformAction()
        {
            int totalOptions = options.Count;

            string input = Console.ReadLine();

            if (int.TryParse(input, out int cancel))
            {
                if (cancel == totalOptions + 1)
                {
                    previousScreen.state = CLIScreenState.Incomplete; // show previous screen again.
                    state = CLIScreenState.Complete;
                    return;
                }
            }

            string[] enteredNumbers = input.Split(',');

            var runSteps = new List<int>();
            var skipSteps = new List<int>();

            foreach (string number in enteredNumbers)
            {
                number.Trim();
                if (int.TryParse(number, out int step))
                {
                    if (step >= 1 && step <= totalOptions)
                    {
                        runSteps.Add(step - 1);
                    }
                    else if (step <= -1 && step >= -totalOptions)
                    {
                        skipSteps.Add(-(step) - 1);
                    }
                    else
                    {
                        inputValidationError = $"Operation {step} not found. Please enter the number for the operation you like to start.";
                        return;
                    }
                }
                else
                {
                    inputValidationError = $"Invalid input. {number} is not a number.";
                    return;
                }
            }

            if (runSteps.Count > 0 && skipSteps.Count > 0)
            {
                inputValidationError = "Either choose a set of steps to skip or a set of steps to run.";
                return;
            }

            state = CLIScreenState.Complete;
            if (skipSteps.Count > 0)
            {
                for (int i = 0; i < totalOptions; i++)
                {
                    if (!skipSteps.Contains(i))
                    {
                        await options[i].Command(i, string.IsNullOrEmpty(selectionHistory) ? options[i].Name : selectionHistory + " -> " + options[i].Name);
                    }
                }
            }
            else
            {
                runSteps.Sort();
                for (int i = 0; i < runSteps.Count; i++)
                {
                    await options[runSteps[i]].Command(runSteps[i], string.IsNullOrEmpty(selectionHistory) ? options[runSteps[i]].Name : selectionHistory + " -> " + options[runSteps[i]].Name);
                }
            }
        }
    }
}
