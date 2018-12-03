using Triangle.Compiler.SyntaxTrees.Commands;
using Triangle.Compiler.SyntaxTrees.Declarations;
using Triangle.Compiler.SyntaxTrees.Expressions;
using Triangle.Compiler.SyntaxTrees.Parameters;
using Triangle.Compiler.SyntaxTrees.Terminals;

namespace Triangle.Compiler.SyntacticAnalyzer
{
    public partial class Parser
    {

        ///////////////////////////////////////////////////////////////////////////////
        //
        // PARAMETERS
        //
        ///////////////////////////////////////////////////////////////////////////////
        
   

        
        ParameterSequence ParseParameters()
        {
            Compiler.WriteDebuggingInfo("Parsing Parameters");
            Location startLocation = tokens.Current.Position.Start;
 
            if (tokens.Current.Kind == TokenKind.RightBracket)
            {
                SourcePosition parameterPosition = new SourcePosition(startLocation, tokens.Current.Position.Finish);
                
               
                return new EmptyParameterSequence(parameterPosition);
            }
            Parameter parameter = ParseParameter();
            if (tokens.Current.Kind == TokenKind.Comma)
            {
                AcceptIt();

                ParameterSequence parameters = ParseParameters();
                SourcePosition parameterPosition = new SourcePosition(startLocation, tokens.Current.Position.Finish);
                return new MultipleParameterSequence(parameter, parameters, parameterPosition);
            }
            else
            {
                SourcePosition parameterPosition = new SourcePosition(startLocation, tokens.Current.Position.Finish);
                return new SingleParameterSequence(parameter,parameterPosition);
            }
           
        }
        
        Parameter ParseParameter()
        {
            Compiler.WriteDebuggingInfo("Parsing Parameter");
            Location startLocation = tokens.Current.Position.Start;
            switch (tokens.Current.Kind)
            {
                case TokenKind.Identifier:
                case TokenKind.IntLiteral:
                case TokenKind.CharLiteral:
                case TokenKind.Operator:
                case TokenKind.LeftBracket:
                    {
                        Compiler.WriteDebuggingInfo("Parsing Value Parameter");

                        Expression expression = ParseExpression();
                        SourcePosition parameterPosition = new SourcePosition(startLocation, tokens.Current.Position.Finish);
                        return new ValueParameter(expression, parameterPosition);
                        //break;
                    }

                case TokenKind.Var:
                    {
                        Compiler.WriteDebuggingInfo("Parsing Variable Parameter");
                        AcceptIt();

                        Identifier identifier = ParseIdentifier();
                        SourcePosition parameterPosition = new SourcePosition(startLocation, tokens.Current.Position.Finish);
                        return new VarParameter(identifier, parameterPosition);
                        //break;
                    }

                default:
                    {
                        RaiseSyntacticError("\"%\" cannot start a parameter", tokens.Current);
                        return null;
                        //break;
                    }
            }

        }
    }
}