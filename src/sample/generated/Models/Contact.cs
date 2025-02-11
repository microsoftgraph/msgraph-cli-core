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
    public partial class Contact : global::ApiSdk.Models.OutlookItem, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>The name of the contact&apos;s assistant.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? AssistantName { get; set; }
#nullable restore
#else
        public string AssistantName { get; set; }
#endif
        /// <summary>The contact&apos;s birthday. The Timestamp type represents date and time information using ISO 8601 format and is always in UTC time. For example, midnight UTC on Jan 1, 2014 is 2014-01-01T00:00:00Z</summary>
        public DateTimeOffset? Birthday { get; set; }
        /// <summary>The businessAddress property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::ApiSdk.Models.PhysicalAddress? BusinessAddress { get; set; }
#nullable restore
#else
        public global::ApiSdk.Models.PhysicalAddress BusinessAddress { get; set; }
#endif
        /// <summary>The business home page of the contact.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? BusinessHomePage { get; set; }
#nullable restore
#else
        public string BusinessHomePage { get; set; }
#endif
        /// <summary>The contact&apos;s business phone numbers.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<string>? BusinessPhones { get; set; }
#nullable restore
#else
        public List<string> BusinessPhones { get; set; }
#endif
        /// <summary>The names of the contact&apos;s children.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<string>? Children { get; set; }
#nullable restore
#else
        public List<string> Children { get; set; }
#endif
        /// <summary>The name of the contact&apos;s company.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? CompanyName { get; set; }
#nullable restore
#else
        public string CompanyName { get; set; }
#endif
        /// <summary>The contact&apos;s department.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Department { get; set; }
#nullable restore
#else
        public string Department { get; set; }
#endif
        /// <summary>The contact&apos;s display name. You can specify the display name in a create or update operation. Note that later updates to other properties may cause an automatically generated value to overwrite the displayName value you have specified. To preserve a pre-existing value, always include it as displayName in an update operation.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? DisplayName { get; set; }
#nullable restore
#else
        public string DisplayName { get; set; }
#endif
        /// <summary>The contact&apos;s email addresses.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::ApiSdk.Models.EmailAddress>? EmailAddresses { get; set; }
#nullable restore
#else
        public List<global::ApiSdk.Models.EmailAddress> EmailAddresses { get; set; }
#endif
        /// <summary>The collection of open extensions defined for the contact. Read-only. Nullable.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::ApiSdk.Models.Extension>? Extensions { get; set; }
#nullable restore
#else
        public List<global::ApiSdk.Models.Extension> Extensions { get; set; }
#endif
        /// <summary>The name the contact is filed under.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? FileAs { get; set; }
#nullable restore
#else
        public string FileAs { get; set; }
#endif
        /// <summary>The contact&apos;s suffix.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Generation { get; set; }
#nullable restore
#else
        public string Generation { get; set; }
#endif
        /// <summary>The contact&apos;s given name.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? GivenName { get; set; }
#nullable restore
#else
        public string GivenName { get; set; }
#endif
        /// <summary>The homeAddress property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::ApiSdk.Models.PhysicalAddress? HomeAddress { get; set; }
#nullable restore
#else
        public global::ApiSdk.Models.PhysicalAddress HomeAddress { get; set; }
#endif
        /// <summary>The contact&apos;s home phone numbers.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<string>? HomePhones { get; set; }
#nullable restore
#else
        public List<string> HomePhones { get; set; }
#endif
        /// <summary>The contact&apos;s instant messaging (IM) addresses.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<string>? ImAddresses { get; set; }
#nullable restore
#else
        public List<string> ImAddresses { get; set; }
#endif
        /// <summary>The contact&apos;s initials.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Initials { get; set; }
#nullable restore
#else
        public string Initials { get; set; }
#endif
        /// <summary>The contact’s job title.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? JobTitle { get; set; }
#nullable restore
#else
        public string JobTitle { get; set; }
#endif
        /// <summary>The name of the contact&apos;s manager.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Manager { get; set; }
#nullable restore
#else
        public string Manager { get; set; }
#endif
        /// <summary>The contact&apos;s middle name.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? MiddleName { get; set; }
#nullable restore
#else
        public string MiddleName { get; set; }
#endif
        /// <summary>The contact&apos;s mobile phone number.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? MobilePhone { get; set; }
#nullable restore
#else
        public string MobilePhone { get; set; }
#endif
        /// <summary>The collection of multi-value extended properties defined for the contact. Read-only. Nullable.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::ApiSdk.Models.MultiValueLegacyExtendedProperty>? MultiValueExtendedProperties { get; set; }
#nullable restore
#else
        public List<global::ApiSdk.Models.MultiValueLegacyExtendedProperty> MultiValueExtendedProperties { get; set; }
#endif
        /// <summary>The contact&apos;s nickname.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? NickName { get; set; }
#nullable restore
#else
        public string NickName { get; set; }
#endif
        /// <summary>The location of the contact&apos;s office.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? OfficeLocation { get; set; }
