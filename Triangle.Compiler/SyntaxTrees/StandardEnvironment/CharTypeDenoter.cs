using Triangle.AbstractMachine;
using Triangle.Compiler.SyntaxTrees.Visitors;

namespace Triangle.Compiler.SyntaxTrees.Types
{
    public class CharTypeDenoter : TypeDenoter
    {
        public override int Size { get { return Machine.CharacterSize; } }

        public CharTypeDenoter() : base(SourcePosition.Empty) { }

        public override TResult Visit<TArg, TResult>(ITypeDenoterVisitor<TArg, TResult> visitor, TArg arg)
        {
            return visitor.VisitCharTypeDenoter(this, arg);
        }
    }
}