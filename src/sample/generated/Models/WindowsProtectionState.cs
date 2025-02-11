// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace ApiSdk.Models
{
    /// <summary>
    /// Device protection status entity.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class WindowsProtectionState : global::ApiSdk.Models.Entity, IParsable
    {
        /// <summary>Current anti malware version</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? AntiMalwareVersion { get; set; }
#nullable restore
#else
        public string AntiMalwareVersion { get; set; }
#endif
        /// <summary>Device malware list</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::ApiSdk.Models.WindowsDeviceMalwareState>? DetectedMalwareState { get; set; }
#nullable restore
#else
        public List<global::ApiSdk.Models.WindowsDeviceMalwareState> DetectedMalwareState { get; set; }
#endif
        /// <summary>Computer endpoint protection state</summary>
        public global::ApiSdk.Models.WindowsDeviceHealthState? DeviceState { get; set; }
        /// <summary>Current endpoint protection engine&apos;s version</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? EngineVersion { get; set; }
#nullable restore
#else
        public string EngineVersion { get; set; }
#endif
        /// <summary>When TRUE indicates full scan is overdue, when FALSE indicates full scan is not overdue. Defaults to setting on client device.</summary>
        public bool? FullScanOverdue { get; set; }
        /// <summary>When TRUE indicates full scan is required, when FALSE indicates full scan is not required. Defaults to setting on client device.</summary>
        public bool? FullScanRequired { get; set; }
        /// <summary>When TRUE indicates the device is a virtual machine, when FALSE indicates the device is not a virtual machine. Defaults to setting on client device.</summary>
        public bool? IsVirtualMachine { get; set; }
        /// <summary>Last quick scan datetime</summary>
        public DateTimeOffset? LastFullScanDateTime { get; set; }
        /// <summary>Last full scan signature version</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? LastFullScanSignatureVersion { get; set; }
#nullable restore
#else
        public string LastFullScanSignatureVersion { get; set; }
#endif
        /// <summary>Last quick scan datetime</summary>
        public DateTimeOffset? LastQuickScanDateTime { get; set; }
        /// <summary>Last quick scan signature version</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? LastQuickScanSignatureVersion { get; set; }
#nullable restore
#else
        public string LastQuickScanSignatureVersion { get; set; }
#endif
        /// <summary>Last device health status reported time</summary>
        public DateTimeOffset? LastReportedDateTime { get; set; }
        /// <summary>When TRUE indicates anti malware is enabled when FALSE indicates anti malware is not enabled.</summary>
        public bool? MalwareProtectionEnabled { get; set; }
        /// <summary>When TRUE indicates network inspection system enabled, when FALSE indicates network inspection system is not enabled. Defaults to setting on client device.</summary>
        public bool? NetworkInspectionSystemEnabled { get; set; }
        /// <summary>Product Status of Windows Defender</summary>
        public global::ApiSdk.Models.WindowsDefenderProductStatus? ProductStatus { get; set; }
        /// <summary>When TRUE indicates quick scan is overdue, when FALSE indicates quick scan is not overdue. Defaults to setting on client device.</summary>
        public bool? QuickScanOverdue { get; set; }
        /// <summary>When TRUE indicates real time protection is enabled, when FALSE indicates real time protection is not enabled. Defaults to setting on client device.</summary>
        public bool? RealTimeProtectionEnabled { get; set; }
        /// <summary>When TRUE indicates reboot is required, when FALSE indicates when TRUE indicates reboot is not required. Defaults to setting on client device.</summary>
        public bool? RebootRequired { get; set; }
        /// <summary>When TRUE indicates signature is out of date, when FALSE indicates signature is not out of date. Defaults to setting on client device.</summary>
        public bool? SignatureUpdateOverdue { get; set; }
        /// <summary>Current malware definitions version</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? SignatureVersion { get; set; }
#nullable restore
#else
        public string SignatureVersion { get; set; }
#endif
        /// <summary>When TRUE indicates the Windows Defender tamper protection feature is enabled, when FALSE indicates the Windows Defender tamper protection feature is not enabled. Defaults to setting on client device.</summary>
        public bool? TamperProtectionEnabled { get; set; }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::ApiSdk.Models.WindowsProtectionState"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static new global::ApiSdk.Models.WindowsProtectionState CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::ApiSdk.Models.WindowsProtectionState();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public override IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>(base.GetFieldDeserializers())
            {
                { "antiMalwareVersion", n => { AntiMalwareVersion = n.GetStringValue(); } },
                { "detectedMalwareState", n => { DetectedMalwareState = n.GetCollectionOfObjectValues<global::ApiSdk.Models.WindowsDeviceMalwareState>(global::ApiSdk.Models.WindowsDeviceMalwareState.CreateFromDiscriminatorValue)?.AsList(); } },
                { "deviceState", n => { DeviceState = n.GetEnumValue<global::ApiSdk.Models.WindowsDeviceHealthState>(); } },
                { "engineVersion", n => { EngineVersion = n.GetStringValue(); } },
                { "fullScanOverdue", n => { FullScanOverdue = n.GetBoolValue(); } },
                { "fullScanRequired", n => { FullScanRequired = n.GetBoolValue(); } },
                { "isVirtualMachine", n => { IsVirtualMachine = n.GetBoolValue(); } },
                { "lastFullScanDateTime", n => { LastFullScanDateTime = n.GetDateTimeOffsetValue(); } },
                { "lastFullScanSignatureVersion", n => { LastFullScanSignatureVersion = n.GetStringValue(); } },
                { "lastQuickScanDateTime", n => { LastQuickScanDateTime = n.GetDateTimeOffsetValue(); } },
                { "lastQuickScanSignatureVersion", n => { LastQuickScanSignatureVersion = n.GetStringValue(); } },
                { "lastReportedDateTime", n => { LastReportedDateTime = n.GetDateTimeOffsetValue(); } },
                { "malwareProtectionEnabled", n => { MalwareProtectionEnabled = n.GetBoolValue(); } },
                { "networkInspectionSystemEnabled", n => { NetworkInspectionSystemEnabled = n.GetBoolValue(); } },
                { "productStatus", n => { ProductStatus = n.GetEnumValue<global::ApiSdk.Models.WindowsDefenderProductStatus>(); } },
                { "quickScanOverdue", n => { QuickScanOverdue = n.GetBoolValue(); } },
                { "realTimeProtectionEnabled", n => { RealTimeProtectionEnabled = n.GetBoolValue(); } },
                { "rebootRequired", n => { RebootRequired = n.GetBoolValue(); } },
                { "signatureUpdateOverdue", n => { SignatureUpdateOverdue = n.GetBoolValue(); } },
                { "signatureVersion", n => { SignatureVersion = n.GetStringValue(); } },
                { "tamperProtectionEnabled", n => { TamperProtectionEnabled = n.GetBoolValue(); } },
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
            writer.WriteStringValue("antiMalwareVersion", AntiMalwareVersion);
            writer.WriteCollectionOfObjectValues<global::ApiSdk.Models.WindowsDeviceMalwareState>("detectedMalwareState", DetectedMalwareState);
            writer.WriteEnumValue<global::ApiSdk.Models.WindowsDeviceHealthState>("deviceState", DeviceState);
            writer.WriteStringValue("engineVersion", EngineVersion);
            writer.WriteBoolValue("fullScanOverdue", FullScanOverdue);
            writer.WriteBoolValue("fullScanRequired", FullScanRequired);
            writer.WriteBoolValue("isVirtualMachine", IsVirtualMachine);
            writer.WriteDateTimeOffsetValue("lastFullScanDateTime", LastFullScanDateTime);
            writer.WriteStringValue("lastFullScanSignatureVersion", LastFullScanSignatureVersion);
            writer.WriteDateTimeOffsetValue("lastQuickScanDateTime", LastQuickScanDateTime);
            writer.WriteStringValue("lastQuickScanSignatureVersion", LastQuickScanSignatureVersion);
            writer.WriteDateTimeOffsetValue("lastReportedDateTime", LastReportedDateTime);
            writer.WriteBoolValue("malwareProtectionEnabled", MalwareProtectionEnabled);
            writer.WriteBoolValue("networkInspectionSystemEnabled", NetworkInspectionSystemEnabled);
            writer.WriteEnumValue<global::ApiSdk.Models.WindowsDefenderProductStatus>("productStatus", ProductStatus);
            writer.WriteBoolValue("quickScanOverdue", QuickScanOverdue);
            writer.WriteBoolValue("realTimeProtectionEnabled", RealTimeProtectionEnabled);
            writer.WriteBoolValue("rebootRequired", RebootRequired);
            writer.WriteBoolValue("signatureUpdateOverdue", SignatureUpdateOverdue);
            writer.WriteStringValue("signatureVersion", SignatureVersion);
            writer.WriteBoolValue("tamperProtectionEnabled", TamperProtectionEnabled);
        }
    }
}
#pragma warning restore CS0618
