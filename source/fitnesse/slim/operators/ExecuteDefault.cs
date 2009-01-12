// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;

namespace fitnesse.slim.operators {
    public class ExecuteDefault: ExecuteBase {
        public ExecuteDefault(): base(string.Empty) {}

        protected override Tree<object> ExecuteOperation(Processor processor, State state) {
            throw new ArgumentException(string.Format("Unrecognized operation '{0}'", state.ParameterString(1)));
        }
    }
}
