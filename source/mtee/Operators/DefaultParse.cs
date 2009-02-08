// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using fitnesse.mtee.engine;

namespace fitnesse.mtee.operators {
    public class DefaultParse<T>: ParseOperator<T> { //todo: also look for constructor with string argument
        public bool IsMatch(Command<T> command) { return true; }

        public object Parse(Command<T> command) {
            if (command.Type.IsAssignableFrom(typeof(string)) /*&& !state.Type.Equals(typeof(DateTime))*/) {
                return command.ParameterValueString;
            }

            RuntimeMember parse = new RuntimeType(command.Type).FindStatic("parse", new [] {typeof(string)});
            if (parse != null && parse.ReturnType == command.Type) {
                return parse.Invoke(null, new object[] {command.ParameterValueString}).Value;
            }
            throw new InvalidOperationException(string.Format("Can't parse {0} because it doesn't have a static Parse method", command.Type.FullName));
        }
    }
}