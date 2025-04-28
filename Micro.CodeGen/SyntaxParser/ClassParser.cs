using Micro.CodeGen.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Micro.CodeGen.SyntaxParser
{
    static class ClassParser
    {
        public static bool NodeIsRequestHandlerClass(SyntaxNode node)
        {
            if (!(node is ClassDeclarationSyntax klass) || klass.AttributeLists.Count == 0)
                return false;

            foreach (var attrList in klass.AttributeLists)
            {
                foreach (var attr in attrList.Attributes)
                {
                    // TODO: fix this hot mess (name is different between test and dev)
                    var attrName = attr.Name.ToFullString();
                    if (attrName == "RequestHandler" || attrName == "Micro.Requests.RequestHandlerAttribute")
                        return true;
                }
            }

            return false;
        }

        public static Klass Parse(GeneratorSyntaxContext context)
        {
            if (!(context.SemanticModel.GetDeclaredSymbol(context.Node) is INamedTypeSymbol klass))
                return null;

            var name = klass.Name;
            var ns = klass.ContainingNamespace.ToDisplayString();

            var methods = klass.GetMembers()
                .OfType<IMethodSymbol>()
                .Select(x =>new Method
                {
                    Name = x.Name,
                    ReturnType = x.ReturnType,
                    Parameters = x.Parameters.ToArray()
                })
                .Where(x => x != null)
                .ToArray();

            return new Klass
            {
                Name = name,
                Namespace = ns,
            };
        }

        static string GetNamespace(ClassDeclarationSyntax classDeclaration)
        {
            SyntaxNode parent = classDeclaration.Parent;

            while (parent != null)
            {
                if (parent is NamespaceDeclarationSyntax namespaceDeclaration)
                    return namespaceDeclaration.Name.ToString();
                else if (parent is FileScopedNamespaceDeclarationSyntax fileScopedNamespace)
                    return fileScopedNamespace.Name.ToString();

                parent = parent.Parent;
            }

            return null;
        }
    }
}