#nullable restore
#else
        public string OfficeLocation { get; set; }
#endif
        /// <summary>The otherAddress property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::ApiSdk.Models.PhysicalAddress? OtherAddress { get; set; }
#nullable restore
#else
        public global::ApiSdk.Models.PhysicalAddress OtherAddress { get; set; }
#endif
        /// <summary>The ID of the contact&apos;s parent folder.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? ParentFolderId { get; set; }
#nullable restore
#else
        public string ParentFolderId { get; set; }
#endif
        /// <summary>The user&apos;s notes about the contact.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? PersonalNotes { get; set; }
#nullable restore
#else
        public string PersonalNotes { get; set; }
#endif
        /// <summary>The photo property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::ApiSdk.Models.ProfilePhoto? Photo { get; set; }
#nullable restore
#else
        public global::ApiSdk.Models.ProfilePhoto Photo { get; set; }
#endif
        /// <summary>The contact&apos;s profession.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Profession { get; set; }
#nullable restore
#else
        public string Profession { get; set; }
#endif
        /// <summary>The collection of single-value extended properties defined for the contact. Read-only. Nullable.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::ApiSdk.Models.SingleValueLegacyExtendedProperty>? SingleValueExtendedProperties { get; set; }
#nullable restore
#else
        public List<global::ApiSdk.Models.SingleValueLegacyExtendedProperty> SingleValueExtendedProperties { get; set; }
#endif
        /// <summary>The name of the contact&apos;s spouse/partner.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? SpouseName { get; set; }
#nullable restore
#else
        public string SpouseName { get; set; }
#endif
        /// <summary>The contact&apos;s surname.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Surname { get; set; }
#nullable restore
#else
        public string Surname { get; set; }
#endif
        /// <summary>The contact&apos;s title.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Title { get; set; }
#nullable restore
#else
        public string Title { get; set; }
#endif
        /// <summary>The phonetic Japanese company name of the contact.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? YomiCompanyName { get; set; }
#nullable restore
#else
        public string YomiCompanyName { get; set; }
#endif
        /// <summary>The phonetic Japanese given name (first name) of the contact.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? YomiGivenName { get; set; }
#nullable restore
#else
        public string YomiGivenName { get; set; }
#endif
        /// <summary>The phonetic Japanese surname (last name)  of the contact.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? YomiSurname { get; set; }
#nullable restore
#else
        public string YomiSurname { get; set; }
