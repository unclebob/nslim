// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using fitnesse.mtee.model;

namespace fitnesse.mtee.engine {
    public class Command<T> {
        public object Instance { get; private set; }
        public Type Type { get; private set; }
        public string  Member { get; private set; }
        public Tree<T> Parameters { get; private set; }
        public Processor<T> Processor { get; private set; }

        public Command(Processor<T> processor) {
            Processor = processor;
        }

        public Command<T> Make { get { return new Command<T>(Processor); }}

        public Command<T> WithParameters(Tree<T> parameters) {
            Parameters = parameters;
            return this;
        }

        public Command<T> WithValue(T value) {
            Parameters = new TreeLeaf<T>(value);
            return this;
        }

        public Command<T> WithInstance(object instance) {
            Instance = instance;
            return this;
        }

        public Command<T> WithType(Type type) {
            Type = type;
            return this;
        }

        public Command<T> WithMember(string member) {
            Member = member;
            return this;
        }

        public object Create() { return Processor.Create(this); }
        public Tree<T> Compose() { return Processor.Compose(this); }
        public bool Compare() { return Processor.Compare(this); }
        public object Execute() { return Processor.Execute(this); }
        public object Parse() { return Processor.Parse(this); }
        public TypedValue Invoke() { return Processor.Invoke(this); }

        public T Parameter(int index) { return Parameters.Branches[index].Value; }
        public int ParameterCount { get { return Parameters == null || Parameters.IsLeaf ? 0 : Parameters.Branches.Count; }}
        public T ParameterValue { get { return Parameters.Value; }}
        public string ParameterValueString { get { return Parameters.Value.ToString(); }}
    }
}