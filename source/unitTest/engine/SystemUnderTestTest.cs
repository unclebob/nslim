// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;
using NUnit.Framework;

namespace fitnesse.unitTest.engine {
    [TestFixture] public class SystemUnderTestTest {
        private SystemUnderTest systemUnderTest;

        [SetUp] public void SetUp() {
            systemUnderTest = new SystemUnderTest();
        }

        [Test] public void TypeIsFoundInCurrentAssembly() {
            CheckTypeFound<SampleClass>("fitnesse.unitTest.engine.SampleClass");
        }

        [Test] public void TypeWithoutNamespaceIsFound() {
            CheckTypeFound<AnotherSampleClass>("AnotherSampleClass");
        }

        [Test] public void TypeIsFoundUsingNamespaces() {
            CheckTypeNotFound("SampleClass");
            systemUnderTest.AddNamespace("fitnesse.unitTest.engine");
            CheckTypeFound<SampleClass>("SampleClass");
        }

        [Test] public void NamespaceIsTrimmed() {
            systemUnderTest.AddNamespace(" fitnesse.unitTest.engine\n");
            CheckTypeFound<SampleClass>("SampleClass");
        }

        [Test] public void TypeIsFoundInLoadedAssembly() {
            systemUnderTest.AddAssembly("sample.dll");
            RuntimeType sample = GetType("fitnesse.sample.SampleDomain");
            Assert.AreEqual("fitnesse.sample.SampleDomain", sample.Type.FullName);
        }

        [Test] public void ReloadingAssemblyIsIgnored() {
            systemUnderTest.AddAssembly("sample.dll");
            systemUnderTest.AddAssembly("sample.dll");
            RuntimeType sample = GetType("fitnesse.sample.SampleDomain");
            Assert.AreEqual("fitnesse.sample.SampleDomain", sample.Type.FullName);
        }

        [Test] public void TypeIsFoundInDefaultNamespace() {
            CheckTypeFound<SystemUnderTest>("SystemUnderTest");
        }

        [Test] public void NamespaceIsRemoved() {
            systemUnderTest.AddNamespace("fitnesse.unitTest.engine");
            CheckTypeFound<SampleClass>("SampleClass");
            systemUnderTest.RemoveNamespace("fitnesse.unitTest.engine");
            CheckTypeNotFound("SampleClass");
        }

        [Test] public void ChangesNotMadeInCopy() {
            var copy = (SystemUnderTest)systemUnderTest.Copy();
            systemUnderTest.AddNamespace("fitnesse.unitTest.engine");
            systemUnderTest = copy;
            CheckTypeNotFound("SampleClass");
        }

        private void CheckTypeFound<T>(string typeName) {
            RuntimeType sample = GetType(typeName);
            Assert.AreEqual(typeof(T), sample.Type);
        }

        private void CheckTypeNotFound(string typeName) {
            string message = string.Empty;
            try {
                GetType(typeName);
            }
            catch (Exception e) {
                message = e.Message;
            }
            Assert.IsTrue(message.StartsWith("Type 'SampleClass' not found in assemblies"));
        }

        private RuntimeType GetType(string name) {
            return systemUnderTest.FindType(new IdentifierName(name));
        }
    }
}

public class AnotherSampleClass {}
