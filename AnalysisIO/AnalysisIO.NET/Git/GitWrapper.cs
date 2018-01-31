using System;
using System.Collections.Generic;
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
            GitHubClient client = new GitHubClient(new ProductHeaderValue(repo));
            gw.Releases = client.Repository.Release.GetAll(repo, project).Result;
            return gw;
        }
    }
}