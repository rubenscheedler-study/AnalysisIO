using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisIO.NET.Tree
{
    public class NamespaceNode : TreeNode
    {
        public ClassNode GetClassNode(string className)
        {
            return Children.FirstOrDefault(c => c.Identifier == className) as ClassNode;
        }
    }
}
