using Triangle.Compiler.SyntacticAnalyzer;
using Triangle.Compiler.SyntaxTrees;
using Triangle.Compiler.ContextualAnalyzer;
using Triangle.Compiler.CodeGenerator;

namespace Triangle.Compiler
{
    public class Compiler
    {
        private const bool DEBUG = true;

        public static void WriteDebuggingInfo(object message)
        {
            if (DEBUG) System.Console.WriteLine(message.ToString());
        }



        /// <summary>
        /// The error reporter.
        /// </summary>
        private ErrorReporter ErrorReporter { get; }

        /// <summary>
        /// The source file to compile.
        /// </summary>
        private SourceFile Source { get; }

        /// <summary>
        /// The lexical analyzer.
        /// </summary>
        private Scanner Scanner { get; }

        /// <summary>
        /// The syntactic analyzer.
        /// </summary>
        private Parser Parser { get; }


        /// <summary>
        /// The Checker.
        /// </summary>
        Checker checker;

        Encoder encoder;



        /// <summary>
        /// Creates a compiler for the given source file.
        /// </summary>
        /// <param name="sourceFileName">
        /// a File that specifies the source program
        /// </param>
        public Compiler(string sourceFileName, ErrorReporter errorReporter)
        {
            ErrorReporter = errorReporter;
            Source = new SourceFile(sourceFileName);
            Scanner = new Scanner(Source);
            Parser = new Parser(Scanner, ErrorReporter);
            checker = new Checker(ErrorReporter);
            encoder = new Encoder(ErrorReporter);
        }

        /// <summary>
        /// Compiles the source program to TAM machine code.
        /// </summary>
        /// <param name="showingTable">
        /// a boolean that determines if the object description details are to
        /// be displayed during code generation (not currently implemented)
        /// </param>
        /// <returns>
        /// true if the source program is free of compile-time errors, otherwise false
        /// </returns>
        public bool CompileProgram()
        {
            ErrorReporter.ReportMessage("********** Triangle Compiler **********");

            if (!Source.IsValid)
            {
                ErrorReporter.ReportMessage($"Cannot access source file \"{Source.Name}\".");
                return false;
            }

            // 1st pass
            ErrorReporter.ReportMessage("Syntactic Analysis ...");
            Program program = Parser.ParseProgram();
            if (ErrorReporter.HasErrors)
            {
                ErrorReporter.ReportMessage("Compilation was unsuccessful.");
                return false;
            }
            // 2nd pass
            ErrorReporter.ReportMessage("Contextual Analysis ...");
            checker.Check(program);
            if (ErrorReporter.HasErrors)
            {
                ErrorReporter.ReportMessage("Compilation was unsuccessful.");
                return false;
            }

            ErrorReporter.ReportMessage("Code Generation ...");
            encoder.EncodeRun(program);
            if (ErrorReporter.HasErrors)
            {
                ErrorReporter.ReportMessage("Compilation was unsuccessful.");
                return false;
            }
            encoder.SaveObjectProgram(objectFileName: "obj11.tam");

            System.Console.WriteLine(program);

            ErrorReporter.ReportMessage("Compilation was successful.");
            return true;

            

        }

        /// <summary>
        /// Triangle compiler main program.
        /// </summary>
        /// <param name="args">
        /// a string array containing the command-line arguments. This must
        /// be a single string specifying the source filename.
        /// </param>
        public static void Main(string[] args)
        {
            ErrorReporter errorReporter = new ErrorReporter();

            if (args.Length != 1)
            {
                errorReporter.ReportMessage("Usage: Compiler.exe source");
            }
            else
            {
                string sourceFileName = args[0];
                if (sourceFileName != null)
                {
                    Compiler compiler = new Compiler(sourceFileName, errorReporter);
                    compiler.CompileProgram();
                }
            }
        }
    }
}