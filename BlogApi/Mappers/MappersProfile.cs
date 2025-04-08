using AutoMapper;
using BlogApi.Models.Dtos;
using BlogApi.Models.Entities;
using Newtonsoft.Json;

namespace BlogApi.Mappers
{
    public class MappersProfile : Profile
    {
        public MappersProfile()
        {
            CreateMap<PostDto, Post>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Tags))); // De List<string> a JSON

            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.Tags) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(src.Tags))); // De JSON a List<string>
        }

    }
}
