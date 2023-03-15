using Microsoft.Kiota.Abstractions.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace ApiSdk.Models {
    public class PlannerPlanDetails : Entity, IParsable {
        /// <summary>The categoryDescriptions property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public PlannerCategoryDescriptions? CategoryDescriptions { get; set; }
#nullable restore
#else
        public PlannerCategoryDescriptions CategoryDescriptions { get; set; }
#endif
        /// <summary>The sharedWith property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public PlannerUserIds? SharedWith { get; set; }
#nullable restore
#else
        public PlannerUserIds SharedWith { get; set; }
#endif
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static new PlannerPlanDetails CreateFromDiscriminatorValue(IParseNode parseNode) {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new PlannerPlanDetails();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        public new IDictionary<string, Action<IParseNode>> GetFieldDeserializers() {
            return new Dictionary<string, Action<IParseNode>>(base.GetFieldDeserializers()) {
                {"categoryDescriptions", n => { CategoryDescriptions = n.GetObjectValue<PlannerCategoryDescriptions>(PlannerCategoryDescriptions.CreateFromDiscriminatorValue); } },
                {"sharedWith", n => { SharedWith = n.GetObjectValue<PlannerUserIds>(PlannerUserIds.CreateFromDiscriminatorValue); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public new void Serialize(ISerializationWriter writer) {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            base.Serialize(writer);
            writer.WriteObjectValue<PlannerCategoryDescriptions>("categoryDescriptions", CategoryDescriptions);
            writer.WriteObjectValue<PlannerUserIds>("sharedWith", SharedWith);
        }
    }
}
