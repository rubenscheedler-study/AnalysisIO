using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using Octokit;

namespace AnalysisIO.NET
{
    public partial class Default : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            //SourceImporter.SourceImporter importer = new SourceImporter.SourceImporter();
            //Tree.Tree t = await importer.BuildTree();
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
        }
    }
}