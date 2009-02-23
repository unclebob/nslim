// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using fitnesse.mtee.engine;
using fitnesse.mtee.exception;
using fitnesse.mtee.model;

namespace fitnesse.slim.operators {
    public class ComposeException: ComposeOperator<string> {
        private const string ExceptionResult = "__EXCEPTION__:{0}";

        public bool TryCompose(Processor<string> processor, TypedValue instance, ref Tree<string> result) {
            if (!typeof(Exception).IsAssignableFrom(instance.Type)) return false;

            if (TryResult<MemberException>(instance,
                e => string.Format("NO_METHOD_IN_CLASS {0} {1}", e.MemberName, e.Type), ref result)) return true;

            if (TryResult<ConstructorException>(instance,
                e => string.Format("NO_CONSTRUCTOR {0}", e.Type), ref result)) return true;

            result = MakeResult(instance.Value.ToString());
            return true;
        }

        private delegate string Format<T>(T exception);

        private static bool TryResult<T>(TypedValue exception, Format<T> formatter, ref Tree<string> result) where T: class {
            var candidateException = exception.Value as T;
            if (candidateException == null) return false;
            result = MakeResult(string.Format("message<<{0}>> {1}", formatter(candidateException), candidateException));
            return true;
        }

        private static Tree<string> MakeResult(string message) {
            return new TreeLeaf<string>(string.Format(ExceptionResult, message));
        }
    }
}
