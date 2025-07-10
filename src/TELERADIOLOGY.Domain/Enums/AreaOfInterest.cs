using Ardalis.SmartEnum;

namespace TELERADIOLOGY.Domain.Enums;

public class AreaOfInterest : SmartEnum<AreaOfInterest>
{
    public static readonly AreaOfInterest ReportReading = new("ReportReading", 1);
    public static readonly AreaOfInterest ReportWrinting = new("ReportWrinting", 2);
    public static readonly AreaOfInterest UtilizingTeleradiologyServices = new("UtilizingTeleradiologyServices", 3);

    private AreaOfInterest(string name, int value) : base(name, value)
    {
    }
}
