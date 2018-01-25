using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
                //Console.Out.Write("ERROR: Provide at a repository and a project.");TODO enable this
                //return;
                args = new string[] { "rationally", "rationally_visio", "0.1.5" };
            }
            string repo = args[0];
            string project = args[1];

            Dictionary<string, string> versionTrees = null;
            try
            {
                if (args.Length == 2) //fetch all trees of the project
                {
                    versionTrees = Task.Run(() => new SourceImporter.SourceImporter().ImportSource(repo, project)).GetAwaiter().GetResult();
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
            catch (PathTooLongException)
            {
                Console.Out.Write("ERROR: This repository is too deeply nested for Windows to handle (MAXPATHLENGTH=260)");
            }
            catch (WebException)
            {
                Console.Out.Write("ERROR: Something went wrong retrieving the repository from GIT.");
            }
            catch (Exception ex)
            {
                Console.Out.Write("ERROR: "+ex.Message);
            }


        }

    }
}
