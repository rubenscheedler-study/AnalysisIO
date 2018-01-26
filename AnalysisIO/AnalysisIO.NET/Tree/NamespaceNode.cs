using System.Linq;

namespace AnalysisIO.NET.Tree
{
    public class NamespaceNode : TreeNode
    {
        public ClassNode GetClassNode(string className) => Children.FirstOrDefault(c => c.Identifier == className) as ClassNode;
    }
}
