using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalysisIO.Tree;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.Semantics;

namespace AnalysisIO.Visitor
{
    class NamespaceVisitor : DepthFirstAstVisitor<CSharpAstResolver, object>
    {

        public override object VisitNamespaceDeclaration(NamespaceDeclaration namespaceDeclaration, CSharpAstResolver astResolver)
        {
            //resolve namespace declaration
            NamespaceResolveResult resolvedNamespace = astResolver.Resolve(namespaceDeclaration) as NamespaceResolveResult;

            //create a node for the namespace in the tree
            NamespaceNode node = new NamespaceNode {Identifier = resolvedNamespace.NamespaceName};
            SourceImporter.SourceImporter.Tree.AddChild(node);

            return base.VisitNamespaceDeclaration(namespaceDeclaration, astResolver);
        }
    }
}
