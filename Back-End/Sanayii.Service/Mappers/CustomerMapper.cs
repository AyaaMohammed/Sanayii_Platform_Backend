using AutoMapper;
using Sanayii.Core.DTOs;
using Sanayii.Core.DTOs.AccountDTOs;
using Sanayii.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanayii.Service.Mappers
{
    public class CustomerMapper : Profile
    {
        public CustomerMapper()
        {
            CreateMap<Customer, RegisterDto>().AfterMap((src, dst) =>
            {
                src.UserName = dst.Username;
            }).ReverseMap();
            CreateMap<Review, ReviewDTO>().ReverseMap();
        }
    }
}

