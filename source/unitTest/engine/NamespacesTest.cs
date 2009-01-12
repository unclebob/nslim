// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.engine;
using NUnit.Framework;

namespace fitnesse.unitTest.engine {
    [TestFixture] public class NamespacesTest {
        private Namespaces namespaces;

        [SetUp] public void SetUp() {
            namespaces = new Namespaces();
        }

        [Test] public void DefaultNamespaceIsRegistered() {
            Assert.IsTrue(namespaces.IsRegistered("fitnesse.mtee.engine"));
        }

        [Test] public void NewNamespaceIsRegistered() {
            Assert.IsFalse(namespaces.IsRegistered("test"));
            namespaces.Add("test");
            Assert.IsTrue(namespaces.IsRegistered("test"));
        }

        [Test] public void NamespaceIsTrimmed() {
            Assert.IsFalse(namespaces.IsRegistered("test"));
            namespaces.Add(" test\n");
            Assert.IsTrue(namespaces.IsRegistered("test"));
        }
    }
}