#endif
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::ApiSdk.Models.Contact"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static new global::ApiSdk.Models.Contact CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::ApiSdk.Models.Contact();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public override IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>(base.GetFieldDeserializers())
            {
                { "assistantName", n => { AssistantName = n.GetStringValue(); } },
                { "birthday", n => { Birthday = n.GetDateTimeOffsetValue(); } },
                { "businessAddress", n => { BusinessAddress = n.GetObjectValue<global::ApiSdk.Models.PhysicalAddress>(global::ApiSdk.Models.PhysicalAddress.CreateFromDiscriminatorValue); } },
                { "businessHomePage", n => { BusinessHomePage = n.GetStringValue(); } },
                { "businessPhones", n => { BusinessPhones = n.GetCollectionOfPrimitiveValues<string>()?.AsList(); } },
                { "children", n => { Children = n.GetCollectionOfPrimitiveValues<string>()?.AsList(); } },
                { "companyName", n => { CompanyName = n.GetStringValue(); } },
                { "department", n => { Department = n.GetStringValue(); } },
                { "displayName", n => { DisplayName = n.GetStringValue(); } },
                { "emailAddresses", n => { EmailAddresses = n.GetCollectionOfObjectValues<global::ApiSdk.Models.EmailAddress>(global::ApiSdk.Models.EmailAddress.CreateFromDiscriminatorValue)?.AsList(); } },
                { "extensions", n => { Extensions = n.GetCollectionOfObjectValues<global::ApiSdk.Models.Extension>(global::ApiSdk.Models.Extension.CreateFromDiscriminatorValue)?.AsList(); } },
                { "fileAs", n => { FileAs = n.GetStringValue(); } },
                { "generation", n => { Generation = n.GetStringValue(); } },
                { "givenName", n => { GivenName = n.GetStringValue(); } },
                { "homeAddress", n => { HomeAddress = n.GetObjectValue<global::ApiSdk.Models.PhysicalAddress>(global::ApiSdk.Models.PhysicalAddress.CreateFromDiscriminatorValue); } },
                { "homePhones", n => { HomePhones = n.GetCollectionOfPrimitiveValues<string>()?.AsList(); } },
                { "imAddresses", n => { ImAddresses = n.GetCollectionOfPrimitiveValues<string>()?.AsList(); } },
                { "initials", n => { Initials = n.GetStringValue(); } },
                { "jobTitle", n => { JobTitle = n.GetStringValue(); } },
                { "manager", n => { Manager = n.GetStringValue(); } },
                { "middleName", n => { MiddleName = n.GetStringValue(); } },
                { "mobilePhone", n => { MobilePhone = n.GetStringValue(); } },
                { "multiValueExtendedProperties", n => { MultiValueExtendedProperties = n.GetCollectionOfObjectValues<global::ApiSdk.Models.MultiValueLegacyExtendedProperty>(global::ApiSdk.Models.MultiValueLegacyExtendedProperty.CreateFromDiscriminatorValue)?.AsList(); } },
                { "nickName", n => { NickName = n.GetStringValue(); } },
                { "officeLocation", n => { OfficeLocation = n.GetStringValue(); } },
                { "otherAddress", n => { OtherAddress = n.GetObjectValue<global::ApiSdk.Models.PhysicalAddress>(global::ApiSdk.Models.PhysicalAddress.CreateFromDiscriminatorValue); } },
                { "parentFolderId", n => { ParentFolderId = n.GetStringValue(); } },
                { "personalNotes", n => { PersonalNotes = n.GetStringValue(); } },
                { "photo", n => { Photo = n.GetObjectValue<global::ApiSdk.Models.ProfilePhoto>(global::ApiSdk.Models.ProfilePhoto.CreateFromDiscriminatorValue); } },
                { "profession", n => { Profession = n.GetStringValue(); } },
                { "singleValueExtendedProperties", n => { SingleValueExtendedProperties = n.GetCollectionOfObjectValues<global::ApiSdk.Models.SingleValueLegacyExtendedProperty>(global::ApiSdk.Models.SingleValueLegacyExtendedProperty.CreateFromDiscriminatorValue)?.AsList(); } },
                { "spouseName", n => { SpouseName = n.GetStringValue(); } },
                { "surname", n => { Surname = n.GetStringValue(); } },
                { "title", n => { Title = n.GetStringValue(); } },
                { "yomiCompanyName", n => { YomiCompanyName = n.GetStringValue(); } },
                { "yomiGivenName", n => { YomiGivenName = n.GetStringValue(); } },
                { "yomiSurname", n => { YomiSurname = n.GetStringValue(); } },
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
            writer.WriteStringValue("assistantName", AssistantName);
            writer.WriteDateTimeOffsetValue("birthday", Birthday);
            writer.WriteObjectValue<global::ApiSdk.Models.PhysicalAddress>("businessAddress", BusinessAddress);
            writer.WriteStringValue("businessHomePage", BusinessHomePage);
            writer.WriteCollectionOfPrimitiveValues<string>("businessPhones", BusinessPhones);
            writer.WriteCollectionOfPrimitiveValues<string>("children", Children);
            writer.WriteStringValue("companyName", CompanyName);
            writer.WriteStringValue("department", Department);
            writer.WriteStringValue("displayName", DisplayName);
            writer.WriteCollectionOfObjectValues<global::ApiSdk.Models.EmailAddress>("emailAddresses", EmailAddresses);
            writer.WriteCollectionOfObjectValues<global::ApiSdk.Models.Extension>("extensions", Extensions);
            writer.WriteStringValue("fileAs", FileAs);
            writer.WriteStringValue("generation", Generation);
            writer.WriteStringValue("givenName", GivenName);
            writer.WriteObjectValue<global::ApiSdk.Models.PhysicalAddress>("homeAddress", HomeAddress);
            writer.WriteCollectionOfPrimitiveValues<string>("homePhones", HomePhones);
            writer.WriteCollectionOfPrimitiveValues<string>("imAddresses", ImAddresses);
            writer.WriteStringValue("initials", Initials);
            writer.WriteStringValue("jobTitle", JobTitle);
            writer.WriteStringValue("manager", Manager);
            writer.WriteStringValue("middleName", MiddleName);
            writer.WriteStringValue("mobilePhone", MobilePhone);
            writer.WriteCollectionOfObjectValues<global::ApiSdk.Models.MultiValueLegacyExtendedProperty>("multiValueExtendedProperties", MultiValueExtendedProperties);
            writer.WriteStringValue("nickName", NickName);
            writer.WriteStringValue("officeLocation", OfficeLocation);
            writer.WriteObjectValue<global::ApiSdk.Models.PhysicalAddress>("otherAddress", OtherAddress);
            writer.WriteStringValue("parentFolderId", ParentFolderId);
            writer.WriteStringValue("personalNotes", PersonalNotes);
            writer.WriteObjectValue<global::ApiSdk.Models.ProfilePhoto>("photo", Photo);
            writer.WriteStringValue("profession", Profession);
            writer.WriteCollectionOfObjectValues<global::ApiSdk.Models.SingleValueLegacyExtendedProperty>("singleValueExtendedProperties", SingleValueExtendedProperties);
            writer.WriteStringValue("spouseName", SpouseName);
            writer.WriteStringValue("surname", Surname);
            writer.WriteStringValue("title", Title);
            writer.WriteStringValue("yomiCompanyName", YomiCompanyName);
            writer.WriteStringValue("yomiGivenName", YomiGivenName);
            writer.WriteStringValue("yomiSurname", YomiSurname);
        }
    }
}
#pragma warning restore CS0618
