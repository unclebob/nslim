using fitnesse.mtee.engine;
using fitnesse.mtee.operators;
using NUnit.Framework;

namespace fitnesse.unitTest.operators {
    [TestFixture] public class ConverterTest {
        private readonly Processor<string> processor = new Processor<string>(new ApplicationUnderTest());

        [Test] public void CustomTypeIsParsed() {
            var converter = new CustomConverter();
            var state = processor.Command.WithType(typeof(CustomClass)).WithValue("info");
            Assert.IsTrue(converter.IsMatch(state));
            var result = converter.Parse(state) as CustomClass;
            Assert.IsNotNull(result);
            Assert.AreEqual("custominfo", result.Info);
        }

        [Test] public void CustomTypeIsComposed() {
            var converter = new CustomConverter();
            var state = processor.Command.WithInstance(new CustomClass {Info = "stuff"}).WithType(typeof(CustomClass));
            Assert.IsTrue(converter.IsMatch(state));
            var result = converter.Compose(state).Value;
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
