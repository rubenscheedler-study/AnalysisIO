using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AnalysisIO.NET.Tree
{
    public class TreeNode
    {
        public List<TreeNode> Children { get; }
        public string Identifier { get; set; }
        [JsonIgnore]
        public object OldNode { get; set; }//transient

        public TreeNode()
        {
            Children = new List<TreeNode>();
        }

        public TreeNode(string identifier, object oldNode) : this()
        {
            Identifier = identifier;
            OldNode = oldNode;
        }
        /// <summary>
        /// Adds the childnode ONLY if it does not already exists.
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(TreeNode child)
        {
            if (Children.Any(c => c.Identifier == child.Identifier))
            {
                return;
            }
            Children.Add(child);
        }
    }
}
