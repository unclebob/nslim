// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;
using NUnit.Framework;

namespace fitnesse.unitTest.engine {
    [TestFixture] public class ProcessorTest {

        private static readonly DefaultTest defaultTest = new DefaultTest();
        private static readonly SpecificTest specificTestA = new SpecificTest("A");
        private static readonly SpecificTest specificTestB = new SpecificTest("B");

        private BasicProcessor processor;

        [SetUp] public void SetUp() {
            processor = new BasicProcessor();
        }

        [Test] public void NoOperatorIsFound() {
            try {
                processor.Execute(new TreeList<string>());
                Assert.Fail();
            }
            catch (ApplicationException) {
                Assert.IsTrue(true);
            }
        }

        [Test] public void DefaultOperatorIsFound() {
            processor.AddOperator(defaultTest);
            object result = processor.Execute(new TreeList<string>());
            Assert.AreEqual("defaultexecute", result.ToString());
        }

        [Test] public void SpecificOperatorIsFound() {
            processor.AddOperator(defaultTest);
            processor.AddOperator(specificTestA);
            processor.AddOperator(specificTestB);
            object result = processor.Execute(new TreeLeaf<string>("A"));
            Assert.AreEqual("executeA", result.ToString());
        }

         [Test] public void TypeIsCreated() {
            object result = processor.Create("fitnesse.unitTest.engine.SampleClass");
            Assert.IsTrue(result is SampleClass);
        }

        [Test] public void MethodIsInvoked() {
            var instance = new SampleClass();
            TypedValue result = processor.Invoke(instance, "methodnoparms", new TreeList<string>());
            Assert.AreEqual("samplereturn", result.Value);
        }

        [Test] public void MethodWithParameterIsInvoked() {
            var instance = new SampleClass();
            TypedValue result = processor.Invoke(instance, "MethodWithParms", new TreeList<string>().AddBranchValue("stringparm0"));
            Assert.AreEqual("samplestringparm0", result.Value);
        }

        [Test] public void OperatorIsRemoved() {
            processor.AddOperator(defaultTest);
            processor.AddOperator(specificTestA);
            object result = processor.Execute(new TreeLeaf<string>("A"));
            Assert.AreEqual("executeA", result.ToString());
            processor.RemoveOperator(specificTestA.GetType().FullName);
            result = processor.Execute(new TreeList<string>("A"));
            Assert.AreEqual("defaultexecute", result.ToString());
        }

        [Test] public void EmptyMemoryContainsNothing() {
            processor.AddMemory<string>();
            Assert.IsFalse(processor.Contains("anything"));
        }

        [Test] public void StoredDataIsLoaded() {
            processor.AddMemory<KeyValueMemory<string, string>>();
            processor.Store(new KeyValueMemory<string, string>("something", "stuff"));
            Assert.IsTrue(processor.Contains(new KeyValueMemory<string, string>("something")));
            Assert.AreEqual("stuff", processor.Load(new KeyValueMemory<string, string>("something")).Instance);
        }

        private class DefaultTest: ExecuteOperator<string> {
            public bool IsMatch(Processor<string> processor, State<string> state) {
                return true;
            }

            public object Execute(Processor<string> processor, State<string> state) {
                return "defaultexecute";
            }
        }

        private class SpecificTest: ExecuteOperator<string> {
            private readonly string name;
            public SpecificTest(string name) {
                this.name = name;
            }
            public bool IsMatch(Processor<string> processor, State<string> state) {
                return state.ParameterValueString == name;
            }

            public object Execute(Processor<string> processor, State<string> state) {
                return "execute" + name;
            }
        }

    }
}