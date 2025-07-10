using Ardalis.SmartEnum;

namespace TELERADIOLOGY.Domain.Enums;

public class CompanyType : SmartEnum<CompanyType>
{
    public static readonly CompanyType Hospital = new("Hospital", 1);
    public static readonly CompanyType Other = new("Other", 2);
    public static readonly CompanyType RadiologyCenter = new("RadiologyCenter", 3);
    public static readonly CompanyType DiagnosticCenter = new("DiagnosticCenter ", 4);
    private CompanyType(string name, int value) : base(name, value)
    {
    }
}

