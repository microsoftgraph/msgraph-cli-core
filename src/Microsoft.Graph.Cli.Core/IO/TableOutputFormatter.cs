using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text.Json;
using Spectre.Console;

namespace Microsoft.Graph.Cli.Core.IO
{

    public class TableOutputFormatter : IOutputFormatter
    {
        public void WriteOutput(string content, IConsole console)
        {
            using var doc = JsonDocument.Parse(content);
            var table = this.ConstructTable(doc);
            AnsiConsole.Write(table);
        }

        public void WriteOutput(Stream content, IConsole console)
        {
            using var doc = JsonDocument.Parse(content);
            var table = this.ConstructTable(doc);
            AnsiConsole.Write(table);
        }

        private Table ConstructTable(JsonDocument document) {
            var root = document.RootElement;
            JsonElement firstElement;
            JsonElement value;
            var hasValueProperty = root.TryGetProperty("value", out value);
            if (hasValueProperty) {
                root = value;
            }

            if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0) {
                var enumerated = root.EnumerateArray();
                firstElement = enumerated.FirstOrDefault();
            } else {
                firstElement = root;
            }

            var properties = new List<(JsonValueKind,string)>();
            var table = new Table();
            if (firstElement.ValueKind != JsonValueKind.Object) {
                properties.Add((firstElement.ValueKind, "Value"));
            } else {
                var restrictedValueKinds = new JsonValueKind[] {
                    JsonValueKind.Array,
                    JsonValueKind.Null,
                    JsonValueKind.Object,
                    JsonValueKind.Undefined
                };
                properties = firstElement.EnumerateObject()
                    .Where(p => !restrictedValueKinds.Contains(p.Value.ValueKind))
                    .Select(p => (p.Value.ValueKind, p.Name)).ToList();
            }

            if (root.ValueKind == JsonValueKind.Array) {
                foreach (var property in properties)
                    table.AddColumn(property.Item2, column => {
                        if (property.Item1 == JsonValueKind.Number)
                            column.RightAligned().PadLeft(10);
                    });

                foreach (var row in root.EnumerateArray())
                {
                    var rowCols = properties.Select(p => {
                        var valueKind = p.Item1;
                        var propertyName = p.Item2;
                        JsonElement property;
                        var hasProp = row.TryGetProperty(propertyName, out property);
                        if (hasProp) {
                            object? value = null;
                            switch (valueKind)
                            {
                                case JsonValueKind.String:
                                    value = property.GetString();
                                    break;
                                case JsonValueKind.True:
                                case JsonValueKind.False:
                                    value = property.GetBoolean();
                                    break;
                                case JsonValueKind.Number:
                                    value = property.GetDecimal();
                                    break;
                            }
                            return new Markup(value?.ToString() ?? "-");
                        } else {
                            return new Markup("-");
                        }
                    });
                    table.AddRow(rowCols);
                }
            }

            return table;
        }
    }
}
