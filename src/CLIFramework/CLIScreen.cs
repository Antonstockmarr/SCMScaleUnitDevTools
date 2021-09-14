using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CLIFramework
{
    public abstract class CLIScreen
    {
        public CLIScreen previousScreen = null;
        public List<CLIOption> options = null;
        public string selectionHistory = "";
        public string infoBeforeOptions = "";
        public string infoAfterOptions = "";
        public string inputValidationError = "";
        public CLIScreenState state = CLIScreenState.Incomplete;

        public CLIScreen(List<CLIOption> options, string selectionHistory, string infoBeforeOptions = "", string infoAfterOptions = "")
        {
            this.options = options;
            this.selectionHistory = selectionHistory;
            this.infoBeforeOptions = infoBeforeOptions;
            this.infoAfterOptions = infoAfterOptions;
        }

        /// <summary>
        /// This function performs the action indicated by the user's input. 
        /// </summary>
        public abstract Task PerformAction();
    }
}
