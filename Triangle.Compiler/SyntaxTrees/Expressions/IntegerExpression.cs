using Triangle.Compiler.SyntaxTrees.Terminals;
using Triangle.Compiler.SyntaxTrees.Visitors;

namespace Triangle.Compiler.SyntaxTrees.Expressions
{
    public class IntegerExpression : Expression
    {
        public IntegerLiteral IntegerLiteral { get; }
        public int Value { get { return IntegerLiteral.Value; } }

        public IntegerExpression(IntegerLiteral integerLiteral, SourcePosition position)
            : base(position)
        {
            Compiler.WriteDebuggingInfo($"Creating {this.GetType().Name}");
            IntegerLiteral = integerLiteral;
        }

        public IntegerExpression(IntegerLiteral integerLiteral)
            : this(integerLiteral, SourcePosition.Empty)
        {
            Compiler.WriteDebuggingInfo($"Creating {this.GetType().Name}");
        }

        public override TResult Visit<TArg, TResult>(IExpressionVisitor<TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitIntegerExpression(this, arg);
        }
    }
}