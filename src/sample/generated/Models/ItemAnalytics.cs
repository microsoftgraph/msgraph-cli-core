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
    public partial class ItemAnalytics : global::ApiSdk.Models.Entity, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>The allTime property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::ApiSdk.Models.ItemActivityStat? AllTime { get; set; }
#nullable restore
#else
        public global::ApiSdk.Models.ItemActivityStat AllTime { get; set; }
#endif
        /// <summary>The itemActivityStats property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::ApiSdk.Models.ItemActivityStat>? ItemActivityStats { get; set; }
#nullable restore
#else
        public List<global::ApiSdk.Models.ItemActivityStat> ItemActivityStats { get; set; }
#endif
        /// <summary>The lastSevenDays property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::ApiSdk.Models.ItemActivityStat? LastSevenDays { get; set; }
#nullable restore
#else
        public global::ApiSdk.Models.ItemActivityStat LastSevenDays { get; set; }
#endif
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::ApiSdk.Models.ItemAnalytics"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static new global::ApiSdk.Models.ItemAnalytics CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::ApiSdk.Models.ItemAnalytics();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public override IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>(base.GetFieldDeserializers())
            {
                { "allTime", n => { AllTime = n.GetObjectValue<global::ApiSdk.Models.ItemActivityStat>(global::ApiSdk.Models.ItemActivityStat.CreateFromDiscriminatorValue); } },
                { "itemActivityStats", n => { ItemActivityStats = n.GetCollectionOfObjectValues<global::ApiSdk.Models.ItemActivityStat>(global::ApiSdk.Models.ItemActivityStat.CreateFromDiscriminatorValue)?.AsList(); } },
                { "lastSevenDays", n => { LastSevenDays = n.GetObjectValue<global::ApiSdk.Models.ItemActivityStat>(global::ApiSdk.Models.ItemActivityStat.CreateFromDiscriminatorValue); } },
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
            writer.WriteObjectValue<global::ApiSdk.Models.ItemActivityStat>("allTime", AllTime);
            writer.WriteCollectionOfObjectValues<global::ApiSdk.Models.ItemActivityStat>("itemActivityStats", ItemActivityStats);
            writer.WriteObjectValue<global::ApiSdk.Models.ItemActivityStat>("lastSevenDays", LastSevenDays);
        }
    }
}
#pragma warning restore CS0618
