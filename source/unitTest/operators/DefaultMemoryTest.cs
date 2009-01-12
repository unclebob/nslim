// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.engine;
using fitnesse.mtee.operators;
using NUnit.Framework;

namespace fitnesse.unitTest.operators {
    [TestFixture] public class DefaultMemoryTest {
        private DefaultMemory memory;
        private readonly Processor processor = new Processor(new Assemblies());

        [SetUp] public void SetUp() {
            memory = new DefaultMemory();
        }

        [Test] public void EmptyMemoryContainsNothing() {
            Assert.IsFalse(memory.Contains(processor, new State("anything")));
        }

        [Test] public void StoredDataIsLoaded() {
            memory.Store(processor, new State("stuff", "something"));
            Assert.IsTrue(memory.Contains(processor, new State("something")));
            Assert.AreEqual("stuff", memory.Load(processor, new State("something")));
        }
    }
}
