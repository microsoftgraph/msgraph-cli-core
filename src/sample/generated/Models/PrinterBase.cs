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
    public partial class PrinterBase : global::ApiSdk.Models.Entity, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>The capabilities property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::ApiSdk.Models.PrinterCapabilities? Capabilities { get; set; }
#nullable restore
#else
        public global::ApiSdk.Models.PrinterCapabilities Capabilities { get; set; }
#endif
        /// <summary>The defaults property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::ApiSdk.Models.PrinterDefaults? Defaults { get; set; }
#nullable restore
#else
        public global::ApiSdk.Models.PrinterDefaults Defaults { get; set; }
#endif
        /// <summary>The name of the printer/printerShare.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? DisplayName { get; set; }
#nullable restore
#else
        public string DisplayName { get; set; }
#endif
        /// <summary>Specifies whether the printer/printerShare is currently accepting new print jobs.</summary>
        public bool? IsAcceptingJobs { get; set; }
        /// <summary>The list of jobs that are queued for printing by the printer/printerShare.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::ApiSdk.Models.PrintJob>? Jobs { get; set; }
#nullable restore
#else
        public List<global::ApiSdk.Models.PrintJob> Jobs { get; set; }
#endif
        /// <summary>The location property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::ApiSdk.Models.PrinterLocation? Location { get; set; }
#nullable restore
#else
        public global::ApiSdk.Models.PrinterLocation Location { get; set; }
#endif
        /// <summary>The manufacturer of the printer/printerShare.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Manufacturer { get; set; }
#nullable restore
#else
        public string Manufacturer { get; set; }
#endif
        /// <summary>The model name of the printer/printerShare.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Model { get; set; }
#nullable restore
#else
        public string Model { get; set; }
#endif
        /// <summary>The status property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::ApiSdk.Models.PrinterStatus? Status { get; set; }
#nullable restore
#else
        public global::ApiSdk.Models.PrinterStatus Status { get; set; }
#endif
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::ApiSdk.Models.PrinterBase"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static new global::ApiSdk.Models.PrinterBase CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::ApiSdk.Models.PrinterBase();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public override IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>(base.GetFieldDeserializers())
            {
                { "capabilities", n => { Capabilities = n.GetObjectValue<global::ApiSdk.Models.PrinterCapabilities>(global::ApiSdk.Models.PrinterCapabilities.CreateFromDiscriminatorValue); } },
                { "defaults", n => { Defaults = n.GetObjectValue<global::ApiSdk.Models.PrinterDefaults>(global::ApiSdk.Models.PrinterDefaults.CreateFromDiscriminatorValue); } },
                { "displayName", n => { DisplayName = n.GetStringValue(); } },
                { "isAcceptingJobs", n => { IsAcceptingJobs = n.GetBoolValue(); } },
                { "jobs", n => { Jobs = n.GetCollectionOfObjectValues<global::ApiSdk.Models.PrintJob>(global::ApiSdk.Models.PrintJob.CreateFromDiscriminatorValue)?.AsList(); } },
                { "location", n => { Location = n.GetObjectValue<global::ApiSdk.Models.PrinterLocation>(global::ApiSdk.Models.PrinterLocation.CreateFromDiscriminatorValue); } },
                { "manufacturer", n => { Manufacturer = n.GetStringValue(); } },
                { "model", n => { Model = n.GetStringValue(); } },
                { "status", n => { Status = n.GetObjectValue<global::ApiSdk.Models.PrinterStatus>(global::ApiSdk.Models.PrinterStatus.CreateFromDiscriminatorValue); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public override void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            base.Serialize(writer);
            writer.WriteObjectValue<global::ApiSdk.Models.PrinterCapabilities>("capabilities", Capabilities);
            writer.WriteObjectValue<global::ApiSdk.Models.PrinterDefaults>("defaults", Defaults);
            writer.WriteStringValue("displayName", DisplayName);
            writer.WriteBoolValue("isAcceptingJobs", IsAcceptingJobs);
            writer.WriteCollectionOfObjectValues<global::ApiSdk.Models.PrintJob>("jobs", Jobs);
            writer.WriteObjectValue<global::ApiSdk.Models.PrinterLocation>("location", Location);
            writer.WriteStringValue("manufacturer", Manufacturer);
            writer.WriteStringValue("model", Model);
            writer.WriteObjectValue<global::ApiSdk.Models.PrinterStatus>("status", Status);
        }
    }
}
#pragma warning restore CS0618
