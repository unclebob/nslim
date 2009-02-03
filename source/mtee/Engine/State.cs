// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using fitnesse.mtee.model;

namespace fitnesse.mtee.engine {
    public struct State<T> {
        public object Instance { get; private set; }
        public Type Type { get; private set; }
        public string  Member { get; private set; }
        public Tree<T> Parameters { get; private set; }

        public State(object instance, Type type, string member, Tree<T> parameters) : this() {
            Instance = instance;
            Type = type;
            Member = member;
            Parameters = parameters;
        }

        public static State<T> MakeExecute(Tree<T> parameters) { return new State<T>(null, typeof (void), string.Empty, parameters); }

        public static State<T> MakeParseTree(Type type, Tree<T> parameters) { return new State<T>(null, type, string.Empty, parameters); }
        public static State<T> MakeParseValue(Type type, T parameter) { return MakeParseTree(type, new TreeLeaf<T>(parameter)); }
        public static State<T> MakeParseString(Type type, string member) { return new State<T>(null, type, member, null); }

        public static State<T> MakeCreate(string member, Tree<T> parameters) { return new State<T>(null, typeof (void), member, parameters); }

        public static State<T> MakeInvoke(object instance, string member, Tree<T> parameters) { return new State<T>(instance, instance.GetType(), member, parameters); }

        public static State<T> MakeCompose(object instance, Type type) { return new State<T>(instance, type, string.Empty, null); }

        public static State<T> MakeCompare(object instance, Type type, Tree<T> parameters) { return new State<T>(instance, type, string.Empty, parameters); }

        public string ParameterString(int index) {
            return Parameters.BranchString(index);
        }

        public int ParameterCount { get { return Parameters == null || Parameters.IsLeaf ? 0 : Parameters.Branches.Count; }}

        public T ParameterValue { get { return Parameters.Value; }}

        //todo: check this warning
        public string ParameterValueString { get { return Parameters == null || Parameters.Value == null ? Member : Parameters.Value.ToString(); }}
    }
}