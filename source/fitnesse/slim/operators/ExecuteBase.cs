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

        protected ExecuteBase(string identifierName) {
            identifier = new IdentifierName(identifierName);
        }

        public bool IsMatch(Command<string> command) {
            return identifier.IsEmpty || (command.ParameterCount >= 2 && identifier.Matches(command.Parameter(1)));
        }

        public object Execute(Command<string> command) {
            try {
                return ExecuteOperation(command);
            }
            catch (Exception e) {
                return Result(command, string.Format(ExceptionResult, e));
            }
        }

        protected abstract Tree<string> ExecuteOperation(Command<string> command);

        protected static Tree<string> DefaultResult(Command<string> command) {
            return Result(command, defaultResult);
        }

        protected static Tree<string> Result(Command<string> command, Tree<string> result) {
            return new TreeList<string>()
                .AddBranchValue(command.Parameter(0))
                .AddBranch(result);
        }

        protected static Tree<string> Result(Command<string> command, string result) {
            return new TreeList<string>()
                .AddBranchValue(command.Parameter(0))
                .AddBranchValue(result);
        }

        protected static Tree<string> ParameterTree(Tree<string> input, int startingIndex) {
            var result = new TreeList<string>(input.Value);
            for (int i = startingIndex; i < input.Branches.Count; i++) {
                result.AddBranch(input.Branches[i]);
            }
            return result;
        }

        protected static TypedValue InvokeMember(Command<string> command, int memberIndex) {
            object target = command.Processor.Load(new SavedInstance(command.Parameter(memberIndex))).Instance;
            return command.Make
                .WithInstance(target)
                .WithMember(command.Parameter(memberIndex + 1))
                .WithParameters(ParameterTree(command.Parameters, memberIndex + 2))
                .Invoke();
        }
    }
}
