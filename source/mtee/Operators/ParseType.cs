// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.engine;
using fitnesse.mtee.model;

namespace fitnesse.mtee.operators {
    class ParseType: ParseOperator {
        public bool IsMatch(Processor processor, State state) {
            return state.Type == typeof (RuntimeType);
        }

        public object Parse(Processor processor, State state) {
            return processor.ApplicationUnderTest.FindType(new IdentifierName(state.ParameterValueString));
        }
    }
}
