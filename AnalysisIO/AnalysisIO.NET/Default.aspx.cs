using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
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
            
        }

        [WebMethod]
        [ScriptMethod]
        public static List<DownloadedRelease> Releases(string repo, string projectName)
        {
            GitWrapper project = GitWrapper.For(repo, projectName);
            return project.Releases.ToList().Select(d => DownloadedRelease.MarkAsDownloadedIfDownloaded(repo,projectName,d)).ToList();
        }

        [WebMethod]
        [ScriptMethod]
        public static string Dependencies(string repo, string projectName, string tagName)
        {// Redirect the output stream of the child process.
#if DEBUG
            string EXE_PATH_DIR = AppDomain.CurrentDomain.BaseDirectory + "..\\AnalysisIO\\bin\\Debug\\";
#else
            string EXE_PATH_DIR = AppDomain.CurrentDomain.BaseDirectory + "bin\\";
#endif
            //string EXE_PATH_DIR2 = AppDomain.CurrentDomain.BaseDirectory + "bin\\";
            //string EXE_PATH_DIR3 = HttpContext.Current.Server.MapPath("/") + "bin\\";
            using (Process p = new Process{StartInfo = {UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true,
                                            FileName = EXE_PATH_DIR + "AnalysisIO_Console.exe", Arguments = $"{repo} {projectName} {tagName}"}})
            {
                p.Start();
                string s = p.StandardOutput.ReadToEnd();
                return s;
            }

        }

    }
}