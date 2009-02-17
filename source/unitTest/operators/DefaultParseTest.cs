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
        private DefaultParse<string> parse;
        private readonly Processor<string> processor = new Processor<string>(new ApplicationUnderTest());

        [SetUp] public void SetUp() {
            parse = new DefaultParse<string>();
        }

        [Test] public void StringIsParsed() {
            object result = null;
            parse.TryParse(processor, typeof (string), TypedValue.Void, new TreeLeaf<string>("stuff"), ref result);
            Assert.AreEqual("stuff", result);
        }

        [Test] public void DateIsParsed() {
            object result = null;
            parse.TryParse(processor, typeof(DateTime), TypedValue.Void, new TreeLeaf<string>("03 Jan 2008"), ref result);
            Assert.AreEqual(new DateTime(2008, 1, 3), result);
        }

        [Test] public void ClassIsParsed() {
            object result = null;
            parse.TryParse(processor, typeof(SampleClass), TypedValue.Void, new TreeLeaf<string>("stuff"), ref result);
            Assert.IsTrue(result is SampleClass);
        }
    }
}
