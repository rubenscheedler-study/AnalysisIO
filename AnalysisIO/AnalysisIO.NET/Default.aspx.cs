using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using Octokit;
using AnalysisIO.NET.Git;
namespace AnalysisIO.NET
{
    [WebService]
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Start the child process.
            Process p = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = "C:/Users/Ruben/Source/Repos/AnalysisIO/AnalysisIO/AnalysisIO/bin/Debug/AnalysisIO_Console.exe",
                    Arguments = "PowerShell PowerShell 6.0.0"
                }
            };
            // Redirect the output stream of the child process.


        }

        [WebMethod]
        [ScriptMethod]
        public static List<Release> Releases(string repo, string projectName)
        {
            GitWrapper project = GitWrapper.For(repo, projectName);
            return project.Releases.ToList();
        }

        [WebMethod]
        [ScriptMethod]
        public static string Dependencies(string repo, string projectName, string tagName)
        {// Redirect the output stream of the child process.
            using (Process p = new Process{StartInfo = {UseShellExecute = false, RedirectStandardOutput = true,
                                            FileName = "C:/Users/R. Kruizinga/Source/Repos/AnalysisIO/AnalysisIO/AnalysisIO/bin/Debug/AnalysisIO_Console.exe", Arguments = $"{repo} {projectName} {tagName}"}})
            {
                p.Start();
                string s = p.StandardOutput.ReadToEnd();
                return s;
            }

        }

    }
}