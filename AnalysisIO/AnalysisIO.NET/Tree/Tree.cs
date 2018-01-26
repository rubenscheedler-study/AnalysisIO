using System.Linq;

namespace AnalysisIO.NET.Tree
{
    public class Tree : TreeNode
    {
        public bool ContainsNamespace(string nameSpace) => Children.Any(c => c.Identifier == nameSpace);

        public NamespaceNode GetNamespaceNode(string nameSpace) => Children.FirstOrDefault(c => c.Identifier == nameSpace) as NamespaceNode;

        /// <summary>
        /// Adds the passed classNode to the tree, under the passed namespace (if that exists) if such a class node does not already exist.
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="classNode"></param>
        /// <returns></returns>
        public ClassNode AddOrGetClassNode(string nameSpace, ClassNode classNode)
        {
            if (!ContainsNamespace(nameSpace))
            {
                return null;
            }

            NamespaceNode n = GetNamespaceNode(nameSpace);
            ClassNode c = n.GetClassNode(classNode.Identifier);
            if (c != null)
            {
                return c; //node is already present
            }

            n.AddChild(classNode);
            return classNode;
        }
    }
}
