// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.engine;
using fitnesse.mtee.model;
using fitnesse.slim.operators;

namespace fitnesse.slim {
    public class Service: Processor<string> {

        public Service() {
            AddMemory<SavedInstance>();
            AddMemory<Symbol>();
            AddOperator(new ExecuteDefault());
            AddOperator(new ExecuteImport());
            AddOperator(new ExecuteMake());
            AddOperator(new ExecuteCall());
            AddOperator(new ExecuteCallAndAssign());
            AddOperator(new ParseList());
            AddOperator(new ParseSymbol(), 1);
            AddOperator(new ComposeDefault());
            AddOperator(new ComposeBoolean());
            AddOperator(new ComposeList());
        }

    }

    public class SavedInstance: KeyValueMemory<string, object> {
        public SavedInstance(string id, object instance): base(id, instance) {}
        public SavedInstance(string id): base(id) {}
    }

    public class Symbol: KeyValueMemory<string, object> {
        public Symbol(string id, object instance): base(id, instance) {}
        public Symbol(string id): base(id) {}
    }
}
