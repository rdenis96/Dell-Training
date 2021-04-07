using AutoMapper;
using RabbitMQPlayground.Domain.Users;

namespace RabbitMQPlayground.API.Configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IUserWrapper, User>();
        }
    }
}