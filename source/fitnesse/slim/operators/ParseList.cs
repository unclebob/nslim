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
    public class ParseList: ParseOperator<string> { // todo: handle any IList type
        public bool IsMatch(Command<string> command) {
            return command.Type.IsGenericType && command.Type.GetGenericTypeDefinition() == typeof (List<>);
        }

        public object Parse(Command<string> command) {
            var result = (IList)Activator.CreateInstance(command.Type);
            foreach (Tree<string> branch in command.Parameters.Branches) {
                result.Add(command.Make
                    .WithType(command.Type.GetGenericArguments()[0])
                    .WithParameters(branch)
                    .Parse());
            }
            return result;
        }
    }
}
