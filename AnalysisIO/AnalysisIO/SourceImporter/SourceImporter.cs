using Octokit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AnalysisIO.Tree;
using AnalysisIO.Visitor;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using Resolver;

namespace AnalysisIO.SourceImporter
{
    public class SourceImporter
    {
        private static string PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/analysisIO/";
        private static string TREE_FILE_NAME = "tree.json";
        public static Tree.Tree Tree;


        /// <summary>
        /// Returns a dictionary of all releases of the given project.
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> ImportSource(string repo, string project)
        {
            var client = new GitHubClient(new ProductHeaderValue(repo));
            var releases = await client.Repository.Release.GetAll(repo, project);
            return ImportReleases(repo,project,releases.ToList());
        }

        /// <summary>
        /// Used to retrieve one release.
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="project"></param>
        /// <param name="release1"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> ImportSource(string repo, string project, string release1)
        {
            var client = new GitHubClient(new ProductHeaderValue(repo));
            var releases = await client.Repository.Release.GetAll(repo, project);
            return ImportReleases(repo,project,releases.Where(r => r.TagName == release1).ToList());
        }

        /// <summary>
        /// Used to retrieve and later compare two releases.
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="project"></param>
        /// <param name="release1"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> ImportSource(string repo, string project, string release1, string release2)
        {
            var client = new GitHubClient(new ProductHeaderValue(repo));
            var releases = await client.Repository.Release.GetAll(repo, project);
            return ImportReleases(repo,project,releases.Where(r => r.TagName == release1 || r.TagName == release2).ToList());
        }

        public Dictionary<string, string> ImportReleases(string repo, string project, List<Release> releases)
        {
            //dict of version and its serialized tree
            Dictionary<string, string> versionTrees = new Dictionary<string, string>();

            FastZip entry = new FastZip();

            foreach (var r in releases)
            {
                string releaseDir = $"{PATH}/{repo}/{project}/{r.TagName}";

                //reset tree
                Tree = new Tree.Tree();

                try
                {
                    WebClient webClient = new WebClient();
                    webClient.Headers.Add("User-Agent", repo);


                    if (!Directory.Exists(releaseDir))
                    {
                        Directory.CreateDirectory(releaseDir);
                    }
                    //fetch archive and extract it
                    string releaseZipFilePath = releaseDir + "/archive.zip";
                    if (!File.Exists(releaseZipFilePath))
                    {
                        webClient.DownloadFile(new Uri(r.ZipballUrl.Replace("https://", "http://")), releaseZipFilePath);
                    }
                    if (Directory.GetDirectories(releaseDir).Length == 0) //the archive extracts in its own folder. If a folder is created already the zip was extracted (TODO user folders)
                    {
                        entry.ExtractZip(releaseZipFilePath, releaseDir, null);
                    }
                }
                catch (WebException e)
                {
                    var xx = 5;
                }

                versionTrees.Add(r.TagName, GetTreeJson(releaseDir));

            }

            return versionTrees;
        }

        /// <summary>
        /// Looks for a tree json in @releaseDir and returns it, or builds the tree and serializes it.
        /// </summary>
        /// <param name="releaseDir"></param>
        /// <returns></returns>
        private string GetTreeJson(string releaseDir)
        {
            string treePath = releaseDir + "/" + TREE_FILE_NAME;
            string jsonTree = "{}";
            if (File.Exists(treePath)) //tree was generated earlier already
            {
                jsonTree = File.ReadAllText(treePath);
                Tree = JsonConvert.DeserializeObject<Tree.Tree>(jsonTree);//TODO not needed?
            }
            else
            {
                string solutionFilePath = Directory.GetFiles(releaseDir, "*.sln", SearchOption.AllDirectories).First();
                Tree.Identifier = solutionFilePath;
                Solution solution = new Solution(solutionFilePath);

                BuildTree(solution);

                //generate json and save it
                jsonTree = JsonConvert.SerializeObject(Tree);
                SaveTree(jsonTree, releaseDir);
            }
            return jsonTree;
        }

        /// <summary>
        /// Visites the solution using multiple visitors to build a tree of namespaces and classes.
        /// </summary>
        /// <param name="solution"></param>
        private void BuildTree(Solution solution)
        {
            //generate namespace nodes of the tree
            foreach (var file in solution.AllFiles)
            {
                CSharpAstResolver astResolver = new CSharpAstResolver(file.Project.Compilation, file.SyntaxTree, file.UnresolvedTypeSystemForFile);
                file.SyntaxTree.AcceptVisitor(new NamespaceVisitor(), astResolver);
            }

            //generate class nodes (leaves) of the tree
            foreach (var file in solution.AllFiles)
            {
                CSharpAstResolver astResolver = new CSharpAstResolver(file.Project.Compilation, file.SyntaxTree, file.UnresolvedTypeSystemForFile);
                file.SyntaxTree.AcceptVisitor(new DependencyVisitor(), astResolver);
            }
        }

        /// <summary>
        /// Save the tree to a file for later reuse.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="dir"></param>
        private void SaveTree(string tree, string dir)
        {
            string treeFilePath = dir + "/" + TREE_FILE_NAME;
            if (!File.Exists(treeFilePath))
            {
                File.Create(treeFilePath).Close();
            }

            File.WriteAllText(treeFilePath, tree);
        }

    }
}
