using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace AnalysisIO.SourceImporter
{
    class SourceImporter
    {
        public SourceImporter()
        {
            AsyncMethod();
        }

        public async void AsyncMethod()
        {
            var client = new GitHubClient(new ProductHeaderValue("rationally"));

            var releases = client.Repository.Release.GetAll("rationally", "rationally_visio");

            var latest = releases.Result[0];
            Console.WriteLine(
                "The latest release is tagged at {0} and is named {1}",
                latest.TagName,
                latest.Name);

            var archive = await client.Repository.Content.GetArchive("rationally", "rationally_visio",ArchiveFormat.Zipball);

            File.WriteAllBytes("rationally.zip", archive);

            FastZip entry = new FastZip();
            entry.ExtractZip("rationally.zip","repos/rationally/versions",null);
        }
    }
}
