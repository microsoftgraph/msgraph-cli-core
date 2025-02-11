// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace ApiSdk.Models
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class WorkbookSortField : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Represents whether the sorting is done in an ascending fashion.</summary>
        public bool? Ascending { get; set; }
        /// <summary>Represents the color that is the target of the condition if the sorting is on font or cell color.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Color { get; set; }
#nullable restore
#else
        public string Color { get; set; }
#endif
        /// <summary>Represents additional sorting options for this field. The possible values are: Normal, TextAsNumber.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? DataOption { get; set; }
#nullable restore
#else
        public string DataOption { get; set; }
#endif
        /// <summary>The icon property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::ApiSdk.Models.WorkbookIcon? Icon { get; set; }
#nullable restore
#else
        public global::ApiSdk.Models.WorkbookIcon Icon { get; set; }
#endif
        /// <summary>Represents the column (or row, depending on the sort orientation) that the condition is on. Represented as an offset from the first column (or row).</summary>
        public int? Key { get; set; }
        /// <summary>Represents the type of sorting of this condition. The possible values are: Value, CellColor, FontColor, Icon.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? SortOn { get; set; }
#nullable restore
#else
        public string SortOn { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::ApiSdk.Models.WorkbookSortField"/> and sets the default values.
        /// </summary>
        public WorkbookSortField()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::ApiSdk.Models.WorkbookSortField"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::ApiSdk.Models.WorkbookSortField CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::ApiSdk.Models.WorkbookSortField();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "ascending", n => { Ascending = n.GetBoolValue(); } },
                { "color", n => { Color = n.GetStringValue(); } },
                { "dataOption", n => { DataOption = n.GetStringValue(); } },
                { "icon", n => { Icon = n.GetObjectValue<global::ApiSdk.Models.WorkbookIcon>(global::ApiSdk.Models.WorkbookIcon.CreateFromDiscriminatorValue); } },
                { "key", n => { Key = n.GetIntValue(); } },
                { "sortOn", n => { SortOn = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteBoolValue("ascending", Ascending);
            writer.WriteStringValue("color", Color);
            writer.WriteStringValue("dataOption", DataOption);
            writer.WriteObjectValue<global::ApiSdk.Models.WorkbookIcon>("icon", Icon);
            writer.WriteIntValue("key", Key);
            writer.WriteStringValue("sortOn", SortOn);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
