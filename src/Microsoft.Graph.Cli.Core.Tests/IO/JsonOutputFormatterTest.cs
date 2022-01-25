using System;
using System.CommandLine;
using Microsoft.Graph.Cli.Core.IO;
using Moq;
using Xunit;

namespace Microsoft.Graph.Cli.Core.Tests.IO;

public class JsonOutputFormatterTest {
    public class WriteOutputFunction_Should {
        [Fact]
        public void Write_A_Line_With_Content() {
            var formatter = OutputFormatterFactory.Instance.GetFormatter(FormatterType.JSON);
            var consoleMock = new Mock<IConsole>();
            consoleMock.Setup(c => c.Out.Write(It.IsAny<string>()));
            var content = "Test content";

            formatter.WriteOutput(content, consoleMock.Object);

            consoleMock.Verify(console => console.Out.Write(content));
            consoleMock.Verify(console => console.Out.Write(Environment.NewLine));
            consoleMock.VerifyNoOtherCalls();
        }
    }
}