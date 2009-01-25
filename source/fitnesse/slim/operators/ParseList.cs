// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;

namespace fitnesse.slim.operators {
    public class ParseList: ParseOperator { // todo: handle any IList type
        public bool IsMatch(Processor processor, State state) {
            return state.Type.IsGenericType && state.Type.GetGenericTypeDefinition() == typeof (List<>);
        }

        public object Parse(Processor processor, State state) {
            var result = (IList)Activator.CreateInstance(state.Type);
            foreach (Tree<object> branch in state.Parameters.Branches) {
                result.Add(processor.ParseTree(state.Type.GetGenericArguments()[0], branch));
            }
            return result;
        }
    }
}
