using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScaleUnitManagement.Utilities;

namespace CLI.Utilities
{
    internal class ScaleUnitDeployer : Deployer
    {
        private readonly string scaleUnitId;

        public ScaleUnitDeployer(string scaleUnitId) : base()
        {
            this.scaleUnitId = scaleUnitId;
        }

        public override async Task Deploy()
        {
            ScaleUnitInstance scaleUnit = Config.FindScaleUnitWithId(scaleUnitId);

            Console.WriteLine($"Deploying scale unit {scaleUnit.PrintableName()}");

            await InitializeEnvironments(scaleUnit);
            await ConfigureEnvironments(scaleUnit);
            await InstallWorkloads(scaleUnit);

            Console.WriteLine($"\nScale unit {scaleUnit.PrintableName()} has been deployed successfully!\n");
        }
    }
}
