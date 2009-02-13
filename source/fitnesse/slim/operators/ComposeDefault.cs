// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;

namespace fitnesse.slim.operators {
    public class ComposeDefault: ComposeOperator<string> {
        private const string nullResult = "null";
        private const string voidResult = "/__VOID__/";

        public bool TryCompose(Processor<string> processor, Type type, object instance, ref Tree<string> result) {
            result = new TreeLeaf<string>(type == typeof(void) ? voidResult 
                : instance == null ? nullResult
                : instance.ToString());
            return true;
        }
    }
}
