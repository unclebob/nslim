// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.model;

namespace fitnesse.mtee.engine {
    public interface Operator<T> {
        bool IsMatch(Command<T> command);
    }

    public interface ExecuteOperator<T>: Operator<T> {
        object Execute(Command<T> command);
    }

    public interface ParseOperator<T>: Operator<T> {
        object Parse(Command<T> command);
    }

    public interface ComposeOperator<T>: Operator<T> {
        Tree<T> Compose(Command<T> command);
    }

    public interface CompareOperator<T>: Operator<T> {
        bool Compare(Command<T> command);
    }

    public interface RuntimeOperator<T>: Operator<T> {
        object Create(Command<T> command);
        TypedValue Invoke(Command<T> command);
    }
}