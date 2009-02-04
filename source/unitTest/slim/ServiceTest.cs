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
                new TreeList<string>().AddBranchValue("step").AddBranchValue("make").AddBranchValue("variable").AddBranchValue(
                    "fitnesse.unitTest.slim.SampleClass");
            service.Execute(statement);
            Assert.AreEqual(1, SampleClass.Count);
        }

        [Test] public void OperatorIsAddedFromConfiguration() {
            var configuration = new Configuration();
            configuration.LoadXml("<config><fitnesse.slim.Service><addOperator>fitnesse.unitTest.slim.SampleOperator</addOperator></fitnesse.slim.Service></config>");
            var statement = new TreeList<string>().AddBranchValue("step").AddBranchValue("sampleCommand");
            var result = (Tree<string>)configuration.GetItem<Service>().Execute(statement);
            Assert.AreEqual("sampleResult", result.Branches[1].Value);
        }

        [Test] public void ParseSymbolIsDoneFirst() {
            service.Store(new Symbol("$symbol", "testvalue"));
            service.AddOperator(new ParseUpperCase());
            var value = service.Parse<string>("$symbol");
            Assert.AreEqual("TESTVALUE", value);
        }

        private class ParseUpperCase: ParseOperator<string> {
            public bool IsMatch(Processor<string> processor, State<string> state) { return true; }
            public object Parse(Processor<string> processor, State<string> state) { return state.ParameterValueString.ToUpper(); }
        }
    }

    public class SampleOperator: ExecuteBase {
        public SampleOperator() : base("sampleCommand") {}
        protected override Tree<string> ExecuteOperation(Processor<string> processor, State<string> state) {
            return Result(state, "sampleResult");
        }
    }
}
