// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.engine;
using fitnesse.mtee.model;
using fitnesse.slim.operators;
using NUnit.Framework;

namespace fitnesse.unitTest.slim {
    [TestFixture] public class ExecuteOperatorsTest {
        private Processor processor;
        private Tree<object> result;

        [SetUp] public void SetUp() {
            processor = new Processor();
            processor.Add(new ComposeDefault());
        }

        [Test] public void ExecuteDefaultReturnsException() {
            var executeDefault = new ExecuteDefault();
            var input = new TreeList<object>().AddBranch("step").AddBranch("garbage");
            ExecuteOperation(executeDefault, input, 2);
            CheckForException("System.ArgumentException: Unrecognized operation 'garbage'");
        }

        [Test] public void ExecuteMakeBadClassReturnsException() {
            var executeMake = new ExecuteMake();
            var input = new TreeList<object>().AddBranch("step").AddBranch("make").AddBranch("variable").AddBranch("garbage");
            ExecuteOperation(executeMake, input, 2);
            CheckForException("System.ArgumentException: Type 'garbage' not found");
        }

        [Test] public void ExecuteImportAddsNamespace() {
            var executeImport = new ExecuteImport();
            var input = new TreeList<object>().AddBranch("step").AddBranch("import").AddBranch("space.name");
            ExecuteOperation(executeImport, input, 2);
            Assert.IsTrue(Namespaces.Instance.IsRegistered("space.name"));
        }

        [Test] public void ExecuteCallAndAssignSavesSymbol() {
            processor.Store("variable", new SampleClass());
            var executeCallAndAssign = new ExecuteCallAndAssign();
            var input =
                new TreeList<object>().AddBranch("step").AddBranch("callAndAssign").AddBranch("symbol").AddBranch(
                    "variable").AddBranch("sampleMethod");
            ExecuteOperation(executeCallAndAssign, input, 2);
            Assert.AreEqual("testresult", result.Branches[1].Value);
            Assert.AreEqual("testresult", processor.Load("$symbol"));
        }

        private void ExecuteOperation(ExecuteOperator executeOperator, Tree<object> input, int branchCount) {
            result = (Tree<object>)executeOperator.Execute(processor, new State(null, typeof(void), null, input));
            Assert.IsFalse(result.IsLeaf);
            Assert.AreEqual(branchCount, result.Branches.Count);
            Assert.AreEqual("step", result.Branches[0].Value);
        }

        private void CheckForException(string exceptionText) {
            Assert.IsTrue(result.Branches[1].Value.ToString().StartsWith("__EXCEPTION__:" + exceptionText));
        }
    }
}
