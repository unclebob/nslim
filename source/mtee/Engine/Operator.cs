// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.model;

namespace fitnesse.mtee.engine {
    public interface Operator<T> {
        bool IsMatch(Processor<T> processor, State<T> state);
    }

    public interface ExecuteOperator<T>: Operator<T> {
        object Execute(Processor<T> processor, State<T> state);
    }

    public interface ParseOperator<T>: Operator<T> {
        object Parse(Processor<T> processor, State<T> state);
    }

    public interface ComposeOperator<T>: Operator<T> {
        Tree<T> Compose(Processor<T> processor, State<T> state);
    }

    public interface CompareOperator<T>: Operator<T> {
        bool Compare(Processor<T> processor, State<T> state);
    }

    public interface RuntimeOperator<T>: Operator<T> {
        object Create(Processor<T> processor, State<T> state);
        TypedValue Invoke(Processor<T> processor, State<T> state);
    }
}