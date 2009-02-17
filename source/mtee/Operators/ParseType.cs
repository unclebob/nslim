// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;

namespace fitnesse.mtee.operators {
    public class ParseType<T>: ParseOperator<T> {
        public bool TryParse(Processor<T> processor, Type type, object instance, Tree<T> parameters, ref object result) {
            if (type != typeof (RuntimeType)) return false;
            result = processor.ApplicationUnderTest.FindType(new IdentifierName(parameters.Value.ToString()));
            return true;
        }
    }
}
