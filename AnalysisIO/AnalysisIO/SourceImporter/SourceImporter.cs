using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AnalysisIO_Console.Resolver;
using AnalysisIO_Console.Visitor;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using Octokit;

namespace AnalysisIO_Console.SourceImporter
{
    public class SourceImporter
    {
        private static readonly string DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/analysisIO/";
        private const string TreeFileName = "tree.json";
        public static Tree.Tree Tree;


        /// <summary>
        /// Returns a dictionary of all releases of the given project.
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> ImportSource(string repo, string project)
        {
            GitHubClient client = new GitHubClient(new ProductHeaderValue(repo));
            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll(repo, project);
            return ImportReleases(repo, project, releases.ToList());
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
            GitHubClient client = new GitHubClient(new ProductHeaderValue(repo));
            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll(repo, project);
            return ImportReleases(repo, project, releases.Where(r => r.TagName == release1).ToList());
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
            GitHubClient client = new GitHubClient(new ProductHeaderValue(repo));
            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll(repo, project);
            return ImportReleases(repo, project, releases.Where(r => r.TagName == release1 || r.TagName == release2).ToList());
        }

        public Dictionary<string, string> ImportReleases(string repo, string project, List<Release> releases)
        {

            if (releases.Count == 0)
            {
                throw new Exception("No releases found or release name is invalid for this project");
            }

            //dict of version and its serialized tree
            Dictionary<string, string> versionTrees = new Dictionary<string, string>();

            FastZip entry = new FastZip();
            foreach (Release release in releases)
            {
                string releaseDirectory = $"{DefaultPath}/{repo}/{project}/{release.TagName}";

                //reset tree
                Tree = new Tree.Tree();

                WebClient webClient = new WebClient();
                webClient.Headers.Add("User-Agent", repo);


                if (!Directory.Exists(releaseDirectory))
                {
                    Directory.CreateDirectory(releaseDirectory);
                }
                //fetch archive and extract it
                string releaseZipFilePath = releaseDirectory + "/archive.zip";
                if (!File.Exists(releaseZipFilePath))
                {
                    webClient.DownloadFile(new Uri(release.ZipballUrl.Replace("https://", "http://")), releaseZipFilePath);
                }
                if (Directory.GetDirectories(releaseDirectory).Length == 0) //the archive extracts in its own folder. If a folder is created already the zip was extracted
                {
                    try
                    {
                        entry.ExtractZip(releaseZipFilePath, releaseDirectory, null);
                    }
                    catch (PathTooLongException)
                    {
                        DeleteDirectory(releaseDirectory, true);
                        throw;
                    }
                }
                versionTrees.Add(release.TagName, GetTreeJson(releaseDirectory));

            }

            return versionTrees;
        }

        public static void DeleteDirectory(string targetDirectory, bool keepArchive)
        {
            string[] files = keepArchive ? new string[0] : Directory.GetFiles(targetDirectory);
            string[] directories = Directory.GetDirectories(targetDirectory);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string directory in directories)
            {
                DeleteDirectory(directory, false);
            }

            if(!keepArchive)
            {
                Directory.Delete(targetDirectory, false);
            }
        }

        /// <summary>
        /// Looks for a tree json in @releaseDirectory and returns it, or builds the tree and serializes it.
        /// </summary>
        /// <param name="releaseDirectory"></param>
        /// <returns></returns>
        private string GetTreeJson(string releaseDirectory)
        {
            string treePath = releaseDirectory + "/" + TreeFileName;
            string jsonTree;
            if (File.Exists(treePath)) //tree was generated earlier already
            {
                jsonTree = File.ReadAllText(treePath);
            }
            else
            {
                string solutionFilePath = Directory.GetFiles(releaseDirectory, "*.sln", SearchOption.AllDirectories).First();
                Tree.Identifier = solutionFilePath;
                Solution solution = new Solution(solutionFilePath);

                BuildTree(solution);

                //generate json and save it
                jsonTree = JsonConvert.SerializeObject(Tree);
                SaveTree(jsonTree, releaseDirectory);
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
            foreach (CSharpFile file in solution.AllFiles)
            {
                CSharpAstResolver astResolver = new CSharpAstResolver(file.Project.Compilation, file.SyntaxTree, file.UnresolvedTypeSystemForFile);
                file.SyntaxTree.AcceptVisitor(new NamespaceVisitor(), astResolver);
            }

            //generate class nodes (leaves) of the tree
            foreach (CSharpFile file in solution.AllFiles)
            {
                CSharpAstResolver astResolver = new CSharpAstResolver(file.Project.Compilation, file.SyntaxTree, file.UnresolvedTypeSystemForFile);
                file.SyntaxTree.AcceptVisitor(new DependencyVisitor(), astResolver);
            }
        }

        /// <summary>
        /// Save the tree to a file for later reuse.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="directory"></param>
        private void SaveTree(string tree, string directory)
        {
            string treeFilePath = directory + "/" + TreeFileName;
            if (!File.Exists(treeFilePath))
            {
                File.Create(treeFilePath).Close();
            }

            File.WriteAllText(treeFilePath, tree);
        }

    }
}
