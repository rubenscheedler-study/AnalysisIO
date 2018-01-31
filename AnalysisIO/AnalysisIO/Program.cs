using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AnalysisIO_Console.Logger;
using Microsoft.Build.Exceptions;
using Newtonsoft.Json;

namespace AnalysisIO_Console
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Out.Write("{\"ERROR\": \"Provide at a repository and a project.\"}");
                return;
                
            }
            string repo = args[0];
            string project = args[1];
            TextWriter consoleOut = Console.Out;
            Console.SetOut(TextWriter.Null); //Symbols that can not be resolved throw an error that ends up in our output. Therefore, disable the output till we need it.

            Log.Write($"repo{repo},project:{project}\n");
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
                Console.SetOut(consoleOut);
                Console.Out.Write(JsonConvert.SerializeObject(versionTrees));

            }
            catch (PathTooLongException)
            {
                Console.SetOut(consoleOut);
                Console.Out.Write("{\"ERROR\": \"This repository is too deeply nested for Windows to handle (MAXPATHLENGTH=260)\"}");
            }
            catch (WebException)
            {
                Console.SetOut(consoleOut);
                Console.Out.Write("{\"ERROR\": \"Something went wrong retrieving the repository from GIT.\"}");
            }
            catch (InvalidProjectFileException ex)
            {
                Console.SetOut(consoleOut);
                Console.Out.Write("{\"ERROR\": \"The solution contains a project that is not supported (C# 6.0 or higher).\"}");
            }
            catch (Exception ex)
            {
                Console.SetOut(consoleOut);
                Console.Out.Write("{\"ERROR\": " + ex.Message);
            }


        }

    }
}
