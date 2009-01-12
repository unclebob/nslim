// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using fitnesse.mtee.model;

namespace fitnesse.mtee.engine {
    public struct State {
        public object Instance { get; private set; }
        public Type Type { get; private set; }
        public string Member { get; private set; }
        public Tree<object> Parameters { get; private set; }

        public State(object instance, Type type, string member, Tree<object> parameters) : this() {
            Instance = instance;
            Type = type;
            Member = member;
            Parameters = parameters;
        }

        public State(Tree<object> parameters): this(typeof(void), parameters) {}
        public State(Type type, Tree<object> parameters): this(null, type, string.Empty, parameters) {}
        public State(object instance, Type type) : this(instance, type, string.Empty, null) {}
        public State(string member, Tree<object> parameters) : this(null, typeof(void), member, parameters) {}
        public State(object instance, string member) : this(instance, typeof(void), member, null) {}
        public State(string member) : this(null, member) {}

        public string ParameterString(int index) {
            return Parameters.BranchString(index);
        }

        public int ParameterCount { get { return Parameters == null || Parameters.IsLeaf ? 0 : Parameters.Branches.Count; }}

        public object ParameterValue { get { return Parameters.Value; }}

        public string ParameterValueString { get { return Parameters == null || Parameters.Value == null ? string.Empty : Parameters.Value.ToString(); }}
    }
}