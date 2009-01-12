// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System;
using fitnesse.mtee.engine;
using fitnesse.mtee.model;
using fitnesse.mtee.operators;
using fitnesse.unitTest.engine;
using NUnit.Framework;

namespace fitnesse.unitTest.operators {
    [TestFixture] public class DefaultParseTest {
        private DefaultParse parse;
        private readonly Processor processor = new Processor(new Assemblies());

        [SetUp] public void SetUp() {
            parse = new DefaultParse();
        }

        [Test] public void StringIsParsed() {
            Assert.AreEqual("stuff", parse.Parse(processor, new State(typeof(string), new TreeLeaf<object>("stuff"))));
        }

        [Test] public void DateIsParsed() {
            Assert.AreEqual(new DateTime(2008, 1, 3), parse.Parse(processor, new State(typeof(DateTime), new TreeLeaf<object>("03 Jan 2008"))));
        }

        [Test] public void ClassIsParsed() {
            Assert.IsTrue(parse.Parse(processor, new State(typeof(SampleClass), new TreeLeaf<object>("stuff"))) is SampleClass);
        }
    }
}
