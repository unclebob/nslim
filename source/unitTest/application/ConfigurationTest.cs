// Copyright © Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using fitnesse.mtee.application;
using NUnit.Framework;

namespace fitnesse.unitTest.application {
    [TestFixture] public class ConfigurationTest {

        [Test] public void MethodIsExecuted() {
            var configuration = new Configuration();
            configuration.LoadXml("<config><fitnesse.unitTest.application.TestConfig><TestMethod>stuff</TestMethod></fitnesse.unitTest.application.TestConfig></config>");
            Assert.AreEqual("stuff", configuration.GetItem<TestConfig>().Data);
        }

        [Test] public void MethodWithTwoParametersIsExecuted() {
            var configuration = new Configuration();
            configuration.LoadXml("<config><fitnesse.unitTest.application.TestConfig><TestMethod second=\"more\">stuff</TestMethod></fitnesse.unitTest.application.TestConfig></config>");
            Assert.AreEqual("more stuff", configuration.GetItem<TestConfig>().Data);
        }

        [Test] public void TwoFilesAreLoadedIncrementally() {
            var configuration = new Configuration();
            configuration.LoadXml("<config><fitnesse.unitTest.application.TestConfig><TestMethod>stuff</TestMethod></fitnesse.unitTest.application.TestConfig></config>");
            configuration.LoadXml("<config><fitnesse.unitTest.application.TestConfig><Append>more</Append></fitnesse.unitTest.application.TestConfig></config>");
            Assert.AreEqual("stuffmore", configuration.GetItem<TestConfig>().Data);
        }
    }

    public class TestConfig {
        public string Data;

        public void TestMethod(string data) {
            Data = data;
        }

        public void TestMethod(string data, string more) {
            Data = more + " " + data;
        }

        public void Append(string data) {
            Data += data;
        }
    }
}