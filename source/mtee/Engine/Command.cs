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
        public Type Operator { get; private set; }

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

        public bool Compare() {
            Operator = typeof (CompareOperator<T>);
            return Processor.FindOperator<CompareOperator<T>>(this).Compare(this);
        }

        public Tree<T> Compose() {
            if (Type == null) Type = Instance != null ? Instance.GetType() : typeof (object);
            Operator = typeof (ComposeOperator<T>);
            return Processor.FindOperator<ComposeOperator<T>>(this).Compose(this);
        }

        public object Create() {
            if (Parameters == null) Parameters = new TreeList<T>();
            Operator = typeof (RuntimeOperator<T>);
            return Processor.FindOperator<RuntimeOperator<T>>(this).Create(this);
        }

        public object Execute() {
            Operator = typeof (ExecuteOperator<T>);
            return Processor.FindOperator<ExecuteOperator<T>>(this).Execute(this);
        }

        public TypedValue Invoke() {
            if (Type == null) Type = Instance.GetType();
            Operator = typeof (RuntimeOperator<T>);
            return Processor.FindOperator<RuntimeOperator<T>>(this).Invoke(this);
        }

        public object Parse() {
            Operator = typeof (ParseOperator<T>);
            return Processor.FindOperator<ParseOperator<T>>(this).Parse(this);
        }

        public T Parameter(int index) { return Parameters.Branches[index].Value; }
        public int ParameterCount { get { return Parameters == null || Parameters.IsLeaf ? 0 : Parameters.Branches.Count; }}
        public T ParameterValue { get { return Parameters.Value; }}
        public string ParameterValueString { get { return Parameters.Value.ToString(); }}
    }
}