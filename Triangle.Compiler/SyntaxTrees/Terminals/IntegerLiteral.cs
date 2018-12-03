using Triangle.Compiler.SyntacticAnalyzer;

namespace Triangle.Compiler.SyntaxTrees.Terminals
{
    public class IntegerLiteral : Terminal
    {
        public int Value { get { return int.Parse(Spelling); } }

        public IntegerLiteral(string spelling, SourcePosition position)
            : base(spelling, position)
        {
            Compiler.WriteDebuggingInfo($"Creating {this.GetType().Name}");
        }

        public IntegerLiteral(Token token) : this(token.Spelling, token.Position)
        {
            Compiler.WriteDebuggingInfo($"Creating {this.GetType().Name}");
        }

        public IntegerLiteral(int value, SourcePosition position)
            : base(value.ToString(), position)
        {
            Compiler.WriteDebuggingInfo($"Creating {this.GetType().Name}");
        }
    }
}