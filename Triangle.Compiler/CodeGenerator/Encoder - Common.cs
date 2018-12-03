using Triangle.AbstractMachine;
using Triangle.Compiler.CodeGenerator.Entities;
using Triangle.Compiler.SyntaxTrees;
using Triangle.Compiler.SyntaxTrees.Declarations;
using Triangle.Compiler.SyntaxTrees.Terminals;

namespace Triangle.Compiler.CodeGenerator
{
    public partial class Encoder : IEncoderVisitor
    {
        ErrorReporter errorReporter;

        Emitter emitter;

        public Encoder(ErrorReporter errorReporter)
        {
            this.errorReporter = errorReporter;
            emitter = new Emitter(this.errorReporter);
            ElaborateStdEnvironment();
        }

        public void EncodeRun(Program ast)
        {
            ast.Visit(this, Frame.Initial);
            emitter.Emit(OpCode.HALT);
        }

        public void SaveObjectProgram(string objectFileName)
        {
            emitter.SaveObjectProgram(objectFileName);
        }

        // REGISTERS

        // Generates code to fetch the value of a named constant or variable
        // and push it on to the stack.
        // currentLevel is the routine level where the vname occurs.
        // frameSize is the anticipated size of the local stack frame when
        // the constant or variable is fetched at run-time.
        // valSize is the size of the constant or variable's value.

        void EncodeAssign(Identifier identifier, Frame frame, int valSize)
        {
            AddressableEntity baseObject = identifier.Declaration.Entity as AddressableEntity;
            // If indexed = true, code will have been generated to load an index
            // value.
            if (valSize > 255)
            {
                errorReporter.ReportMessage("can't store values larger than 255 words");
                valSize = 255; // to allow code generation to continue
            }
            baseObject.EncodeAssign(emitter, frame, valSize, identifier);
        }

        // Generates code to fetch the value of a named constant or variable
        // and push it on to the stack.
        // currentLevel is the routine level where the vname occurs.
        // frameSize is the anticipated size of the local stack frame when
        // the constant or variable is fetched at run-time.
        // valSize is the size of the constant or variable's value.

        void EncodeFetch(Identifier identifier, Frame frame, int valSize)
        {
            IFetchableEntity baseObject = identifier.Declaration.Entity as IFetchableEntity;
            // If indexed = true, code will have been generated to load an index
            // value.
            if (valSize > 255)
            {
                errorReporter.ReportMessage("can't load values larger than 255 words");
                valSize = 255; // to allow code generation to continue
            }
            baseObject.EncodeFetch(emitter, frame, valSize, identifier);
        }

        // Generates code to compute and push the address of a named variable.
        // vname is the program phrase that names this variable.
        // currentLevel is the routine level where the vname occurs.
        // frameSize is the anticipated size of the local stack frame when
        // the variable is addressed at run-time.

        void EncodeFetchAddress(Identifier identifier, Frame frame)
        {
            AddressableEntity baseObject = identifier.Declaration.Entity as AddressableEntity;
            // If indexed = true, code will have been generated to load an index
            // value.
            baseObject.EncodeFetchAddress(emitter, frame, identifier);
        }

        // Decides run-time representation of a standard constant.
        void ElaborateStdConst(ConstDeclaration decl, int value)
        {
            int typeSize = decl.Expression.Type.Visit(this, null);
            decl.Entity = new KnownValue(typeSize, value);
            Encoder.WriteTableDetails(decl);
        }

        // Decides run-time representation of a standard routine.
        void ElaborateStdPrimRoutine(Declaration routineDeclaration, Primitive primitive)
        {
            routineDeclaration.Entity = new PrimitiveRoutine(Machine.ClosureSize, primitive);
            Encoder.WriteTableDetails(routineDeclaration);
        }

        void ElaborateStdEqRoutine(Declaration routineDeclaration, Primitive primitive)
        {
            routineDeclaration.Entity = new EqualityProcedure(Machine.ClosureSize, primitive);
            Encoder.WriteTableDetails(routineDeclaration);
        }

        void ElaborateStdEnvironment()
        {
            ElaborateStdConst(StandardEnvironment.FalseDecl, Machine.FalseValue);
            ElaborateStdConst(StandardEnvironment.TrueDecl, Machine.TrueValue);
            ElaborateStdPrimRoutine(StandardEnvironment.AddDecl, Primitive.ADD);
            ElaborateStdPrimRoutine(StandardEnvironment.SubtractDecl, Primitive.SUB);
            ElaborateStdPrimRoutine(StandardEnvironment.MultiplyDecl, Primitive.MULT);
            ElaborateStdPrimRoutine(StandardEnvironment.DivideDecl, Primitive.DIV);
            ElaborateStdPrimRoutine(StandardEnvironment.LessDecl, Primitive.LT);
            ElaborateStdPrimRoutine(StandardEnvironment.GreaterDecl, Primitive.GT);
            ElaborateStdPrimRoutine(StandardEnvironment.ChrDecl, Primitive.ID);
            ElaborateStdPrimRoutine(StandardEnvironment.OrdDecl, Primitive.ID);
            ElaborateStdPrimRoutine(StandardEnvironment.EolDecl, Primitive.EOL);
            ElaborateStdPrimRoutine(StandardEnvironment.EofDecl, Primitive.EOF);
            ElaborateStdPrimRoutine(StandardEnvironment.GetDecl, Primitive.GET);
            ElaborateStdPrimRoutine(StandardEnvironment.PutDecl, Primitive.PUT);
            ElaborateStdPrimRoutine(StandardEnvironment.GetintDecl, Primitive.GETINT);
            ElaborateStdPrimRoutine(StandardEnvironment.PutintDecl, Primitive.PUTINT);
            ElaborateStdPrimRoutine(StandardEnvironment.PuteolDecl, Primitive.PUTEOL);
            ElaborateStdEqRoutine(StandardEnvironment.EqualDecl, Primitive.EQ);
        }

        static void WriteTableDetails(AbstractSyntaxTree ast)
        {
        }
    }
}