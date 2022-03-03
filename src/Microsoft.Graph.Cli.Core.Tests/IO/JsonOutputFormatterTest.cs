using System;
using System.CommandLine;
using System.IO;
using System.Text;
using Microsoft.Graph.Cli.Core.IO;
using Microsoft.Kiota.Cli.Commons.IO;
using Moq;
using Spectre.Console;
using Xunit;

namespace Microsoft.Graph.Cli.Core.Tests.IO;

public class JsonOutputFormatterTest {
    public class WriteOutputFunction_Should {
        [Fact]
        public void Write_A_Line_With_String_Content() {
            var formatter = new OutputFormatterFactory().GetFormatter(FormatterType.JSON);
            var content = "Test content";
            var stringWriter = new StringWriter();
            var console = AnsiConsole.Create(new AnsiConsoleSettings {Out = new AnsiConsoleOutput(stringWriter)});
            AnsiConsole.Console = console;

            formatter.WriteOutput(content, new JsonOutputFormatterOptions());

            Assert.Equal($"\"{content}\"{Environment.NewLine}", stringWriter.ToString());
        }

        [Fact]
        public void Write_Indented_Output_Given_A_Minified_Json_String() {
            var formatter = new OutputFormatterFactory().GetFormatter(FormatterType.JSON);
            var content = "{\"a\": 1, \"b\": \"test\"}";
            var stringWriter = new StringWriter();
            var console = AnsiConsole.Create(new AnsiConsoleSettings {Out = new AnsiConsoleOutput(stringWriter)});
            AnsiConsole.Console = console;

            formatter.WriteOutput(content, new JsonOutputFormatterOptions());
            var expected = "\"{\\u0022a\\u0022: 1, \\u0022b\\u0022: \\u0022test\\u0022}\"\r\n";

            Assert.Equal(expected, stringWriter.ToString());
        }

        [Fact]
        public void Write_A_Line_With_Stream_Content() {
            var formatter = new OutputFormatterFactory().GetFormatter(FormatterType.JSON);
            var content = "Test content";
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(content));
            var stringWriter = new StringWriter();
            var console = AnsiConsole.Create(new AnsiConsoleSettings {Out = new AnsiConsoleOutput(stringWriter)});
            AnsiConsole.Console = console;

            formatter.WriteOutput(stream, new JsonOutputFormatterOptions());

            Assert.Equal($"\"{content}\"{Environment.NewLine}", stringWriter.ToString());
        }

        [Fact]
        public void Write_Indented_Output_Given_A_Minified_Json_Stream() {
            var formatter = new OutputFormatterFactory().GetFormatter(FormatterType.JSON);
            var content = "{\"a\": 1, \"b\": \"test\"}";
            var stream = new MemoryStream(Encoding.ASCII.GetBytes(content));
            var stringWriter = new StringWriter();
            var console = AnsiConsole.Create(new AnsiConsoleSettings {Out = new AnsiConsoleOutput(stringWriter)});
            AnsiConsole.Console = console;

            formatter.WriteOutput(stream, new JsonOutputFormatterOptions());
            var expected = "\"{\\u0022a\\u0022: 1, \\u0022b\\u0022: \\u0022test\\u0022}\"\r\n";

            Assert.Equal(expected, stringWriter.ToString());
        }
    }
}