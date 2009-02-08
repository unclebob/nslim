// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.engine;
using fitnesse.mtee.model;

namespace fitnesse.slim.operators {
    public class ComposeDefault: ComposeOperator<string> {
        private const string nullResult = "null";
        private const string voidResult = "/__VOID__/";

        public bool IsMatch(Command<string> command) {
            return true;
        }

        public Tree<string> Compose(Command<string> command) {
            return new TreeLeaf<string>(command.Type == typeof(void) ? voidResult 
                : command.Instance == null ? nullResult
                : command.Instance.ToString());
        }
    }
}
