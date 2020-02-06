using AutoMapper;
using TechLibrary.Domain;
using TechLibrary.Models;

namespace TechLibrary.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<BookRequest, Book>().ForMember(x => x.ShortDescr, opt => opt.MapFrom(src => src.Descr));
        }
    }
}