// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System.Collections.Generic;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;

namespace fitnesse.mtee.operators {
    public class DefaultRuntime<T>: RuntimeOperator<T> {
        public bool IsMatch(Command<T> command) { return true; }

        public object Create(Command<T> command) {
            var runtimeType = command.Processor.ParseString<RuntimeType>(command.Member);
            if (command.ParameterCount == 0) return runtimeType.CreateInstance();
            RuntimeMember member = runtimeType.GetConstructor(command.ParameterCount);
            return member.Invoke(runtimeType.Type, GetParameterList(command.Parameters, command, member)).Value;
        }

        public TypedValue Invoke(Command<T> command) {
            RuntimeMember member = new RuntimeType(command.Type).GetInstance(command.Member, command.ParameterCount);
            return member.Invoke(command.Instance, GetParameterList(command.Parameters, command, member));
        }

        private static object[] GetParameterList(Tree<T> parameters, Command<T> command, RuntimeMember member) {
            var parameterList = new List<object>();
            int i = 0;
            foreach (Tree<T> parameter in parameters.Branches) {
                parameterList.Add(command.Make
                    .WithType(member.GetParameterType(i))
                    .WithParameters(parameter)
                    .Parse());
                i++;
            }
            return parameterList.ToArray();
        }
    }
}