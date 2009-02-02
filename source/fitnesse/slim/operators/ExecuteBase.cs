// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;

namespace fitnesse.slim.operators {
    public abstract class ExecuteBase: ExecuteOperator {
        private const string defaultResult = "OK";
        private const string ExceptionResult = "__EXCEPTION__:{0}";
        private readonly IdentifierName identifier;

        protected ExecuteBase(string identifierName) {
            identifier = new IdentifierName(identifierName);
        }

        public bool IsMatch(Processor processor, State state) {
            return identifier.IsEmpty || (state.ParameterCount >= 2 && identifier.Matches(state.ParameterString(1)));
        }

        public object Execute(Processor processor, State state) {
            try {
                return ExecuteOperation(processor, state);
            }
            catch (Exception e) {
                return new TreeList<object>()
                    .AddBranch(state.ParameterString(0))
                    .AddBranch(string.Format(ExceptionResult, e));
            }
        }

        protected abstract Tree<object> ExecuteOperation(Processor processor, State state);

        protected static Tree<object> DefaultResult(State state) {
            return Result(state, defaultResult);
        }

        protected static Tree<object> Result(State state, object result) {
            return new TreeList<object>()
                .AddBranch(state.ParameterString(0))
                .AddBranch(result);
        }

        protected static Tree<object> ParameterTree(Tree<object> input, int startingIndex) {
            var result = new TreeList<object>(input.Value);
            for (int i = startingIndex; i < input.Branches.Count; i++) {
                result.Branches.Add(input.Branches[i]);
            }
            return result;
        }

        protected static TypedValue InvokeMember(Processor processor, State state, int memberIndex) {
            object target = processor.Load(new SavedInstance(state.ParameterString(memberIndex))).Instance;
            return processor.Invoke(target, state.ParameterString(memberIndex + 1), ParameterTree(state.Parameters, memberIndex + 2));
        }
    }
}
