// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.model;

namespace fitnesse.mtee.engine {
    public interface Operator {
        bool IsMatch(Processor processor, State state);
    }

    public interface ExecuteOperator: Operator {
        object Execute(Processor processor, State state);
    }

    public interface ParseOperator: Operator {
        object Parse(Processor processor, State state);
    }

    public interface ComposeOperator: Operator {
        object Compose(Processor processor, State state);
    }

    public interface RuntimeOperator: Operator {
        object Create(Processor processor, State state);
        TypedValue Invoke(Processor processor, State state);
    }

    public interface MemoryOperator: Operator {
        object Load(Processor processor, State state);
        object Store(Processor processor, State state);
        bool Contains(Processor processor, State state);
    }
}