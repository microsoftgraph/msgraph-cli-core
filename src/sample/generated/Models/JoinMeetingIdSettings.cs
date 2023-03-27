using Microsoft.Kiota.Abstractions.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace ApiSdk.Models {
    public class JoinMeetingIdSettings : IAdditionalDataHolder, IParsable {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>Indicates whether a passcode is required to join a meeting when using joinMeetingId. Optional.</summary>
        public bool? IsPasscodeRequired { get; set; }
        /// <summary>The meeting ID to be used to join a meeting. Optional. Read-only.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? JoinMeetingId { get; set; }
#nullable restore
#else
        public string JoinMeetingId { get; set; }
#endif
        /// <summary>The passcode to join a meeting.  Optional. Read-only.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Passcode { get; set; }
#nullable restore
#else
        public string Passcode { get; set; }
#endif
        /// <summary>
        /// Instantiates a new joinMeetingIdSettings and sets the default values.
        /// </summary>
        public JoinMeetingIdSettings() {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static JoinMeetingIdSettings CreateFromDiscriminatorValue(IParseNode parseNode) {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new JoinMeetingIdSettings();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        public IDictionary<string, Action<IParseNode>> GetFieldDeserializers() {
            return new Dictionary<string, Action<IParseNode>> {
                {"isPasscodeRequired", n => { IsPasscodeRequired = n.GetBoolValue(); } },
                {"joinMeetingId", n => { JoinMeetingId = n.GetStringValue(); } },
                {"passcode", n => { Passcode = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public void Serialize(ISerializationWriter writer) {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteBoolValue("isPasscodeRequired", IsPasscodeRequired);
            writer.WriteStringValue("joinMeetingId", JoinMeetingId);
            writer.WriteStringValue("passcode", Passcode);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}