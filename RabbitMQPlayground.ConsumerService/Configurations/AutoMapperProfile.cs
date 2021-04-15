using AutoMapper;
using RabbitMQPlayground.Domain.Users;

namespace RabbitMQPlayground.ConsumerService.Configurations
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IUserWrapper, User>();
        }
    }
}