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
    public partial class SynchronizationRule : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The containerFilter property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::ApiSdk.Models.ContainerFilter? ContainerFilter { get; set; }
#nullable restore
#else
        public global::ApiSdk.Models.ContainerFilter ContainerFilter { get; set; }
#endif
        /// <summary>true if the synchronization rule can be customized; false if this rule is read-only and shouldn&apos;t be changed.</summary>
        public bool? Editable { get; set; }
        /// <summary>The groupFilter property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::ApiSdk.Models.GroupFilter? GroupFilter { get; set; }
#nullable restore
#else
        public global::ApiSdk.Models.GroupFilter GroupFilter { get; set; }
#endif
        /// <summary>Synchronization rule identifier. Must be one of the identifiers recognized by the synchronization engine. Supported rule identifiers can be found in the synchronization template returned by the API.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Id { get; set; }
#nullable restore
#else
        public string Id { get; set; }
#endif
        /// <summary>Additional extension properties. Unless instructed explicitly by the support team, metadata values shouldn&apos;t be changed.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::ApiSdk.Models.StringKeyStringValuePair>? Metadata { get; set; }
#nullable restore
#else
        public List<global::ApiSdk.Models.StringKeyStringValuePair> Metadata { get; set; }
#endif
        /// <summary>Human-readable name of the synchronization rule. Not nullable.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Name { get; set; }
#nullable restore
#else
        public string Name { get; set; }
#endif
        /// <summary>Collection of object mappings supported by the rule. Tells the synchronization engine which objects should be synchronized.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::ApiSdk.Models.ObjectMapping>? ObjectMappings { get; set; }
#nullable restore
#else
        public List<global::ApiSdk.Models.ObjectMapping> ObjectMappings { get; set; }
#endif
        /// <summary>Priority relative to other rules in the synchronizationSchema. Rules with the lowest priority number will be processed first.</summary>
        public int? Priority { get; set; }
        /// <summary>Name of the source directory. Must match one of the directory definitions in synchronizationSchema.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? SourceDirectoryName { get; set; }
#nullable restore
#else
        public string SourceDirectoryName { get; set; }
#endif
        /// <summary>Name of the target directory. Must match one of the directory definitions in synchronizationSchema.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? TargetDirectoryName { get; set; }
#nullable restore
#else
        public string TargetDirectoryName { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::ApiSdk.Models.SynchronizationRule"/> and sets the default values.
        /// </summary>
        public SynchronizationRule()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::ApiSdk.Models.SynchronizationRule"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::ApiSdk.Models.SynchronizationRule CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::ApiSdk.Models.SynchronizationRule();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "containerFilter", n => { ContainerFilter = n.GetObjectValue<global::ApiSdk.Models.ContainerFilter>(global::ApiSdk.Models.ContainerFilter.CreateFromDiscriminatorValue); } },
                { "editable", n => { Editable = n.GetBoolValue(); } },
                { "groupFilter", n => { GroupFilter = n.GetObjectValue<global::ApiSdk.Models.GroupFilter>(global::ApiSdk.Models.GroupFilter.CreateFromDiscriminatorValue); } },
                { "id", n => { Id = n.GetStringValue(); } },
                { "metadata", n => { Metadata = n.GetCollectionOfObjectValues<global::ApiSdk.Models.StringKeyStringValuePair>(global::ApiSdk.Models.StringKeyStringValuePair.CreateFromDiscriminatorValue)?.AsList(); } },
                { "name", n => { Name = n.GetStringValue(); } },
                { "objectMappings", n => { ObjectMappings = n.GetCollectionOfObjectValues<global::ApiSdk.Models.ObjectMapping>(global::ApiSdk.Models.ObjectMapping.CreateFromDiscriminatorValue)?.AsList(); } },
                { "priority", n => { Priority = n.GetIntValue(); } },
                { "sourceDirectoryName", n => { SourceDirectoryName = n.GetStringValue(); } },
                { "targetDirectoryName", n => { TargetDirectoryName = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteObjectValue<global::ApiSdk.Models.ContainerFilter>("containerFilter", ContainerFilter);
            writer.WriteBoolValue("editable", Editable);
            writer.WriteObjectValue<global::ApiSdk.Models.GroupFilter>("groupFilter", GroupFilter);
            writer.WriteStringValue("id", Id);
            writer.WriteCollectionOfObjectValues<global::ApiSdk.Models.StringKeyStringValuePair>("metadata", Metadata);
            writer.WriteStringValue("name", Name);
            writer.WriteCollectionOfObjectValues<global::ApiSdk.Models.ObjectMapping>("objectMappings", ObjectMappings);
            writer.WriteIntValue("priority", Priority);
            writer.WriteStringValue("sourceDirectoryName", SourceDirectoryName);
            writer.WriteStringValue("targetDirectoryName", TargetDirectoryName);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
