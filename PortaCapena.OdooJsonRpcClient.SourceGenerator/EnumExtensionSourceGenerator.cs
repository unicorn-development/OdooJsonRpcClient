using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace PortaCapena.OdooJsonRpcClient.SourceGenerator;

[Generator]
public class EnumExtensionSourceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var compilation = context.Compilation;

        var sb = new StringBuilder();
        sb.AppendLine("using System;");
        sb.AppendLine("namespace PortaCapena.OdooJsonRpcClient.Extensions {");
        sb.AppendLine("    public static partial class EnumExtension {");

        foreach (var tree in compilation.SyntaxTrees)
        {
            var semantic = compilation.GetSemanticModel(tree);
            var nodes = tree.GetRoot().DescendantNodes()
                .OfType<EnumDeclarationSyntax>();

            foreach (var enumDeclaration in nodes)
            {
                if (semantic.GetDeclaredSymbol(enumDeclaration) is not INamedTypeSymbol enumSymbol)
                    continue;

                sb.AppendLine($"        public static string Description(this {enumSymbol.ToDisplayString()} value)");
                sb.AppendLine("        {");
                sb.AppendLine("            switch (value)");
                sb.AppendLine("            {");

                foreach (var enumField in enumSymbol.GetMembers()
                             .OfType<IFieldSymbol>()
                             .Where(m => m.HasConstantValue))
                {
                    var attribute = enumField.GetAttributes()
                        .FirstOrDefault(a => a.AttributeClass?.ToString() == "System.ComponentModel.DescriptionAttribute");

                    var constructorArguments = attribute?.ConstructorArguments;

                    if (constructorArguments?.IsDefaultOrEmpty ?? true)
                        continue;

                    if (constructorArguments.Value.First().Value is string description)
                    {
                        var classPath = enumSymbol.ToDisplayString();
                        sb.AppendLine($"                case {classPath}.{enumField.Name}: return \"{description}\";");
                    }
                }

                sb.AppendLine("                default: return value.ToString();");
                sb.AppendLine("            }");
                sb.AppendLine("        }");
            }
        }

        sb.AppendLine("    }");
        sb.AppendLine("}");

        context.AddSource("EnumDescriptionExtensions.g.cs", sb.ToString());
    }
}
