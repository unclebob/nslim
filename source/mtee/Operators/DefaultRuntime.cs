// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System.Collections.Generic;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;

namespace fitnesse.mtee.operators {
    public class DefaultRuntime<T>: RuntimeOperator<T> {
        public bool IsMatch(Processor<T> processor, State<T> state) { return true; }

        public object Create(Processor<T> processor, State<T> state) {
            var runtimeType = processor.ParseString<RuntimeType>(state.Member);
            if (state.ParameterCount == 0) return runtimeType.CreateInstance();
            RuntimeMember member = runtimeType.GetConstructor(state.ParameterCount);
            return member.Invoke(runtimeType.Type, GetParameterList(state.Parameters, processor, member)).Value;
        }

        public TypedValue Invoke(Processor<T> processor, State<T> state) {
            RuntimeMember member = new RuntimeType(state.Type).GetInstance(state.Member, state.ParameterCount);
            return member.Invoke(state.Instance, GetParameterList(state.Parameters, processor, member));
        }

        private static object[] GetParameterList(Tree<T> parameters, Processor<T> processor, RuntimeMember member) {
            var parameterList = new List<object>();
            int i = 0;
            foreach (Tree<T> parameter in parameters.Branches) {
                parameterList.Add(processor.ParseTree(member.GetParameterType(i), parameter));
                i++;
            }
            return parameterList.ToArray();
        }
    }
}