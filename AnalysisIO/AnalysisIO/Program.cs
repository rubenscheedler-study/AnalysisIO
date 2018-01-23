using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace AnalysisIO
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                //Console.Out.Write("ERROR: provide at least a repo and a project.");//TODO aanzetton
                args = new string[] { "OpenRA", "OpenRA", "playtest-20180102" };
            }
            string repo = args[0];
            string project = args[1];

            Dictionary<string, string> versionTrees = null;
            if (args.Length == 2) //fetch all trees of the project
            {
                versionTrees = Task.Run(() => new SourceImporter.SourceImporter().ImportSource(repo,project)).GetAwaiter().GetResult();
            }
            if (args.Length == 3) //fetch only the tree of the release specified in the third argument
            {
                versionTrees = Task.Run(() => new SourceImporter.SourceImporter().ImportSource(repo, project, args[2])).GetAwaiter().GetResult();
            }
            if (args.Length == 4) //fetch only the tree of the releases specified in the third and fourth argument
            {
                versionTrees = Task.Run(() => new SourceImporter.SourceImporter().ImportSource(repo, project, args[2], args[3])).GetAwaiter().GetResult();
            }


            Console.Out.Write(JsonConvert.SerializeObject(versionTrees));
        }

    }
}
