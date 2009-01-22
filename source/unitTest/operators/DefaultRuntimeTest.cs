// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.engine;
using fitnesse.mtee.model;
using fitnesse.mtee.Model;
using fitnesse.mtee.operators;
using fitnesse.unitTest.engine;
using NUnit.Framework;

namespace fitnesse.unitTest.operators {
    [TestFixture] public class DefaultRuntimeTest {
        private DefaultRuntime runtime;
        private readonly Processor processor = new Processor(new SystemUnderTest());

        [SetUp] public void SetUp() {
            runtime = new DefaultRuntime();
        }

        [Test] public void InstanceIsCreated() {
            Assert.IsTrue(
                runtime.Create(processor, new State("fitnesse.unitTest.engine.SampleClass", new TreeList<object>())) is SampleClass);
        }

        [Test] public void StandardInstanceIsCreated() {
            Assert.IsTrue(
                runtime.Create(processor, new State("System.Boolean", new TreeList<object>())) is bool);
        }

        [Test] public void MethodIsInvoked() {
            TypedValue result = runtime.Invoke(processor,
                                               new State(new SampleClass(), typeof (SampleClass), "methodwithparms",
                                                         new TreeList<object>().AddBranch("stuff")));
            Assert.AreEqual(typeof(string), result.Type);
            Assert.AreEqual("samplestuff", result.Value);
        }
    }
}
