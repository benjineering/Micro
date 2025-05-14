using Micro.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Micro.Server.SyntaxParsers
{
    static class ClassParser
    {
        public static ClassParserResult Parse(GeneratorAttributeSyntaxContext context)
        {
            if (!(context.TargetNode is PropertyDeclarationSyntax property))
            {
                return new ClassParserResult
                {
                    Diagnostics = MicroDiagnostics.CreateArray(
                        MicroDiagnosticType.NotAProperty, 
                        new Location[] { context.TargetNode.GetLocation() }
                    ),
                };
            }

            var initializer = property.ExpressionBody?.Expression
                ?? (property.Initializer?.Value);

            if (!(initializer is ArrayCreationExpressionSyntax arrayCreation))
                return new ClassParserResult
                {
                    Diagnostics = MicroDiagnostics.CreateArray(
                        MicroDiagnosticType.NotAProperty,
                        new Location[] { initializer.GetLocation() }
                    ),
                };

            var classes = new List<Class>();

            foreach (var expr in arrayCreation.Initializer.Expressions)
            {
                if (expr is ObjectCreationExpressionSyntax objCreation)
                {
                    var classData = ParseClass(objCreation);
                    classes.Add(classData);
                }
            }

            return new ClassParserResult { }
        }

        private static Class ParseClass(ObjectCreationExpressionSyntax expr)
        {
            var args = expr.ArgumentList.Arguments;

            var typeNameExpr = (ObjectCreationExpressionSyntax)args[0].Expression;
            var typeName = ParseTypeName(typeNameExpr);

            var methodsArray = (ArrayCreationExpressionSyntax)args[1].Expression;

            var methods = methodsArray.Initializer.Expressions
                .OfType<ObjectCreationExpressionSyntax>()
                .Select(ParseMethod)
                .ToArray();

            return new Class(typeName, methods);
        }

        private static Method ParseMethod(ObjectCreationExpressionSyntax expr)
        {
            var args = expr.ArgumentList.Arguments;

            var name = args[0].Expression.ToString().Trim('"');

            var parametersArray = (ArrayCreationExpressionSyntax)args[1].Expression;
            var parameters = parametersArray.Initializer.Expressions
                .OfType<ObjectCreationExpressionSyntax>()
                .Select(ParseParameter)
                .ToArray();

            var returnTypeExpr = (ObjectCreationExpressionSyntax)args[2].Expression;
            var returnType = ParseTypeName(returnTypeExpr);

            return new Method(name, parameters, returnType);
        }

        private static Parameter ParseParameter(ObjectCreationExpressionSyntax expr)
        {
            var args = expr.ArgumentList.Arguments;

            var name = args[0].Expression.ToString().Trim('"');

            var typeNameExpr = (ObjectCreationExpressionSyntax)args[1].Expression;
            var typeName = ParseTypeName(typeNameExpr);

            return new Parameter(name, typeName);
        }

        private static TypeName ParseTypeName(ObjectCreationExpressionSyntax expr)
        {
            var args = expr.ArgumentList.Arguments;

            var @namespace = args[0].Expression.ToString().Trim('"');
            var name = args[1].Expression.ToString().Trim('"');

            return new TypeName(@namespace, name);
        }
    }
}
