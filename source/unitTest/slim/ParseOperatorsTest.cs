// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System.Collections.Generic;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;
using fitnesse.slim.operators;
using NUnit.Framework;

namespace fitnesse.unitTest.slim {
    [TestFixture] public class ParseOperatorsTest {
        private Processor processor;

        [SetUp] public void SetUp() {
            processor = new Processor(new Assemblies());
        }

        [Test] public void ParseSymbolReplacesWithValue() {
            processor.Store("$symbol", "testvalue");
            Assert.AreEqual("testvalue", Parse(new ParseSymbol(), new State(typeof(object), new TreeLeaf<object>("$symbol"))));
        }

        [Test] public void ParseSymbolReplacesEmbeddedValues() {
            processor.Store("$symbol1", "test");
            processor.Store("$symbol2", "value");
            Assert.AreEqual("-testvalue-", Parse(new ParseSymbol(), new State(typeof(object), new TreeLeaf<object>("-$symbol1$symbol2-"))));
        }

        [Test] public void TreeIsParsedForList() {
            var list =
                Parse(new ParseList(),
                      new State(typeof (List<int>), new TreeList<object>().AddBranch("5").AddBranch("4"))) as List<int>;
            Assert.IsNotNull(list);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(5, list[0]);
            Assert.AreEqual(4, list[1]);
        }

        private object Parse(ParseOperator parseOperator, State state) {
            Assert.IsTrue(parseOperator.IsMatch(processor, state));
            return parseOperator.Parse(processor, state);
        }
    }
}
