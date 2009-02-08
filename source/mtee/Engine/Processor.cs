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
            AddOperator((Operator<U>)Command.WithMember(operatorName).Create());
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

        public Command<U> Command { get { return new Command<U>(this); }}

        public bool Compare(Command<U> command) { return FindOperator<CompareOperator<U>>(command).Compare(command); }

        public Tree<U> Compose(Command<U> command) {
            if (command.Type == null) command.WithType(command.Instance != null ? command.Instance.GetType() : typeof (object));
            return FindOperator<ComposeOperator<U>>(command).Compose(command);
        }

        public object Create(Command<U> command) {
            if (command.Parameters == null) command.WithParameters(new TreeList<U>());
            return FindOperator<RuntimeOperator<U>>(command).Create(command);
        }

        public object Execute(Command<U> command) { return FindOperator<ExecuteOperator<U>>(command).Execute(command); }

        public TypedValue Invoke(Command<U> command) {
            if (command.Type == null) command.WithType(command.Instance.GetType());
            return FindOperator<RuntimeOperator<U>>(command).Invoke(command);
        }

        public object Parse(Command<U> command) {
            return FindOperator<ParseOperator<U>>(command).Parse(command);
        }

        public T ParseTree<T>(Tree<U> input) {
            return (T) Command.WithType(typeof (T)).WithParameters(input).Parse();
        }

        public T Parse<T>(U input) {
            return (T) Command.WithType(typeof (T)).WithValue(input).Parse();
        }

        public object ParseString(Type type, string input) {
            return Command
                .WithType(type)
                .WithParameters(Command
                    .WithInstance(input)
                    .WithType(typeof (string))
                    .Compose())
                .Parse();
        }

        public T ParseString<T>(string input) {
            return (T) ParseString(typeof (T), input);
        }

        private T FindOperator<T> (Command<U> command) where T: class, Operator<U>{
            for (int priority = operators.Count - 1; priority >= 0; priority--) {
                for (int i = operators[priority].Count - 1; i >= 0; i--) {
                    var candidate = operators[priority][i] as T;
                    if (candidate != null && candidate.IsMatch(command)) {
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