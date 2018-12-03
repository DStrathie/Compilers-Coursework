using Triangle.Compiler.SyntaxTrees.Visitors;

namespace Triangle.Compiler.SyntaxTrees.Parameters
{
    public class EmptyParameterSequence : ParameterSequence
    {
        public EmptyParameterSequence(SourcePosition position)
            : base(position)
        {
            Compiler.WriteDebuggingInfo($"Creating {this.GetType().Name}");
        }

        public override TResult Visit<TArg, TResult>(IParameterSequenceVisitor<TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitEmptyParameterSequence(this, arg);
        }
    }
}