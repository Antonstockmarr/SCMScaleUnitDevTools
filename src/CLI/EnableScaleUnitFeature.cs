using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CLIFramework;
using ScaleUnitManagement.ScaleUnitFeatureManager.Common;
using ScaleUnitManagement.ScaleUnitFeatureManager.Utilities;
using ScaleUnitManagement.Utilities;

namespace CLI
{
    internal class EnableScaleUnitFeature : DevToolMenu
    {
        protected List<IStep> availableSteps;

        public override async Task Show(int input, string selectionHistory)
        {
            List<CLIOption> options = SelectScaleUnitOptions(PrintAvailableStepsForScaleUnit);
            var screen = new SingleSelectScreen(options, selectionHistory, "Environments:\n", "\nWhich environment would you like to configure?: ");
            await CLIController.ShowScreen(screen);
        }

        private async Task PrintAvailableStepsForScaleUnit(int input, string selectionHistory)
        {
            using var context = ScaleUnitContext.CreateContext(GetScaleUnitId(input - 1));
            if (ScaleUnitContext.GetScaleUnitId().Equals("@@"))
                await new EnableScaleUnitFeatureOnHub().PrintAvailableSteps(input, selectionHistory);
            else
                await new EnableScaleUnitFeatureOnScaleUnit().PrintAvailableSteps(input, selectionHistory);
        }

        protected virtual List<IStep> GetAvailableSteps()
        {
            var sf = new StepFactory();
            List<IStep> steps = sf.GetStepsOfType<ICommonStep>();
            return steps;
        }

        protected async Task PrintAvailableSteps(int input, string selectionHistory)
        {
            var options = new List<CLIOption>();
            availableSteps = GetAvailableSteps();
            availableSteps.Sort((x, y) => x.Priority().CompareTo(y.Priority()));

            foreach (IStep step in availableSteps)
            {
                options.Add(new CLIOption() { Name = step.Label(), Command = RunStep });
            }

            ScaleUnitInstance scaleUnit = Config.FindScaleUnitWithId(ScaleUnitContext.GetScaleUnitId());
            string helpMessage = $"Please select steps you would only like to run by supplying the step numbers to run\n" +
                $"in a comma separated list, e.g. \"1,2,5,6\", or skip specific steps by supplying the negative step numbers\n" +
                $"to skip in a comma separated list, e.g. \"-3,-4\".\n" +
                $"\nTasks to run for {scaleUnit.PrintableName()}:\n";

            CLIScreen screen = new MultiSelectScreen(options, selectionHistory, helpMessage, "\nSelect tasks to run/skip: ");
            await CLIController.ShowScreen(screen);
        }

        private async Task RunStep(int input, string selectionHistory)
        {
            try
            {
                Console.WriteLine("Executing step: " + availableSteps[input].Label());
                await availableSteps[input].Run();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error occurred while enabling scale unit feature:\n{ex}");
            }
        }
    }
}
