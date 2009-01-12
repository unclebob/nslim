// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Reflection;
using fitnesse.mtee.model;
using fitnesse.mtee.operators;

namespace fitnesse.mtee.engine {
    public class Processor { //todo: add setup and teardown
        private readonly List<List<Operator>> operators = new List<List<Operator>>();
        public Assemblies Assemblies { get; private set;}

        public Processor(Assemblies assemblies) {
            Assemblies = assemblies;
            Add(new DefaultParse());
            Add(new DefaultRuntime());
            Add(new DefaultMemory());
        }

        public Processor(): this(Assemblies.Instance) {}

        public void Add(Operator anOperator) { Add(anOperator, 0); }

        public void Add(Operator anOperator, int priority) {
            while (operators.Count <= priority) operators.Add(new List<Operator>());
            operators[priority].Add(anOperator);
        }

        public object Execute(Tree<object> input) {
            return Invoke("execute", new State(input));
        }

        public object Parse(Type type, Tree<object> input) {
            return Invoke("parse", new State(type, input));
        }

        public object Compose(object result) {
            return Invoke("compose", new State(result, result != null ? result.GetType() : typeof(object)));
        }

        public object Invoke(object instance, string member, Tree<object> parameters) {
            return Invoke("invoke", new State(instance, instance.GetType(), member, parameters));
        }

        public object Create(string typeName, Tree<object> parameters) {
            return Invoke("create", new State(typeName, parameters));
        }

        public object Create(string typeName) {
            return Create(typeName, new TreeList<object>());
        }

        public void Store(string variableName, object instance) {
            Invoke("store", new State(instance, variableName));
        }

        public object Load(string variableName) {
            return Invoke("load", new State(variableName));
        }

        public bool Contains(string variableName) {
            return (bool)Invoke("contains", new State(variableName));
        }

        private object Invoke(string operation, State state) {
            for (int priority = operators.Count - 1; priority >= 0; priority--) {
                for (int i = operators[priority].Count - 1; i >= 0; i--) {
                    Operator candidate = operators[priority][i];
                    RuntimeMember runtimeMember = new RuntimeType(candidate.GetType()).FindInstance(operation, 2);
                    if (runtimeMember != null && candidate.IsMatch(this, state)) {
                        return TryInvoke(runtimeMember, candidate, new object[] {this, state});
                    }
                }
            }
            throw new ApplicationException(string.Format("No default for {0}", operation));
        }

        public object TryInvoke(RuntimeMember runtimeMember, object instance, object[] parameters) {
            try {
                return runtimeMember.Invoke(instance, parameters);
            }
            catch (TargetInvocationException e) {
                throw e.InnerException;
            }
        }
    }
}