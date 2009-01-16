// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System.Collections.Generic;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;
using fitnesse.mtee.Model;

namespace fitnesse.mtee.operators {
    public class DefaultRuntime: RuntimeOperator {
        public bool IsMatch(Processor processor, State state) { return true; }

        public object Create(Processor processor, State state) {
            RuntimeType runtimeType = processor.Assemblies.FindType(state.Member);
            RuntimeMember member = runtimeType.GetConstructor(state.ParameterCount);
            return member.Invoke(runtimeType.Type, GetParameterList(state.Parameters, processor, member));
        }

        public TypedValue Invoke(Processor processor, State state) {
            RuntimeMember member = new RuntimeType(state.Type).GetInstance(state.Member, state.ParameterCount);
            return new TypedValue(
                member.Invoke(state.Instance, GetParameterList(state.Parameters, processor, member)),
                member.ReturnType);
        }

        private static object[] GetParameterList(Tree<object> parameters, Processor processor, RuntimeMember member) {
            var parameterList = new List<object>();
            int i = 0;
            foreach (Tree<object> parameter in parameters.Branches) {
                parameterList.Add(processor.Parse(member.GetParameterType(i), parameter));
                i++;
            }
            return parameterList.ToArray();
        }
    }
}