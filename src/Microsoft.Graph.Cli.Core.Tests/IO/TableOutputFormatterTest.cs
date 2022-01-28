using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Microsoft.Graph.Cli.Core.IO;
using Moq;
using Spectre.Console;
using Spectre.Console.Rendering;
using Spectre.Console.Testing;
using Xunit;

namespace Microsoft.Graph.Cli.Core.Tests.IO;

public class TableOutputFormatterTest {
    public class ConstructTableFunction_Should {
        [Fact]
        public void Create_A_Table_With_Single_Column_And_Row_When_Object_With_Value() {
            var console = new TestConsole();
            var formatter = new TableOutputFormatter();
            var content = "{\"x\": \"\", \"value\": 10}";
            var doc = JsonDocument.Parse(content);

            var table = formatter.ConstructTable(doc);

            Assert.Single(table.Columns);
            Assert.Single(table.Rows);
            var headerText = table.Columns[0].Header.GetSegments(console).Select(s => s.Text).FirstOrDefault();
            Assert.Equal("Value", headerText);
            var rowCellText = table.Rows.First()[0].GetSegments(console).Select(s => s.Text).FirstOrDefault();
            Assert.Equal("10", rowCellText);
        }

        [Fact]
        public void Create_A_Table_Given_An_JSON_Array() {
            var console = new TestConsole();
            var formatter = new TableOutputFormatter();
            var content = "[{\"a\": \"value a\", \"b\": null, \"c\": \"value c\"}, {\"a\": \"value a\", \"b\": \"value b\", \"c\": null}]";
            var doc = JsonDocument.Parse(content);

            var table = formatter.ConstructTable(doc);

            Assert.Equal(3, table.Columns.Count);
            Assert.Equal(2, table.Rows.Count);
            var headerText = table.Columns[0].Header.GetSegments(console).Select(s => s.Text).FirstOrDefault();
            Assert.Equal("a", headerText);
            var row0col1Text = table.Rows.First()[1].GetSegments(console).Select(s => s.Text).FirstOrDefault();
            Assert.Equal("-", row0col1Text);
        }

        [Fact]
        public void Create_A_Table_Given_An_JSON_ObjectWith_Value_Array() {
            var console = new TestConsole();
            var formatter = new TableOutputFormatter();
            var content = "{\"value\": [{\"a\": \"value a\", \"b\": null, \"c\": \"value c\"}, {\"a\": \"value a\", \"b\": \"value b\", \"c\": null}]}";
            var doc = JsonDocument.Parse(content);

            var table = formatter.ConstructTable(doc);

            Assert.Equal(3, table.Columns.Count);
            Assert.Equal(2, table.Rows.Count);
            var headerText = table.Columns[0].Header.GetSegments(console).Select(s => s.Text).FirstOrDefault();
            Assert.Equal("a", headerText);
            var row0col1Text = table.Rows.First()[1].GetSegments(console).Select(s => s.Text).FirstOrDefault();
            Assert.Equal("-", row0col1Text);
        }
    }
}