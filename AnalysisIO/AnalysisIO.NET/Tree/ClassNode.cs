using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AnalysisIO.NET.Tree
{
    public class ClassNode : TreeNode
    {
        [JsonIgnore]
        public List<ClassNode> DependencyNodes { get; }
        public List<string> Dependencies { get; }

        public ClassNode() : base()
        {
            DependencyNodes = new List<ClassNode>();
            Dependencies = new List<string>();
        }

        public ClassNode(string className, object oldNode) : base(className, oldNode)
        {
            DependencyNodes = new List<ClassNode>();
            Dependencies = new List<string>();
        }

        /// <summary>
        /// Adds classNode to the dependencies ONLY of it is not already in that list.
        /// </summary>
        /// <param name="classNode"></param>
        public void AddDependency(ClassNode classNode)
        {
            if (DependencyNodes.Any(d => d.Identifier == classNode.Identifier))
            {
                return;
            }

            DependencyNodes.Add(classNode);
            Dependencies.Add(classNode.Identifier);
        }
    }
}

