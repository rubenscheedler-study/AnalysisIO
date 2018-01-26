using System.Linq;

namespace AnalysisIO_Console.Tree
{
    public class NamespaceNode : TreeNode
    {
        public ClassNode GetClassNode(string className) => Children.FirstOrDefault(c => c.Identifier == className) as ClassNode;
    }
}
