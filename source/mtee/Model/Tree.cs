// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;

namespace fitnesse.mtee.model {
    public interface TreeWriter<T> {
        void WritePrefix(Tree<T> tree);
        void WriteBranch(Tree<T> tree, int index);
        void WriteSuffix(Tree<T> tree);
    }

    public abstract class Tree<T> {
        protected T value;
        public T Value { get { return value; } }

        public abstract bool IsLeaf { get; }
        public abstract IList<Tree<T>> Branches { get; }

        public TreeWriter<T> Serialize(TreeWriter<T> serializer) {
            serializer.WritePrefix(this);
            if (!IsLeaf) {
                for (int i = 0; i < Branches.Count; i++) {
                    serializer.WriteBranch(Branches[i], i);
                }
            }
            serializer.WriteSuffix(this);
            return serializer;
        }

        public string BranchString(int index) {
            return Branches[index].Value.ToString();
        }
    }

    public class TreeList<T>: Tree<T> {
        private readonly List<Tree<T>> list = new List<Tree<T>>();

        public TreeList() {}

        public TreeList(T value) { this.value = value; }

        public override bool IsLeaf { get { return false; } }

        public override IList<Tree<T>> Branches { get { return list; } }

        public TreeList<T> AddBranch(T value) {
            Branches.Add(value as Tree<T> ?? new TreeLeaf<T>(value));
            return this;
        }
    }

    public class TreeLeaf<T>: Tree<T> {
        public TreeLeaf(T value) { this.value = value; }

        public override bool IsLeaf { get { return true; } }

        public override IList<Tree<T>> Branches { get { throw new InvalidOperationException(); } }
    }
}