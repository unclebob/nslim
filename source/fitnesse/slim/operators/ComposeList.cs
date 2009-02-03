// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System.Collections.Generic;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;

namespace fitnesse.slim.operators {
    public class ComposeList: ComposeOperator<string> { //todo: handle any enumerable type
        public bool IsMatch(Processor<string> processor, State<string> state) {
            return state.Type == typeof (List<object>);
        }

        public Tree<string> Compose(Processor<string> processor, State<string> state) {
            var list = state.Instance as List<object> ?? new List<object>();
            var tree = new TreeList<string>();
            foreach (object value in list) {
                tree.AddBranch(processor.Compose(value, value != null ? value.GetType() : typeof(object)));
            }
            return tree;
        }
    }
}
