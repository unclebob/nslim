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
    [TestFixture] public class ComposeOperatorsTest {

        private Processor processor;

        [SetUp] public void SetUp() {
            processor = new Processor(new Assemblies());
        }
        
        [Test] public void NullIsComposed() {
            CheckCompose(new ComposeDefault(), new State(null, typeof (object)), "null");
        }
        
        [Test] public void VoidIsComposed() {
            CheckCompose(new ComposeDefault(), new State(null, typeof (void)), "/__VOID__/");
        }
        
        [Test] public void DefaultComposeIsString() {
            CheckCompose(new ComposeDefault(), new State(1.23, typeof (double)), "1.23");
        }
        
        [Test] public void BooleanTrueIsComposed() {
            CheckCompose(new ComposeBoolean(), new State(true, typeof (bool)), "true");
        }
        
        [Test] public void BooleanFalseIsComposed() {
            CheckCompose(new ComposeBoolean(), new State(false, typeof (bool)), "false");
        }

        [Test] public void ListIsComposedAsTree() {
            processor.Add(new ComposeDefault());
            var result = Compose(new ComposeList(), new State(new List<object> {"a", 1.23}, typeof (List<object>))) as Tree<object>;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Branches.Count);
            Assert.AreEqual("a", result.Branches[0].Value); 
            Assert.AreEqual("1.23", result.Branches[1].Value); 
        }

        [Test] public void NestedListIsComposedAsTree() {
            processor.Add(new ComposeDefault());
            processor.Add(new ComposeList());
            var result = Compose(new ComposeList(), new State(new List<object> {"a", new List<object> {"b", "c"}}, typeof (List<object>))) as Tree<object>;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Branches.Count);
            Assert.AreEqual("a", result.Branches[0].Value);
            Assert.AreEqual("b", result.Branches[1].Branches[0].Value); 
            Assert.AreEqual("c", result.Branches[1].Branches[1].Value); 
        }

        private object Compose(ComposeOperator composeOperator, State state) {
            Assert.IsTrue(composeOperator.IsMatch(processor, state));
            return composeOperator.Compose(processor, state);
        }

        private void CheckCompose(ComposeOperator composeOperator, State state, object expected) {
            Assert.IsTrue(composeOperator.IsMatch(processor, state));
            Assert.AreEqual(expected, Compose(composeOperator, state));
        }
    }
}
