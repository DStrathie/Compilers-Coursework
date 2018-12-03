using Triangle.Compiler.SyntaxTrees.Commands;
using Triangle.Compiler.SyntaxTrees.Declarations;
using Triangle.Compiler.SyntaxTrees.Expressions;
using Triangle.Compiler.SyntaxTrees.Parameters;
using Triangle.Compiler.SyntaxTrees.Terminals;
using Triangle.Compiler.SyntaxTrees.Types;


namespace Triangle.Compiler.SyntacticAnalyzer
{
    public partial class Parser
    {

        ///////////////////////////////////////////////////////////////////////////////
        //
        // DECLARATIONS
        //
        ///////////////////////////////////////////////////////////////////////////////

        /**
         * Parses the declaration, and constructs an AST to represent its phrase
         * structure.
         * 
         * @return a {@link triangle.compiler.syntax.trees.declarations.Declaration}
         * 
         * @throws SyntaxError
         *           a syntactic error
         * 
         */
        Declaration ParseDeclaration()
        {
            Compiler.WriteDebuggingInfo("Parsing Declaration");
            Location startLocation = tokens.Current.Start;
            Declaration declaration = ParseSingleDeclaration();
            while (tokens.Current.Kind == TokenKind.Semicolon)
            {
                AcceptIt();
                Declaration declarationinWhile = ParseSingleDeclaration();
                SourcePosition declarationPosition = new SourcePosition(startLocation, tokens.Current.Finish);
                declaration = new SequentialDeclaration(declaration, declarationinWhile, declarationPosition);

            }
            return declaration;

        }

        /**
         * Parses the single declaration, and constructs an AST to represent its
         * phrase structure.
         * 
         * @return a {@link triangle.compiler.syntax.trees.declarations.Declaration}
         * 
         * @throws SyntaxError
         *           a syntactic error
         * 
         */
        Declaration ParseSingleDeclaration()
        {
            Compiler.WriteDebuggingInfo("Parsing Single Declaration");
            Location startLocation = tokens.Current.Start;
            switch (tokens.Current.Kind)
            {

                case TokenKind.Const:
                    {
                        AcceptIt();
                        
                        Identifier identifier = ParseIdentifier();
                        
                        Accept(TokenKind.Is);
                       
                        Expression expression = ParseExpression();
                        SourcePosition declarationPosition = new SourcePosition(startLocation, tokens.Current.Finish);
                        return new ConstDeclaration(identifier, expression, declarationPosition);
                       

                    }

                case TokenKind.Var:
                    {
                        AcceptIt();
                        
                        Identifier identifier = ParseIdentifier();
                        Accept(TokenKind.Colon);
                        
                        TypeDenoter denoter = ParseTypeDenoter();
                        
                        if (tokens.Current.Kind == TokenKind.Becomes)
                        {
                            AcceptIt();
                
                            Expression expression = ParseExpression();

                            SourcePosition declarationPosition = new SourcePosition(startLocation, tokens.Current.Finish);
                            return new InitDeclaration(identifier, denoter, expression, declarationPosition);
                            
                        }
                        else
                        {
                            SourcePosition declarationPosition = new SourcePosition(startLocation, tokens.Current.Finish);
                            return new VarDeclaration(identifier, denoter, declarationPosition);
                        }
                    }

                default:
                    {
                        RaiseSyntacticError("\"%\" cannot start a declaration", tokens.Current);
                        return null;
                    }

            }

        }
    }
}