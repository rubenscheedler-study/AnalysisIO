using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisIO.NET.Tree
{
    public class ClassNode : TreeNode
    {
        public List<ClassNode> Dependencies { get; }

        public ClassNode() : base()
        {
            Dependencies = new List<ClassNode>();
        }

        public ClassNode(string className, object oldNode) : base(className, oldNode)
        {
            Dependencies = new List<ClassNode>();
        }

        /// <summary>
        /// Adds classNode to the dependencies ONLY of it is not already in that list.
        /// </summary>
        /// <param name="classNode"></param>
        public void AddDependency(ClassNode classNode)
        {
            if (Dependencies.Any(d => d.Identifier == classNode.Identifier))
            {
                return;
            }

            Dependencies.Add(classNode);
        }
    }
}
