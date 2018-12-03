using Triangle.Compiler.SyntacticAnalyzer;
using Triangle.Compiler.SyntaxTrees.Declarations;
using Triangle.Compiler.SyntaxTrees.Types;
using Triangle.Compiler.SyntaxTrees.Visitors;

namespace Triangle.Compiler.SyntaxTrees.Terminals
{
    public class Identifier : Terminal
    {
        public TypeDenoter Type { get; set; }
        public Declaration Declaration { get; set; }
        public bool IsVariable { get { return Declaration is ConstDeclaration || Declaration is VarDeclaration || Declaration is InitDeclaration; } }

        public static readonly Identifier Empty = new Identifier(string.Empty, SourcePosition.Empty);

        public Identifier(string spelling, SourcePosition position)
            : base(spelling, position)
        {
            Compiler.WriteDebuggingInfo($"Creating {this.GetType().Name}");
        }

        public Identifier(Token token)
            : this(token.Spelling, token.Position)
        {
            Compiler.WriteDebuggingInfo($"Creating {this.GetType().Name}");
        }

        public Identifier(string spelling)
            : this(spelling, SourcePosition.Empty)
        {
            Compiler.WriteDebuggingInfo($"Creating {this.GetType().Name}");
        }

        public TResult Visit<TArg, TResult>(ITerminalVisitor<TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitIdentifier(this, arg);
        }
    }
}