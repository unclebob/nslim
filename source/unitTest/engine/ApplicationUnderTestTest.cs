// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;
using NUnit.Framework;

namespace fitnesse.unitTest.engine {
    [TestFixture] public class ApplicationUnderTestTest {
        private ApplicationUnderTest applicationUnderTest;

        [SetUp] public void SetUp() {
            applicationUnderTest = new ApplicationUnderTest();
        }

        [Test] public void TypeIsFoundInCurrentAssembly() {
            CheckTypeFound<SampleClass>("fitnesse.unitTest.engine.SampleClass");
        }

        [Test] public void TypeWithoutNamespaceIsFound() {
            CheckTypeFound<AnotherSampleClass>("AnotherSampleClass");
        }

        [Test] public void TypeIsFoundUsingNamespaces() {
            CheckTypeNotFound("SampleClass");
            applicationUnderTest.AddNamespace("fitnesse.unitTest.engine");
            CheckTypeFound<SampleClass>("SampleClass");
        }

        [Test] public void NamespaceIsTrimmed() {
            applicationUnderTest.AddNamespace(" fitnesse.unitTest.engine\n");
            CheckTypeFound<SampleClass>("SampleClass");
        }

        [Test] public void TypeIsFoundInLoadedAssembly() {
            applicationUnderTest.AddAssembly("sample.dll");
            RuntimeType sample = GetType("fitnesse.sample.SampleDomain");
            Assert.AreEqual("fitnesse.sample.SampleDomain", sample.Type.FullName);
        }

        [Test] public void ReloadingAssemblyIsIgnored() {
            applicationUnderTest.AddAssembly("sample.dll");
            applicationUnderTest.AddAssembly("sample.dll");
            RuntimeType sample = GetType("fitnesse.sample.SampleDomain");
            Assert.AreEqual("fitnesse.sample.SampleDomain", sample.Type.FullName);
        }

        [Test] public void TypeIsFoundInDefaultNamespace() {
            CheckTypeFound<ApplicationUnderTest>("ApplicationUnderTest");
        }

        [Test] public void NamespaceIsRemoved() {
            applicationUnderTest.AddNamespace("fitnesse.unitTest.engine");
            CheckTypeFound<SampleClass>("SampleClass");
            applicationUnderTest.RemoveNamespace("fitnesse.unitTest.engine");
            CheckTypeNotFound("SampleClass");
        }

        [Test] public void ChangesNotMadeInCopy() {
            var copy = (ApplicationUnderTest)applicationUnderTest.Copy();
            applicationUnderTest.AddNamespace("fitnesse.unitTest.engine");
            applicationUnderTest = copy;
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
            return applicationUnderTest.FindType(new IdentifierName(name));
        }
    }
}

public class AnotherSampleClass {}
