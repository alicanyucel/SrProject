using Ardalis.SmartEnum;

namespace TELERADIOLOGY.Domain.Enums;

public sealed class ApplicationStatus : SmartEnum<ApplicationStatus>
{
    public static readonly ApplicationStatus Approved = new(nameof(Approved), 1);
    public static readonly ApplicationStatus Unapproved = new(nameof(Unapproved), 2);
    public static readonly ApplicationStatus AdditionalInformationRequested = new(nameof(AdditionalInformationRequested), 3);
    public static readonly ApplicationStatus Rejected = new(nameof(Rejected), 4);

    private ApplicationStatus(string name, int value) : base(name, value)
    {
    }
}