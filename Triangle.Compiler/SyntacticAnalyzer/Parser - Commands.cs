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
        // COMMANDS
        //
        ///////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Parses the command, and constructs an AST to represent its phrase
        /// structure.
        /// </summary>
        /// <returns>
        /// a <link>Triangle.SyntaxTrees.Commands.Command</link>
        /// </returns>
        /// <throws type="SyntaxError">
        /// a syntactic error
        /// </throws>
        Command ParseCommand()
        {
            Compiler.WriteDebuggingInfo("Parsing Command");
            Location startLocation = tokens.Current.Start;
            Command command = ParseSingleCommand();
            while (tokens.Current.Kind == TokenKind.Semicolon)
            {
                AcceptIt();
                Command command2 = ParseSingleCommand();
                SourcePosition commandPosition = new SourcePosition(startLocation, tokens.Current.Finish);
                command = new SequentialCommand(command, command2, commandPosition);
            }
            return command;
        }

        /// <summary>
        /// Parses the single command, and constructs an AST to represent its phrase
        /// structure.
        /// </summary>
        /// <returns>
        /// a {@link triangle.compiler.syntax.trees.commands.Command}
        /// </returns>
        /// <throws type="SyntaxError">
        /// a syntactic error
        /// </throws>
        Command ParseSingleCommand()
        {
            Compiler.WriteDebuggingInfo("Parsing Single Command");
            Location startLocation = tokens.Current.Start;

            switch (tokens.Current.Kind)
            {
                case TokenKind.Identifier:
                    {
                        Compiler.WriteDebuggingInfo("Parsing Assignment Command or Call Command");
                        Identifier identifier = ParseIdentifier();
                        if (tokens.Current.Kind == TokenKind.LeftBracket)
                        {
                            Compiler.WriteDebuggingInfo("Parsing Call Command");
                            AcceptIt();
                            ParameterSequence parameters = ParseParameters();
                            Accept(TokenKind.RightBracket);
                            SourcePosition commandPosition = new SourcePosition(startLocation, tokens.Current.Finish);
                            return new CallCommand(identifier, parameters, commandPosition);
                        }
                        else if (tokens.Current.Kind == TokenKind.Becomes)
                        {
                            Compiler.WriteDebuggingInfo("Parsing Assignment Command");
                            Accept(TokenKind.Becomes);
                            Expression expression = ParseExpression();
                            SourcePosition commandPosition = new SourcePosition(startLocation, tokens.Current.Finish);
                            return new AssignCommand(identifier, expression, commandPosition);
                        }
                        else
                        {
                            RaiseSyntacticError("Expected either an assignment or function call but found a \"%\"", tokens.Current);
                            return null;
                        }
                    }

                case TokenKind.Begin:
                    {
                        Compiler.WriteDebuggingInfo("Parsing Begin Command");

                        AcceptIt();
                        Command command = ParseCommand();
                        
                        Accept(TokenKind.End);
                        SourcePosition commandPosition = new SourcePosition(startLocation, tokens.Current.Finish);
                        return command;

                       
                    }

                case TokenKind.Let:
                    {
                        Compiler.WriteDebuggingInfo("Parsing Let Command");
                        AcceptIt();
                        //ParseDeclaration();
                        Declaration declaration = ParseDeclaration();
                        Accept(TokenKind.In);
                        //ParseSingleCommand();
                        Command command = ParseCommand();
                        SourcePosition commandPosition = new SourcePosition(startLocation, tokens.Current.Finish);
                        return new LetCommand(declaration, command, commandPosition);
                    }

                case TokenKind.If:
                    {
                        Compiler.WriteDebuggingInfo("Parsing If Command");
                        AcceptIt();
                        //ParseExpression();
                        Expression expression = ParseExpression();
                        Accept(TokenKind.Then);
                        //ParseSingleCommand();
                        Command command = ParseCommand();
                        Accept(TokenKind.Else);
                        Command command1 = ParseCommand();
                        SourcePosition commandPosition = new SourcePosition(startLocation, tokens.Current.Finish);
                        return new IfCommand(expression, command, command1, commandPosition);
                    }

                case TokenKind.While:
                    {
                        Compiler.WriteDebuggingInfo("Parsing While Command");
                        AcceptIt();
                        
                        Expression expression = ParseExpression();
                        Accept(TokenKind.Do);
                        
                        Command command = ParseCommand();
                        SourcePosition commandPosition = new SourcePosition(startLocation, tokens.Current.Finish);
                        return new WhileCommand(expression, command, commandPosition);
                    }

                case TokenKind.Skip:
                    {
                        Compiler.WriteDebuggingInfo("Parsing Skip Command");
                        AcceptIt();
                        SourcePosition commandPosition = new SourcePosition(startLocation, tokens.Current.Finish);
                        return new SkipCommand(commandPosition);
                    }

                default:
                    RaiseSyntacticError("\"%\" cannot start a command", tokens.Current);
                    return null;
            }
        }
    }
}