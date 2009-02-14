// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using fitnesse.mtee.model;

namespace fitnesse.mtee.engine {
    public interface Operator {}

    public interface CompareOperator<T>: Operator {
        bool TryCompare(Processor<T> processor, Type type, object instance, Tree<T> parameters, ref bool result);
    }

    public interface ComposeOperator<T>: Operator {
        bool TryCompose(Processor<T> processor, Type type, object instance, ref Tree<T> result);
    }

    public interface ExecuteOperator<T>: Operator {
        bool TryExecute(Processor<T> processor, object instance, Tree<T> parameters, ref object result);
    }

    public interface ParseOperator<T>: Operator {
        bool TryParse(Processor<T> processor, Type type, Tree<T> parameters, ref object result);
    }

    public interface RuntimeOperator<T>: Operator {
        bool TryCreate(Processor<T> processor, string memberName, Tree<T> parameters, ref object result);
        bool TryInvoke(Processor<T> processor, Type type, object instance, string memberName, Tree<T> parameters, ref TypedValue result);
    }
}