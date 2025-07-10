public class AreaOfInterest : SmartEnum<AreaOfInterest>, ISmartEnum, IEquatable<SmartEnum<AreaOfInterest, int>>, IComparable<SmartEnum<AreaOfInterest, int>>
{
    public static readonly AreaOfInterest ReportReading = new(nameof(ReportReading), 1);
    public static readonly AreaOfInterest ReportWriting = new(nameof(ReportWriting), 2);
    public static readonly AreaOfInterest UtilizingTeleradiologyServices = new(nameof(UtilizingTeleradiologyServices), 3);
    public static readonly AreaOfInterest Radiology = new(nameof(Radiology), 4); // Add this line to define 'Radiology'  

    private AreaOfInterest(string name, int value) : base(name, value)
    {
    }
}
