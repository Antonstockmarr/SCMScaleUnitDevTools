using System.Threading.Tasks;
using CLIFramework;
using ScaleUnitManagement.Utilities;
using ScaleUnitManagement.ScaleUnitFeatureManager.Common;
using System;
using System.Collections.Generic;

namespace CLI.SetupToolsOptions
{
    internal class ImportWorkloadBlob : DevToolMenu
    {
        public override async Task Show(int input, string selectionHistory)
        {
            List<CLIOption> options = SelectScaleUnitOptions(ImportWorkloadBlobFromSasToken);
            var screen = new SingleSelectScreen(options, selectionHistory, "Please select the scale unit you would like to import workloads to:\n", "\nScale unit storage to import to: ");
            await CLIController.ShowScreen(screen);
        }

        private async Task ImportWorkloadBlobFromSasToken(int input, string selectionHistory)
        {
            using var context = ScaleUnitContext.CreateContext(GetScaleUnitId(input - 1));
            string sasToken = CLIController.EnterValuePrompt("Please paste in the blob SAS URL for the blob storage that the workloads should be copied from:");
            var storageAccountManager = new StorageAccountManager();
            try
            {
                await storageAccountManager.ImportWorkloadsBlob(sasToken);
                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occured while importing blobs: {ex.Message}");
            }
        }
    }
}
