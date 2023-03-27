using Microsoft.Kiota.Abstractions.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace ApiSdk.Models {
    public class ChatMessagePolicyViolationPolicyTip : IAdditionalDataHolder, IParsable {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The URL a user can visit to read about the data loss prevention policies for the organization. (ie, policies about what users shouldn&apos;t say in chats)</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ComplianceUrl { get; set; }
#nullable restore
#else
        public string ComplianceUrl { get; set; }
#endif
        /// <summary>Explanatory text shown to the sender of the message.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? GeneralText { get; set; }
#nullable restore
#else
        public string GeneralText { get; set; }
#endif
        /// <summary>The list of improper data in the message that was detected by the data loss prevention app. Each DLP app defines its own conditions, examples include &apos;Credit Card Number&apos; and &apos;Social Security Number&apos;.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<string>? MatchedConditionDescriptions { get; set; }
#nullable restore
#else
        public List<string> MatchedConditionDescriptions { get; set; }
#endif
        /// <summary>
        /// Instantiates a new chatMessagePolicyViolationPolicyTip and sets the default values.
        /// </summary>
        public ChatMessagePolicyViolationPolicyTip() {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static ChatMessagePolicyViolationPolicyTip CreateFromDiscriminatorValue(IParseNode parseNode) {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new ChatMessagePolicyViolationPolicyTip();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        public IDictionary<string, Action<IParseNode>> GetFieldDeserializers() {
            return new Dictionary<string, Action<IParseNode>> {
                {"complianceUrl", n => { ComplianceUrl = n.GetStringValue(); } },
                {"generalText", n => { GeneralText = n.GetStringValue(); } },
                {"matchedConditionDescriptions", n => { MatchedConditionDescriptions = n.GetCollectionOfPrimitiveValues<string>()?.ToList(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public void Serialize(ISerializationWriter writer) {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteStringValue("complianceUrl", ComplianceUrl);
            writer.WriteStringValue("generalText", GeneralText);
            writer.WriteCollectionOfPrimitiveValues<string>("matchedConditionDescriptions", MatchedConditionDescriptions);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}