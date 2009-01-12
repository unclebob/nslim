// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System.Collections.Generic;
using fitnesse.mtee.engine;

namespace fitnesse.mtee.operators {
    public class DefaultMemory: MemoryOperator {
        private readonly Dictionary<string, object> memory = new Dictionary<string, object>();

        public bool IsMatch(Processor processor, State state) { return true; }

        public object Load(Processor processor, State state) { return memory[state.Member]; }

        public object Store(Processor processor, State state) {
            memory[state.Member] = state.Instance;
            return state.Instance;
        }

        public bool Contains(Processor processor, State state) {
            return memory.ContainsKey(state.Member);
        }
    }
}