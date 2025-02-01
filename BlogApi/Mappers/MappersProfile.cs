using AutoMapper;
using BlogApi.Models.Dtos;
using BlogApi.Models.Entities;

namespace BlogApi.Mappers
{
    public class MappersProfile : Profile
    {
        public MappersProfile()
        {
            CreateMap<PostDto, Post>().ReverseMap();
        }

    }
}
