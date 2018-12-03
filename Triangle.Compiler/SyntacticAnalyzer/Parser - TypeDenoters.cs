using Triangle.Compiler.SyntaxTrees.Types;
using Triangle.Compiler.SyntaxTrees.Commands;
using Triangle.Compiler.SyntaxTrees.Declarations;
using Triangle.Compiler.SyntaxTrees.Expressions;
using Triangle.Compiler.SyntaxTrees.Parameters;
using Triangle.Compiler.SyntaxTrees.Terminals;

namespace Triangle.Compiler.SyntacticAnalyzer
{
    public partial class Parser
    {

        // /////////////////////////////////////////////////////////////////////////////
        //
        // TYPE-DENOTERS
        //
        // /////////////////////////////////////////////////////////////////////////////

        /**
         * Parses the type denoter, and constructs an AST to represent its phrase
         * structure.
         * 
         * @return a {@link triangle.compiler.syntax.trees.types.TypeDenoter}
         * 
         * @throws SyntaxError
         *           a syntactic error
         * 
         */
        TypeDenoter ParseTypeDenoter()
        {
            Compiler.WriteDebuggingInfo("Parsing Type Denoter");
            Location startLocation = tokens.Current.Start;
            switch (tokens.Current.Kind)
            {
                case TokenKind.Identifier:
                    {
                        Identifier identifier = ParseIdentifier();
                        SourcePosition typePosition = new SourcePosition(startLocation, tokens.Current.Finish);
                        return new SimpleTypeDenoter(identifier, typePosition);
                        
                    }
                    
                default:
                    {
                        RaiseSyntacticError("\"%\" cannot start a type denoter", tokens.Current);
                        return null;
                        
                    }

            }
        }

    }
}