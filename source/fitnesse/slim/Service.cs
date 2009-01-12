// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.application;
using fitnesse.mtee.engine;
using fitnesse.slim.operators;

namespace fitnesse.slim {
    public class Service: Processor {
        public static Service Instance { get { return Context.Instance.GetItem<Service>(); } }

        public Service() {
            Add(new ExecuteDefault());
            Add(new ExecuteImport());
            Add(new ExecuteMake());
            Add(new ExecuteCall());
            Add(new ExecuteCallAndAssign());
            Add(new ParseList());
            Add(new ParseSymbol(), 1);
            Add(new ComposeDefault());
            Add(new ComposeBoolean());
            Add(new ComposeList());
        }

        public void AddOperator(string operatorName) {
            Add((Operator)Create(operatorName));
        }
    }
}
