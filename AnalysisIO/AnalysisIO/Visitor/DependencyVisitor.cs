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
    class DependencyVisitor : DepthFirstAstVisitor<CSharpAstResolver, object>
    {
        public override object VisitInvocationExpression(InvocationExpression invocationExpression, CSharpAstResolver astResolver)
        {
            InvocationResolveResult rr = astResolver.Resolve(invocationExpression) as InvocationResolveResult;
            if (rr != null)
            {
                ResolveResult target = rr.TargetResult;

                Tree.Tree t = SourceImporter.SourceImporter.Tree;

                //1) target node
                ClassNode targetNode = new ClassNode(target.Type.FullName, target);
                targetNode = t.AddOrGetClassNode(target.Type.Namespace, targetNode);//override if already in tree with that node

                if (targetNode != null)
                {
                    //2) caller node (can't be null)
                    //find out class in which the invocation statement is found
                    TypeDeclaration classDeclaration = invocationExpression.Ancestors.Where(a => a is TypeDeclaration).Cast<TypeDeclaration>().First();
                    //resolve the class declaration
                    TypeResolveResult resolvedClass = astResolver.Resolve(classDeclaration) as TypeResolveResult;
                    ClassNode callerNode = new ClassNode(resolvedClass.Type.FullName, resolvedClass);
                    callerNode = t.AddOrGetClassNode(resolvedClass.Type.Namespace, callerNode); //override if already in tree with that node

                    //3) edge (dependency) between caller and target
                    callerNode.AddDependency(targetNode);
                }
            }
            return base.VisitInvocationExpression(invocationExpression, astResolver);
        }

        public override object VisitFieldDeclaration(FieldDeclaration fieldDeclaration, CSharpAstResolver astResolver)
        {
           ResolveResult resolveResult = astResolver.Resolve(fieldDeclaration.ReturnType);
           if (resolveResult != null)
           {
                Tree.Tree t = SourceImporter.SourceImporter.Tree;

                //1) target node
                ClassNode targetNode = new ClassNode(resolveResult.Type.FullName, resolveResult);
                targetNode = t.AddOrGetClassNode(resolveResult.Type.Namespace, targetNode);//override if already in tree with that node

                if (targetNode != null)
                {
                    //2) caller node (can't be null)
                    //find out class in which the invocation statement is found
                    TypeDeclaration classDeclaration = fieldDeclaration.Ancestors.Where(a => a is TypeDeclaration).Cast<TypeDeclaration>().First();
                    //resolve the class declaration
                    TypeResolveResult resolvedClass = astResolver.Resolve(classDeclaration) as TypeResolveResult;
                    ClassNode callerNode = new ClassNode(resolvedClass.Type.FullName, resolvedClass);
                    callerNode = t.AddOrGetClassNode(resolvedClass.Type.Namespace, callerNode); //override if already in tree with that node

                    //3) edge (dependency) between caller and target
                    callerNode.AddDependency(targetNode);
                }
            }
            return base.VisitFieldDeclaration(fieldDeclaration, astResolver);
        }
        public override object VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration, CSharpAstResolver astResolver)
        {
            ResolveResult resolveResult = astResolver.Resolve(propertyDeclaration.ReturnType);
            if (resolveResult != null)
            {
                Tree.Tree t = SourceImporter.SourceImporter.Tree;

                //1) target node
                ClassNode targetNode = new ClassNode(resolveResult.Type.FullName, resolveResult);
                targetNode = t.AddOrGetClassNode(resolveResult.Type.Namespace, targetNode);//override if already in tree with that node

                if (targetNode != null)
                {
                    //2) caller node (can't be null)
                    //find out class in which the invocation statement is found
                    TypeDeclaration classDeclaration = propertyDeclaration.Ancestors.Where(a => a is TypeDeclaration).Cast<TypeDeclaration>().First();
                    //resolve the class declaration
                    TypeResolveResult resolvedClass = astResolver.Resolve(classDeclaration) as TypeResolveResult;
                    ClassNode callerNode = new ClassNode(resolvedClass.Type.FullName, resolvedClass);
                    callerNode = t.AddOrGetClassNode(resolvedClass.Type.Namespace, callerNode); //override if already in tree with that node

                    //3) edge (dependency) between caller and target
                    callerNode.AddDependency(targetNode);
                }
            }
            return base.VisitPropertyDeclaration(propertyDeclaration, astResolver);
        }
    }
}
