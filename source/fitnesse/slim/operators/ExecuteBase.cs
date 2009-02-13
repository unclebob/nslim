// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;

namespace fitnesse.slim.operators {
    public abstract class ExecuteBase: ExecuteOperator<string> {
        private const string defaultResult = "OK";
        private const string ExceptionResult = "__EXCEPTION__:{0}";
        private readonly IdentifierName identifier;

        public bool TryExecute(Processor<string> processor, Tree<string> parameters, ref object result) {
            if (!identifier.IsEmpty && (parameters.Branches.Count < 2 || !identifier.Matches(parameters.Branches[1].Value))) return false;
            try {
                result = ExecuteOperation(processor, parameters);
            }
            catch (Exception e) {
                result = Result(parameters, string.Format(ExceptionResult, e));
            }
            return true;
        }

        protected ExecuteBase(string identifierName) {
            identifier = new IdentifierName(identifierName);
        }

        protected abstract Tree<string> ExecuteOperation(Processor<string> processor, Tree<string> parameters);

        protected static Tree<string> DefaultResult(Tree<string> parameters) {
            return Result(parameters, defaultResult);
        }

        protected static Tree<string> Result(Tree<string> parameters, Tree<string> result) {
            return new TreeList<string>()
                .AddBranchValue(parameters.Branches[0].Value)
                .AddBranch(result);
        }

        protected static Tree<string> Result(Tree<string> parameters, string result) {
            return new TreeList<string>()
                .AddBranchValue(parameters.Branches[0].Value)
                .AddBranchValue(result);
        }

        protected static Tree<string> ParameterTree(Tree<string> input, int startingIndex) {
            var result = new TreeList<string>(input.Value);
            for (int i = startingIndex; i < input.Branches.Count; i++) {
                result.AddBranch(input.Branches[i]);
            }
            return result;
        }

        protected static TypedValue InvokeMember(Processor<string> processor, Tree<string> parameters, int memberIndex) {
            object target = processor.Load(new SavedInstance(parameters.Branches[memberIndex].Value)).Instance;
            return processor.Invoke(target, parameters.Branches[memberIndex + 1].Value, ParameterTree(parameters, memberIndex + 2));
        }
    }
}
