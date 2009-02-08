// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.engine;
using fitnesse.mtee.model;
using fitnesse.mtee.operators;
using fitnesse.unitTest.engine;
using NUnit.Framework;

namespace fitnesse.unitTest.operators {
    [TestFixture] public class DefaultRuntimeTest {
        private DefaultRuntime<string> runtime;
        private readonly BasicProcessor processor = new BasicProcessor();

        [SetUp] public void SetUp() {
            runtime = new DefaultRuntime<string>();
        }

        [Test] public void InstanceIsCreated() {
            Assert.IsTrue(
                runtime.Create(processor.Command.WithMember("fitnesse.unitTest.engine.SampleClass").WithParameters(new TreeList<string>())) is SampleClass);
        }

        [Test] public void StandardInstanceIsCreated() {
            Assert.IsTrue(
                runtime.Create(processor.Command.WithMember("System.Boolean").WithParameters(new TreeList<string>())) is bool);
        }

        [Test] public void MethodIsInvoked() {
            TypedValue result = runtime.Invoke(processor.Command
                                                   .WithInstance(new SampleClass())
                                                   .WithType(typeof(SampleClass))
                                                   .WithMember("methodwithparms")
                                                   .WithParameters(new TreeList<string>().AddBranchValue("stuff")));
            Assert.AreEqual(typeof(string), result.Type);
            Assert.AreEqual("samplestuff", result.Value);
        }
    }
}
