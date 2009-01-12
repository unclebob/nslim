// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.application;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;
using fitnesse.slim;
using fitnesse.slim.operators;
using NUnit.Framework;

namespace fitnesse.unitTest.slim {
    [TestFixture] public class ServiceTest {
        private Service service;

        [SetUp] public void SetUp() {
            service = new Service();
        }

        [Test] public void InstanceIsCreated() {
            SampleClass.Count = 0;
            var statement =
                new TreeList<object>().AddBranch("step").AddBranch("make").AddBranch("variable").AddBranch(
                    "fitnesse.unitTest.slim.SampleClass");
            service.Execute(statement);
            Assert.AreEqual(1, SampleClass.Count);
        }

        [Test] public void OperatorIsAddedFromConfiguration() {
            new Configuration().LoadXml("<config><fitnesse.slim.Service><addOperator>fitnesse.unitTest.slim.SampleOperator</addOperator></fitnesse.slim.Service></config>");
            var statement = new TreeList<object>().AddBranch("step").AddBranch("sampleCommand");
            var result = (Tree<object>)Service.Instance.Execute(statement);
            Assert.AreEqual("sampleResult", result.BranchString(1));
        }

        [Test] public void ParseSymbolIsDoneFirst() {
            service.Store("$symbol", "testvalue");
            service.Add(new ParseUpperCase());
            object value = service.Parse(typeof(string), new TreeLeaf<object>("$symbol"));
            Assert.AreEqual("TESTVALUE", value.ToString());
        }

        private class ParseUpperCase: ParseOperator {
            public bool IsMatch(Processor processor, State state) { return true; }
            public object Parse(Processor processor, State state) { return state.ParameterValueString.ToUpper(); }
        }
    }

    public class SampleOperator: ExecuteBase {
        public SampleOperator() : base("sampleCommand") {}
        protected override Tree<object> ExecuteOperation(Processor processor, State state) {
            return Result(state, "sampleResult");
        }
    }
}
