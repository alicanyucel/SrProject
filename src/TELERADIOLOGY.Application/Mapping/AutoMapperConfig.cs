using AutoMapper;

namespace TELERADIOLOGY.Application
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
               
                cfg.AddProfile<MappingProfile>();
            });

           
            config.AssertConfigurationIsValid();

            return config.CreateMapper();
        }
    }
}