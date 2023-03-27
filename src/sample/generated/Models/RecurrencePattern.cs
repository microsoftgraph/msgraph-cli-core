using Microsoft.Kiota.Abstractions.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace ApiSdk.Models {
    public class RecurrencePattern : IAdditionalDataHolder, IParsable {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The day of the month on which the event occurs. Required if type is absoluteMonthly or absoluteYearly.</summary>
        public int? DayOfMonth { get; set; }
        /// <summary>A collection of the days of the week on which the event occurs. The possible values are: sunday, monday, tuesday, wednesday, thursday, friday, saturday. If type is relativeMonthly or relativeYearly, and daysOfWeek specifies more than one day, the event falls on the first day that satisfies the pattern.  Required if type is weekly, relativeMonthly, or relativeYearly.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<DayOfWeekObject?>? DaysOfWeek { get; set; }
#nullable restore
#else
        public List<DayOfWeekObject?> DaysOfWeek { get; set; }
#endif
        /// <summary>The firstDayOfWeek property</summary>
        public DayOfWeekObject? FirstDayOfWeek { get; set; }
        /// <summary>The index property</summary>
        public WeekIndex? Index { get; set; }
        /// <summary>The number of units between occurrences, where units can be in days, weeks, months, or years, depending on the type. Required.</summary>
        public int? Interval { get; set; }
        /// <summary>The month in which the event occurs.  This is a number from 1 to 12.</summary>
        public int? Month { get; set; }
        /// <summary>The type property</summary>
        public RecurrencePatternType? Type { get; set; }
        /// <summary>
        /// Instantiates a new recurrencePattern and sets the default values.
        /// </summary>
        public RecurrencePattern() {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static RecurrencePattern CreateFromDiscriminatorValue(IParseNode parseNode) {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new RecurrencePattern();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        public IDictionary<string, Action<IParseNode>> GetFieldDeserializers() {
            return new Dictionary<string, Action<IParseNode>> {
                {"dayOfMonth", n => { DayOfMonth = n.GetIntValue(); } },
                {"daysOfWeek", n => { DaysOfWeek = n.GetCollectionOfEnumValues<DayOfWeekObject>()?.ToList(); } },
                {"firstDayOfWeek", n => { FirstDayOfWeek = n.GetEnumValue<DayOfWeekObject>(); } },
                {"index", n => { Index = n.GetEnumValue<WeekIndex>(); } },
                {"interval", n => { Interval = n.GetIntValue(); } },
                {"month", n => { Month = n.GetIntValue(); } },
                {"type", n => { Type = n.GetEnumValue<RecurrencePatternType>(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public void Serialize(ISerializationWriter writer) {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteIntValue("dayOfMonth", DayOfMonth);
            writer.WriteCollectionOfEnumValues<DayOfWeekObject>("daysOfWeek", DaysOfWeek);
            writer.WriteEnumValue<DayOfWeekObject>("firstDayOfWeek", FirstDayOfWeek);
            writer.WriteEnumValue<WeekIndex>("index", Index);
            writer.WriteIntValue("interval", Interval);
            writer.WriteIntValue("month", Month);
            writer.WriteEnumValue<RecurrencePatternType>("type", Type);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}