using fitnesse.mtee.engine;
using fitnesse.mtee.model;
using fitnesse.mtee.operators;
using NUnit.Framework;

namespace fitnesse.unitTest.operators {
    [TestFixture] public class ConverterTest {
        private readonly Processor processor = new Processor(new SystemUnderTest());

        [Test] public void CustomTypeIsParsed() {
            var converter = new CustomConverter();
            var state = new State(typeof(CustomClass), new TreeLeaf<object>("info"));
            Assert.IsTrue(converter.IsMatch(processor, state));
            var result = converter.Parse(processor, state) as CustomClass;
            Assert.IsNotNull(result);
            Assert.AreEqual("custominfo", result.Info);
        }

        [Test] public void CustomTypeIsComposed() {
            var converter = new CustomConverter();
            var state = new State(new CustomClass {Info = "stuff"}, typeof(CustomClass));
            Assert.IsTrue(converter.IsMatch(processor, state));
            var result = converter.Compose(processor, state) as string;
            Assert.IsNotNull(result);
            Assert.AreEqual("mystuff", result);
        }

        private class CustomConverter: Converter<CustomClass> {
            protected override CustomClass Parse(string input) {
                return new CustomClass {Info = ("custom" + input)};
            }

            protected override string Compose(CustomClass input) {
                return "my" + input.Info;
            }
        }

        private class CustomClass {
            public string Info;
        }
    }
}
