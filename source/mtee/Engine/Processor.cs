// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Reflection;
using fitnesse.mtee.model;
using fitnesse.mtee.Model;
using fitnesse.mtee.operators;

namespace fitnesse.mtee.engine {
    public class Processor { //todo: add setup and teardown
        private readonly List<List<Operator>> operators = new List<List<Operator>>();
        public SystemUnderTest SystemUnderTest { get; set; }

        public Processor(SystemUnderTest systemUnderTest) {
            SystemUnderTest = systemUnderTest;
            AddOperator(new DefaultParse());
            AddOperator(new DefaultRuntime());
            AddOperator(new DefaultMemory());
        }

        public Processor(): this(new SystemUnderTest()) {}

        public void AddOperator(string operatorName) {
            AddOperator((Operator)Create(operatorName));
        }

        public void AddOperator(Operator anOperator) { AddOperator(anOperator, 0); }

        public void AddOperator(Operator anOperator, int priority) {
            while (operators.Count <= priority) operators.Add(new List<Operator>());
            operators[priority].Add(anOperator);
        }

        public void AddNamespace(string namespaceName) {
            SystemUnderTest.AddNamespace(namespaceName);
        }

        public object Execute(Tree<object> input) {
            var state = new State(input);
            return FindOperator<ExecuteOperator>(state).Execute(this, state);
        }

        public object Parse(Type type, Tree<object> input) {
            var state = new State(type, input);
            return FindOperator<ParseOperator>(state).Parse(this, state);
        }

        public object Compose(object result, Type type) {
            var state = new State(result, type);
            return FindOperator<ComposeOperator>(state).Compose(this, state);
        }

        public TypedValue Invoke(object instance, string member, Tree<object> parameters) {
            var state = new State(instance, instance.GetType(), member, parameters);
            return FindOperator<RuntimeOperator>(state).Invoke(this, state);
        }

        public object Create(string typeName, Tree<object> parameters) {
            var state = new State(typeName, parameters);
            return FindOperator<RuntimeOperator>(state).Create(this, state);
        }

        public object Create(string typeName) {
            return Create(typeName, new TreeList<object>());
        }

        public void Store(string variableName, object instance) {
            var state = new State(instance, variableName);
            FindOperator<MemoryOperator>(state).Store(this, state);
        }

        public object Load(string variableName) {
            var state = new State(variableName);
            return FindOperator<MemoryOperator>(state).Load(this, state);
        }

        public bool Contains(string variableName) {
            var state = new State(variableName);
            return FindOperator<MemoryOperator>(state).Contains(this, state);
        }

        private T FindOperator<T> (State state) where T: class, Operator{
            for (int priority = operators.Count - 1; priority >= 0; priority--) {
                for (int i = operators[priority].Count - 1; i >= 0; i--) {
                    var candidate = operators[priority][i] as T;
                    if (candidate != null && candidate.IsMatch(this, state)) {
                        return candidate;
                    }
                }
            }
            throw new ApplicationException(string.Format("No default for {0}", typeof(T).Name));
        }
    }
}