// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;

namespace fitnesse.slim.operators {
    public class ComposeBoolean: ComposeOperator<string> {
        public bool TryCompose(Processor<string> processor, Type type, object instance, ref Tree<string> result) {
            if (type != typeof (bool)) return false;
            result = new TreeLeaf<string>((bool)instance ? "true" : "false");
            return true;
        }
    }
}
