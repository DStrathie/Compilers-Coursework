using System.Collections.Generic;
using Triangle.Compiler.SyntaxTrees.Declarations;
using Triangle.Compiler.SyntaxTrees.Terminals;

namespace Triangle.Compiler.ContextualAnalyzer
{
    public sealed class IdentificationTable
    {
        private Stack<Dictionary<string, Declaration>> scopes;

        public IdentificationTable()
        {
            //throw new System.NotImplementedException();
            scopes = new Stack<Dictionary<string, Declaration>>();
            scopes.Push(new Dictionary<string, Declaration>());
        }

        // Opens a new level in the identification table, 1 higher than the
        // current topmost level.
        public void OpenScope()
        {
            //throw new System.NotImplementedException();
            scopes.Push(new Dictionary<string, Declaration>());
        }

        // Closes the topmost level in the identification table, discarding
        // all entries belonging to that level.
        public void CloseScope()
        {
            //throw new System.NotImplementedException();
            scopes.Pop();
        }

        /**
         * Makes a new entry in the identification table for the given terminal and
         * attribute. The new entry belongs to the current level. duplicated is set to
         * to true iff there is already an entry for the same identifier at the
         * current level.
         * 
         * @param terminal
         *          the terminal symbol
         * @param attr
         *          the attribute
         */
        public void Enter(Terminal terminal, Declaration attr)
        {
            if (scopes.Peek().ContainsKey(terminal.Spelling)){
                attr.Duplicated = true;
            }else{
                attr.Duplicated = false;
                scopes.Peek().Add(terminal.Spelling, attr);
            }
            

        }

        /**
         * Finds an entry for the given identifier in the identification table, if
         * any. If there are several entries for that identifier, finds the entry at
         * the highest level, in accordance with the scope rules. Returns null iff no
         * entry is found, otherwise returns the attribute field of the entry found.
         * 
         * @param id
         *          the identifier of the declaration to retrieve
         * @return the matching declaration or null if none exists
         */
        public Declaration Retrieve(string id)
        {
            foreach (Dictionary<string, Declaration> scope in scopes)
            {
                if (scope.TryGetValue(id, out Declaration attr))
                {
                    return attr;
                }
            }
            // Wasn't declared anywhere
            return null;
        }
    }
}