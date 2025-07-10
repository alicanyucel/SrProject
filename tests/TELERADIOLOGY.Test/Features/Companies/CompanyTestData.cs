using TELERADIOLOGY.Application.Features.Companies.CreateCompany;
using TELERADIOLOGY.Application.Features.Companies.DeleteCompany;
using TELERADIOLOGY.Application.Features.Companies.UpdateCompany;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Test.Features.Companies;

public static class CompanyTestData
{
    #region Default Values (DRY - Single source of truth)

    private static readonly string DefaultSmallTitle = "Test Ltd";
    private static readonly string DefaultTitle = "Test Company Ltd";
    private static readonly string DefaultEmail = "test@company.com";
    private static readonly string DefaultTaxNo = "1234567890";
    private static readonly int ValidCompanyType = 1;
    private static readonly int InvalidCompanyType = -999;

    #endregion

    #region Entity Creation (Simple & Direct)

    public static Company CreateCompany(Guid? id = null)
    {
        return new Company
        {
            Id = id ?? Guid.NewGuid(),
            CompanySmallTitle = DefaultSmallTitle,
            CompanyTitle = DefaultTitle,
            PhoneNumber = "05551234567",
            Email = DefaultEmail,
            Address = "Test Address",
            TaxNo = DefaultTaxNo,
            TaxOffice = "Test Tax Office",
            Representative = "Test Representative",
            WebSite = "www.test.com",
            City = "İstanbul",
            District = "Test District",
            Status = true,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
    }

   
    public static Company CreateCompanyForDelete()
    {
        return CreateCompany().With(c => c.IsDeleted = false);
    }

   
    public static Company CreateDeletedCompany()
    {
        return CreateCompany().With(c => c.IsDeleted = true);
    }

    #endregion

    #region Command Creation (KISS)

  
    public static CreateCompanyCommand CreateValidCommand()
    {
        return new CreateCompanyCommand(
            CompanySmallTitle: DefaultSmallTitle,
            CompanyTitle: DefaultTitle,
            PhoneNumber: "05551234567",
            Email: DefaultEmail,
            Address: "Test Address",
            TaxNo: DefaultTaxNo,
            TaxOffice: "Test Tax Office",
            Representative: "Test Representative",
            WebSite: "www.test.com",
            City: "İstanbul",
            District: "Test District",
            Status: true,
            CompanyTypeValue: ValidCompanyType
        );
    }

   
    public static CreateCompanyCommand CreateInvalidCommand()
    {
        return CreateValidCommand() with { CompanyTypeValue = InvalidCompanyType };
    }

   
    public static DeleteCompanyByIdCommand CreateDeleteCommand(Guid? companyId = null)
    {
        return new DeleteCompanyByIdCommand(companyId ?? Guid.NewGuid());
    }

    public static DeleteCompanyByIdCommand CreateDeleteCommandWithEmptyId()
    {
        return new DeleteCompanyByIdCommand(Guid.Empty);
    }

    #endregion

    #region Update Command Creation

   
    public static UpdateCompanyCommand CreateValidUpdateCommand(Guid? companyId = null)
    {
        return new UpdateCompanyCommand(
            Id: companyId ?? Guid.NewGuid(),
            CompanySmallTitle: "Updated Ltd",
            CompanyTitle: "Updated Company Ltd",
            PhoneNumber: "05559876543",
            Email: "updated@company.com",
            Address: "Updated Address",
            TaxNo: "9876543210",
            TaxOffice: "Updated Tax Office",
            Representative: "Updated Representative",
            WebSite: "www.updated.com",
            City: "Ankara",
            District: "Updated District",
            Status: true,
            CompanyTypeValue: ValidCompanyType
        );
    }

   
    public static UpdateCompanyCommand CreateInvalidUpdateCommand(Guid? companyId = null)
    {
        return CreateValidUpdateCommand(companyId) with { CompanyTypeValue = InvalidCompanyType };
    }

   
    public static UpdateCompanyCommand CreateUpdateCommandForNonExistentCompany()
    {
        return CreateValidUpdateCommand(Guid.Parse("99999999-9999-9999-9999-999999999999"));
    }

    #endregion

    #region Update Command Extensions

    
    public static UpdateCompanyCommand WithId(this UpdateCompanyCommand command, Guid id)
    {
        return command with { Id = id };
    }

   
    public static UpdateCompanyCommand WithTitle(this UpdateCompanyCommand command, string title)
    {
        return command with { CompanyTitle = title };
    }

   
    public static UpdateCompanyCommand WithCompanyType(this UpdateCompanyCommand command, int companyType)
    {
        return command with { CompanyTypeValue = companyType };
    }

   
    public static UpdateCompanyCommand WithStatus(this UpdateCompanyCommand command, bool status)
    {
        return command with { Status = status };
    }

    #endregion

    #region Company Entity for Update Scenarios

   
    public static Company CreateCompanyForUpdate(Guid? id = null)
    {
        return CreateCompany(id).With(c =>
        {
            c.IsDeleted = false;
            c.Status = true;
        });
    }

   
    public static Company CreateDeletedCompanyForUpdate(Guid? id = null)
    {
        return CreateCompany(id).With(c =>
        {
            c.IsDeleted = true;
            c.Status = false;
        });
    }

    
    public static Company CreateCompanyWithState(Guid id, bool isDeleted = false, bool status = true)
    {
        return CreateCompany(id).With(c =>
        {
            c.IsDeleted = isDeleted;
            c.Status = status;
        });
    }

    #endregion

    #region Quick Modifications (Fluent but Simple)

   
    public static T With<T>(this T obj, Action<T> action) where T : class
    {
        action(obj);
        return obj;
    }

   
    public static CreateCompanyCommand WithTitle(this CreateCompanyCommand command, string title)
    {
        return command with { CompanyTitle = title };
    }

    public static CreateCompanyCommand WithCompanyType(this CreateCompanyCommand command, int companyType)
    {
        return command with { CompanyTypeValue = companyType };
    }

    public static CreateCompanyCommand WithEmail(this CreateCompanyCommand command, string email)
    {
        return command with { Email = email };
    }

    #endregion
}
