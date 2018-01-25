using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Octokit;

namespace AnalysisIO.NET.Git
{
    public class GitWrapper
    {
        public IReadOnlyList<Release> Releases { get; set; }
        public string Repository { get; set; }
        public static GitWrapper For(string repo, string project)
        {
            GitWrapper gw = new GitWrapper();
            var client = new GitHubClient(new ProductHeaderValue(repo));
            gw.Releases = client.Repository.Release.GetAll(repo, project).Result;
            return gw;
        }
    }
}