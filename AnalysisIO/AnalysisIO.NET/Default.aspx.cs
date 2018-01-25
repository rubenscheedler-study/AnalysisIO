using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using Octokit;
using AnalysisIO.NET.Git;
namespace AnalysisIO.NET
{
    [WebService]
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SourceImporter.SourceImporter importer = new SourceImporter.SourceImporter();
            //uxJson.InnerHtml = JsonConvert.SerializeObject(t);
            /*var client = new GitHubClient(new ProductHeaderValue("rationally"));

            var releases = client.Repository.Release.GetAll("rationally", "rationally_visio");

            var latest = releases.Result[0];
            Console.WriteLine(
                "The latest release is tagged at {0} and is named {1}",
                latest.TagName,
                latest.Name);

            //var a = await client.Repository.Content.GetAllContents("rationally", "rationally_visio");
            var task = client.Repository.Content.GetArchive("rationally", "rationally_visio", ArchiveFormat.Zipball);
            var archive = await task;
            //File.WriteAllBytes("rationally.zip", archive);

            FastZip entry = new FastZip();
            entry.ExtractZip("rationally.zip", "repos/rationally/versions", null);*/

            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "C:/Users/Ruben/Source/Repos/AnalysisIO/AnalysisIO/AnalysisIO/bin/Debug/AnalysisIO_Console.exe";
            p.StartInfo.Arguments = "PowerShell PowerShell 6.0.0";
            //p.Start();

            //uxJson.InnerHtml = p.StandardOutput.ReadToEnd();

            //p.WaitForExit();

        }

        [WebMethod]
        [ScriptMethod]
        public static List<Release> Releases(string repo, string projectName)
        {
            GitWrapper project =  GitWrapper.For(repo, projectName);
            return project.Releases.ToList();
        }

        [WebMethod]
        [ScriptMethod]
        public static string Dependencies(string repo, string projectName, string tagName)
        {
            using (Process p = new Process())
            {
                // Redirect the output stream of the child process.
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "C:/Users/Ruben/Source/Repos/AnalysisIO/AnalysisIO/AnalysisIO/bin/Debug/AnalysisIO_Console.exe";
                p.StartInfo.Arguments = $"{repo} {projectName} {tagName}";
                p.Start();

                string s =  p.StandardOutput.ReadToEnd();
                return s;
            }
                
        }

    }
}