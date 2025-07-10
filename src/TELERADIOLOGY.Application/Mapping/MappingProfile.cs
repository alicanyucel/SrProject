using AutoMapper;
using TELERADIOLOGY.Application.Dtos.Company;
using TELERADIOLOGY.Application.Dtos.Hospital;
using TELERADIOLOGY.Application.Dtos.Permission;
using TELERADIOLOGY.Application.Dtos.User;
using TELERADIOLOGY.Application.Features.Companies.CreateCompany;
using TELERADIOLOGY.Application.Features.Companies.UpdateCompany;
using TELERADIOLOGY.Application.Features.DoctorSignatures.CreateDoctorSignature;
using TELERADIOLOGY.Application.Features.DoctorSignatures.UpdateDoctorSignature;
using TELERADIOLOGY.Application.Features.Hospitals.CreateHospital;
using TELERADIOLOGY.Application.Features.Hospitals.UpdateHospital;
using TELERADIOLOGY.Application.Features.Members.CreateMember;
using TELERADIOLOGY.Application.Features.Members.UpdateMember;
using TELERADIOLOGY.Application.Features.Permissions.CreatePermission;
using TELERADIOLOGY.Application.Features.Permissions.UpdatePermission;
using TELERADIOLOGY.Application.Features.Reports.CreateReport;
using TELERADIOLOGY.Application.Features.Reports.UpdateReport;
using TELERADIOLOGY.Application.Features.Templates.CreateTemplate;
using TELERADIOLOGY.Application.Features.Templates.UpdateTemplate;
using TELERADIOLOGY.Application.Features.Users.Createuser;
using TELERADIOLOGY.Application.Features.Users.UpdateUser;
using TELERADIOLOGY.Application.Features.Partitions.CreatePartition;
using TELERADIOLOGY.Domain.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // USER & ROLES
        CreateMap<AppUser, UserResultDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles));

        CreateMap<AppUserRole, UserRoleDto>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : string.Empty));

        CreateMap<CreateUserCommand, AppUser>()
            .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

        CreateMap<UpdateUserByIdCommand, AppUser>().ReverseMap();

        CreateMap<CreateUserCommand, AppUserRole>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.UserId, opt => opt.Ignore());

        // PERMISSION
        CreateMap<CreatePermissionCommand, Permission>()
            .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method.ToUpperInvariant()));
        CreateMap<UpdatePermissionCommand, Permission>()
            .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method.ToUpperInvariant()));
        CreateMap<Permission, PermissionDto>();

        // MEMBER
        CreateMap<UpdateMemberByIdCommand, Member>().ReverseMap();
        CreateMap<CreateMemberCommand, Member>()
            .ForMember(dest => dest.AreaOfInterest, opt => opt.MapFrom(src => src.AreaOfInterest))
            .ReverseMap(); 

        // DOCTOR SIGNATURE
        CreateMap<CreateDoctorSignatureCommand, DoctorSignature>().ReverseMap();
        CreateMap<UpdateDoctorSignatureByIdCommand, DoctorSignature>().ReverseMap();

        // TEMPLATE
        CreateMap<CreateTemplateCommand, Template>().ReverseMap();
        CreateMap<UpdateTemplateCommand, Template>().ReverseMap();

        // HOSPITAL
        CreateMap<CreateHospitalCommand, Hospital>().ReverseMap();
        CreateMap<UpdateHospitalCommand, Hospital>().ReverseMap();
        CreateMap<Hospital, HospitalResultDto>().ReverseMap();

        // REPORT
        CreateMap<CreateReportCommand, Report>().ReverseMap();
        CreateMap<UpdateReportCommand, Report>()
            .ForMember(dest => dest.ReportName, opt => opt.MapFrom(src => src.ReportName))
            .ForMember(dest => dest.Emergency, opt => opt.MapFrom(src => src.Emergency))
            .ForMember(dest => dest.ModalityType, opt => opt.MapFrom(src => src.ModalityType));
        CreateMap<Report, UpdateReportCommand>()
            .ForMember(dest => dest.ReportName, opt => opt.MapFrom(src => src.ReportName))
            .ForMember(dest => dest.Emergency, opt => opt.MapFrom(src => src.Emergency))
            .ForMember(dest => dest.ModalityType, opt => opt.MapFrom(src => src.ModalityType));
        CreateMap<AppUserRole, UserRoleDto>()
           .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
           .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : string.Empty));

        CreateMap<AppUser, UserResultDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.UserRoles))
            .MaxDepth(3)
            .PreserveReferences();
        // COMPANY
        CreateMap<Company, CompanyResultDto>().ReverseMap();
        CreateMap<CreateCompanyCommand, Company>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()));
        CreateMap<UpdateCompanyCommand, Company>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ReverseMap();

        // COMPANY USER
        CreateMap<CompanyUser, CompanyUserDto>()
            .ForMember(dest => dest.CompanyTitle, opt => opt.MapFrom(src => src.Company.CompanyTitle))
            .ForMember(dest => dest.CompanySmallTitle, opt => opt.MapFrom(src => src.Company.CompanySmallTitle))
            .ForMember(dest => dest.Representative, opt => opt.MapFrom(src => src.Company.Representative))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Company.PhoneNumber))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Company.Email));

        CreateMap<CreateCompanyUserCommand, CompanyUser>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // PARTITION
        CreateMap<CreatePartitionCommand, Partition>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false));
    }
}
