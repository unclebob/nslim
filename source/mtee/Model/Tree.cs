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
        public abstract T Value { get; }

        public abstract bool IsLeaf { get; }
        public abstract IList<Tree<T>> Branches { get; }

        public IEnumerable<T> Leaves {
            get {
                if (IsLeaf) yield return Value;
                else foreach (Tree<T> branch in Branches) foreach (T leaf in branch.Leaves) yield return leaf;
            }
        }

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
    }

    public class TreeList<T>: Tree<T> {
        private readonly List<Tree<T>> list = new List<Tree<T>>();
        private readonly T aValue;
        public override T Value { get { return aValue; } }

        public TreeList() {}

        public TreeList(T value) { aValue = value; }

        public override bool IsLeaf { get { return false; } }

        public override IList<Tree<T>> Branches { get { return list; } }

        public TreeList<T> AddBranchValue(object value) {
            Branches.Add(value as Tree<T> ?? new TreeLeaf<T>((T)value));
            return this;
        }

        public TreeList<T> AddBranch(Tree<T> value) {
            Branches.Add(value);
            return this;
        }
    }

    public class TreeLeaf<T>: Tree<T> {
        private readonly T aValue;
        public override T Value { get { return aValue; } }
        public TreeLeaf(T value) { aValue = value; }

        public override bool IsLeaf { get { return true; } }

        public override IList<Tree<T>> Branches { get { throw new InvalidOperationException(); } }
    }
}