using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Octokit;

namespace AnalysisIO.NET.Git
{
    public class DownloadedRelease
    {
        private static readonly string DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/analysisIO/";
        public bool IsDownloaded { get; set; }
        public Release Release { get; set; }

        public static DownloadedRelease MarkAsDownloadedIfDownloaded(string repo, string project, Release release)
        {
            DownloadedRelease d = new DownloadedRelease();
            d.Release = release;
            d.IsDownloaded = Directory.Exists($"{DefaultPath}{repo}/{project}/{release.TagName}");
            return d;
        }

    }
}