// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.engine;
using fitnesse.mtee.model;
using NUnit.Framework;

namespace fitnesse.unitTest.engine {
    [TestFixture] public class InterpreterTest {
        private Processor processor;

        [SetUp] public void SetUp() {
            processor = new Processor();
        }
        [Test] public void ExecuteUsesOperators() {
            processor.AddOperator(new TestOperator());
            var result = (Tree<object>)processor.Execute(new TreeLeaf<object>("my input"));
            Assert.AreEqual("my input test", result.Value);
        }

        [Test] public void InstanceIsStoredAndLoaded() {
            processor.Store("testname", "testvalue");
            Assert.AreEqual("testvalue", processor.Load("testname"));
        }

        private class TestOperator: ExecuteOperator {
            public bool IsMatch(Processor processor, State state) { return true; }

            public object Execute(Processor processor, State state) {
                return new TreeLeaf<object>(string.Format("{0} test", state.ParameterValue));
            }
        }

    }
}