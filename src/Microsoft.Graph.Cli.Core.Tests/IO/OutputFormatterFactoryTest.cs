using System;
using Microsoft.Graph.Cli.Core.IO;
using Xunit;

namespace Microsoft.Graph.Cli.Core.Tests.IO;

public class OutputFormatterFactoryTest {
    public class InstanceProperty_Should {
        [Fact]
        public void ReturnOutputFormatterFactoryInstance() {
            var instance = OutputFormatterFactory.Instance;

            Assert.NotNull(instance);
        }

        [Fact]
        public void ReturnSingletonInstance_On_Multiple_Calls() {
            var instance1 = OutputFormatterFactory.Instance;
            var instance2 = OutputFormatterFactory.Instance;

            Assert.Same(instance1, instance2);
        }
    }

    public class GetFormatterFunction_Should {
        [Theory]
        [InlineData(FormatterType.NONE)]
        public void ThrowException_On_Invalid_FormatterType(FormatterType formatterType) {
            var factory = OutputFormatterFactory.Instance;

            Assert.Throws<NotSupportedException>(() => factory.GetFormatter(formatterType));
        }

        [Fact]
        public void Return_JsonOutputFormatter_On_JSON_FormatterType() {
            var factory = OutputFormatterFactory.Instance;

            var formatter = factory.GetFormatter(FormatterType.JSON);

            Assert.NotNull(formatter);
            Assert.True(formatter is JsonOutputFormatter);
        }

        [Fact]
        public void Return_JsonOutputFormatter_On_JSON_String() {
            var factory = OutputFormatterFactory.Instance;

            var formatter = factory.GetFormatter("json");

            Assert.NotNull(formatter);
            Assert.True(formatter is JsonOutputFormatter);
        }
    }
}