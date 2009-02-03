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

        private Processor<string> processor;

        [SetUp] public void SetUp() {
            processor = new Processor<string>(new ApplicationUnderTest());
        }
        
        [Test] public void NullIsComposed() {
            CheckCompose(new ComposeDefault(), State<string>.MakeCompose(null, typeof (object)), "null");
        }
        
        [Test] public void VoidIsComposed() {
            CheckCompose(new ComposeDefault(), State<string>.MakeCompose(null, typeof (void)), "/__VOID__/");
        }
        
        [Test] public void DefaultComposeIsString() {
            CheckCompose(new ComposeDefault(), State<string>.MakeCompose(1.23, typeof (double)), "1.23");
        }
        
        [Test] public void BooleanTrueIsComposed() {
            CheckCompose(new ComposeBoolean(), State<string>.MakeCompose(true, typeof (bool)), "true");
        }
        
        [Test] public void BooleanFalseIsComposed() {
            CheckCompose(new ComposeBoolean(), State<string>.MakeCompose(false, typeof (bool)), "false");
        }

        [Test] public void ListIsComposedAsTree() {
            processor.AddOperator(new ComposeDefault());
            var result = Compose(new ComposeList(), State<string>.MakeCompose(new List<object> {"a", 1.23}, typeof (List<object>)));
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Branches.Count);
            Assert.AreEqual("a", result.Branches[0].Value); 
            Assert.AreEqual("1.23", result.Branches[1].Value); 
        }

        [Test] public void NestedListIsComposedAsTree() {
            processor.AddOperator(new ComposeDefault());
            processor.AddOperator(new ComposeList());
            var result = Compose(new ComposeList(), State<string>.MakeCompose(new List<object> {"a", new List<object> {"b", "c"}}, typeof (List<object>)));
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Branches.Count);
            Assert.AreEqual("a", result.Branches[0].Value);
            Assert.AreEqual("b", result.Branches[1].Branches[0].Value); 
            Assert.AreEqual("c", result.Branches[1].Branches[1].Value); 
        }

        private Tree<string> Compose(ComposeOperator<string> composeOperator, State<string> state) {
            Assert.IsTrue(composeOperator.IsMatch(processor, state));
            return composeOperator.Compose(processor, state);
        }

        private void CheckCompose(ComposeOperator<string> composeOperator, State<string> state, object expected) {
            Assert.IsTrue(composeOperator.IsMatch(processor, state));
            Assert.AreEqual(expected, Compose(composeOperator, state).Value);
        }
    }
}
