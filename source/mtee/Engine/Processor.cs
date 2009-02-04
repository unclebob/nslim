// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using fitnesse.mtee.model;
using fitnesse.mtee.operators;

namespace fitnesse.mtee.engine {

    public class Processor<U>: Copyable { //todo: add setup and teardown
        //todo: this is turning into a facade so push everything else out
        private readonly List<List<Operator<U>>> operators = new List<List<Operator<U>>>();

        private readonly Dictionary<Type, object> memoryBanks = new Dictionary<Type, object>();

        public ApplicationUnderTest ApplicationUnderTest { get; set; }

        public Processor(ApplicationUnderTest applicationUnderTest) {
            ApplicationUnderTest = applicationUnderTest;
            AddOperator(new DefaultParse<U>());
            AddOperator(new ParseType<U>());
            AddOperator(new DefaultRuntime<U>());
        }

        public Processor(): this(new ApplicationUnderTest()) {}

        public Processor(Processor<U> other): this(new ApplicationUnderTest(other.ApplicationUnderTest)) {
            operators.Clear();
            foreach (List<Operator<U>> list in other.operators) {
                operators.Add(new List<Operator<U>>(list));
            }
            memoryBanks = new Dictionary<Type, object>(other.memoryBanks);
        }

        public void AddOperator(string operatorName) {
            AddOperator((Operator<U>)Create(operatorName));
        }

        public void AddOperator(Operator<U> anOperator) { AddOperator(anOperator, 0); }

        public void AddOperator(Operator<U> anOperator, int priority) {
            while (operators.Count <= priority) operators.Add(new List<Operator<U>>());
            operators[priority].Add(anOperator);
        }

        public void RemoveOperator(string operatorName) {
            foreach (List<Operator<U>> list in operators)
                foreach (Operator<U> item in list)
                    if (item.GetType().FullName == operatorName) {
                        list.Remove(item);
                        return;
                    }
        }

        public void AddNamespace(string namespaceName) {
            ApplicationUnderTest.AddNamespace(namespaceName);
        }

        public object Execute(Tree<U> input) {
            var state = State<U>.MakeExecute(input);
            return FindOperator<ExecuteOperator<U>>(state).Execute(this, state);
        }

        public object ParseTree(Type type, Tree<U> input) {
            var state = State<U>.MakeParseTree(type, input);
            return FindOperator<ParseOperator<U>>(state).Parse(this, state);
        }

        public object Parse(Type type, U input) {
            var state = State<U>.MakeParseValue(type, input);
            return FindOperator<ParseOperator<U>>(state).Parse(this, state);
        }

        public T ParseTree<T>(Tree<U> input) {
            return (T) ParseTree(typeof (T), input);
        }

        public T Parse<T>(U input) {
            return (T) Parse(typeof (T), input);
        }

        public object ParseString(Type type, string input) {
            return ParseTree(type, Compose(input, typeof (string)));
        }

        public T ParseString<T>(string input) {
            return (T) ParseString(typeof (T), input);
        }

        public Tree<U> Compose(object result, Type type) {
            var state = State<U>.MakeCompose(result, type);
            return FindOperator<ComposeOperator<U>>(state).Compose(this, state);
        }

        public TypedValue Invoke(object instance, string member, Tree<U> parameters) {
            var state = State<U>.MakeInvoke(instance, member, parameters);
            return FindOperator<RuntimeOperator<U>>(state).Invoke(this, state);
        }

        public object Create(string typeName, Tree<U> parameters) {
            var state = State<U>.MakeCreate(typeName, parameters);
            return FindOperator<RuntimeOperator<U>>(state).Create(this, state);
        }

        public object Create(string typeName) {
            return Create(typeName, new TreeList<U>());
        }

        public bool Compare(object instance, Type type, Tree<U> parameters) {
            var state = State<U>.MakeCompare(instance, type, parameters);
            return FindOperator<CompareOperator<U>>(state).Compare(this, state);
        }

        private T FindOperator<T> (State<U> state) where T: class, Operator<U>{
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

        Copyable Copyable.Copy() {
            return new Processor<U>(this);
        }

        public void AddMemory<T>() {
            memoryBanks[typeof (T)] = new List<T>();
        }

        public void Store<T>(T newItem) {
            List<T> memory = GetMemory<T>();
            foreach (T item in memory) {
                if (!newItem.Equals(item)) continue;
                memory.Remove(item);
                break;
            }
            memory.Add(newItem);
        }

        public T Load<T>(T matchItem) {
            foreach (T item in GetMemory<T>()) {
                if (matchItem.Equals(item)) return item;
            }
            throw new KeyNotFoundException();
        }

        public bool Contains<T>(T matchItem) {
            foreach (T item in GetMemory<T>()) {
                if (matchItem.Equals(item)) return true;
            }
            return false;
        }

        public void Clear<T>() {
            GetMemory<T>().Clear();
        }

        private List<T> GetMemory<T>() { return (List<T>) memoryBanks[typeof (T)];}
    }
}