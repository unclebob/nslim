// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.engine;
using NUnit.Framework;

namespace fitnesse.unitTest.engine {
    [TestFixture] public class RuntimeTypeTest {
        private SampleClass instance;

        [SetUp] public void SetUp() {
            instance = new SampleClass();
        }

        [Test] public void VoidMethodIsInvoked() {
            RuntimeMember method = new RuntimeType(instance.GetType()).GetInstance("voidmethod", 0);
            Assert.IsNotNull(method);
            object result = method.Invoke(instance, new object[] {});
            Assert.AreEqual(typeof(void), result);
        }

        [Test] public void MethodWithReturnIsInvoked() {
            RuntimeMember method = new RuntimeType(instance.GetType()).GetInstance("methodnoparms", 0);
            Assert.IsNotNull(method);
            object result = method.Invoke(instance, new object[] {});
            Assert.AreEqual("samplereturn", result.ToString());
        }

        [Test] public void MethodWithUnderscoresIsInvoked() {
            RuntimeMember method = new RuntimeType(instance.GetType()).GetInstance("methodwithunderscores", 0);
            Assert.IsNotNull(method);
            object result = method.Invoke(instance, new object[] {});
            Assert.AreEqual("samplereturn", result.ToString());
        }

        [Test] public void MethodWithParmsIsInvoked() {
            RuntimeMember method = new RuntimeType(instance.GetType()).GetInstance("methodwithparms", 1);
            Assert.IsNotNull(method);
            object result = method.Invoke(instance, new object[] {"input"});
            Assert.AreEqual("sampleinput", result.ToString());
        }

        [Test] public void StaticMethodWithParmsIsInvoked() {
            RuntimeMember method = new RuntimeType(instance.GetType()).FindStatic("parse", new [] {typeof(string)});
            Assert.IsNotNull(method);
            object result = method.Invoke(instance.GetType(), new object[] {"input"});
            Assert.AreEqual(typeof(SampleClass), result.GetType());
        }

        [Test] public void ConstructorIsInvoked() {
            RuntimeMember method = new RuntimeType(instance.GetType()).GetConstructor(0);
            Assert.IsNotNull(method);
            object result = method.Invoke(instance.GetType(), new object[] {});
            Assert.AreEqual(typeof(SampleClass), result.GetType());
        }
    }
}
