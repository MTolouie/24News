using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News_Models.DTOs;
using News_DataLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace News_Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<NewsDTO, News>().ReverseMap();
            CreateMap<ApplicationUserDTO, ApplicationUser>().ReverseMap();
            CreateMap<IdentityRoleDTO, IdentityRole>().ReverseMap();
            CreateMap<PendingNews, PendingNewsDTO>().ReverseMap();
            CreateMap<Comment,CommentDTO>().ReverseMap();

           
        }
    }
}
