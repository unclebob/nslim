// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.engine;
using NUnit.Framework;

namespace fitnesse.unitTest.engine {
    [TestFixture] public class AssembliesTest {
        private Namespaces namespaces;
        private Assemblies assemblies;

        [SetUp] public void SetUp() {
            namespaces = new Namespaces();
            assemblies = new Assemblies(namespaces);
        }

        [Test] public void TypeIsFoundInCurrentAssembly() {
            RuntimeType sample = assemblies.FindType("fitnesse.unitTest.engine.SampleClass");
            Assert.AreEqual(typeof(SampleClass), sample.Type);
        }

        [Test] public void TypeIsFoundUsingNamespaces() {
            namespaces.Add("fitnesse.unitTest.engine");
            RuntimeType sample = assemblies.FindType("SampleClass");
            Assert.AreEqual(typeof(SampleClass), sample.Type);
        }

        [Test] public void TypeIsFoundInLoadedAssembly() {
            assemblies.Add("sample.dll");
            RuntimeType sample = assemblies.FindType("fitnesse.sample.SampleDomain");
            Assert.AreEqual("fitnesse.sample.SampleDomain", sample.Type.FullName);
        }

        [Test] public void ReloadingAssemblyIsIgnored() {
            assemblies.Add("sample.dll");
            assemblies.Add("sample.dll");
            RuntimeType sample = assemblies.FindType("fitnesse.sample.SampleDomain");
            Assert.AreEqual("fitnesse.sample.SampleDomain", sample.Type.FullName);
        }
    }
}
