using Microsoft.Kiota.Abstractions.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace ApiSdk.Models {
    public class ChatMessagePolicyViolation : IAdditionalDataHolder, IParsable {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The dlpAction property</summary>
        public ChatMessagePolicyViolationDlpActionTypes? DlpAction { get; set; }
        /// <summary>Justification text provided by the sender of the message when overriding a policy violation.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? JustificationText { get; set; }
#nullable restore
#else
        public string JustificationText { get; set; }
#endif
        /// <summary>The policyTip property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public ChatMessagePolicyViolationPolicyTip? PolicyTip { get; set; }
#nullable restore
#else
        public ChatMessagePolicyViolationPolicyTip PolicyTip { get; set; }
#endif
        /// <summary>The userAction property</summary>
        public ChatMessagePolicyViolationUserActionTypes? UserAction { get; set; }
        /// <summary>The verdictDetails property</summary>
        public ChatMessagePolicyViolationVerdictDetailsTypes? VerdictDetails { get; set; }
        /// <summary>
        /// Instantiates a new chatMessagePolicyViolation and sets the default values.
        /// </summary>
        public ChatMessagePolicyViolation() {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static ChatMessagePolicyViolation CreateFromDiscriminatorValue(IParseNode parseNode) {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new ChatMessagePolicyViolation();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        public IDictionary<string, Action<IParseNode>> GetFieldDeserializers() {
            return new Dictionary<string, Action<IParseNode>> {
                {"dlpAction", n => { DlpAction = n.GetEnumValue<ChatMessagePolicyViolationDlpActionTypes>(); } },
                {"justificationText", n => { JustificationText = n.GetStringValue(); } },
                {"policyTip", n => { PolicyTip = n.GetObjectValue<ChatMessagePolicyViolationPolicyTip>(ChatMessagePolicyViolationPolicyTip.CreateFromDiscriminatorValue); } },
                {"userAction", n => { UserAction = n.GetEnumValue<ChatMessagePolicyViolationUserActionTypes>(); } },
                {"verdictDetails", n => { VerdictDetails = n.GetEnumValue<ChatMessagePolicyViolationVerdictDetailsTypes>(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public void Serialize(ISerializationWriter writer) {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteEnumValue<ChatMessagePolicyViolationDlpActionTypes>("dlpAction", DlpAction);
            writer.WriteStringValue("justificationText", JustificationText);
            writer.WriteObjectValue<ChatMessagePolicyViolationPolicyTip>("policyTip", PolicyTip);
            writer.WriteEnumValue<ChatMessagePolicyViolationUserActionTypes>("userAction", UserAction);
            writer.WriteEnumValue<ChatMessagePolicyViolationVerdictDetailsTypes>("verdictDetails", VerdictDetails);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
