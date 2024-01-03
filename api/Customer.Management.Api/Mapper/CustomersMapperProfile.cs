using AutoMapper;
using Customers.Management.Api.Dtos;
using Customers.Management.Repository.Entities;

namespace Customers.Management.Api.Mapper
{
    public class CustomersMapperProfile : Profile
    {
        public CustomersMapperProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
        }
    }
}
