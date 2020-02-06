using AutoMapper;
using TechLibrary.Domain;
using TechLibrary.Models;

namespace TechLibrary.MappingProfiles
{
    public class ResponseToDomainProfile : Profile
    {
        public ResponseToDomainProfile()
        {
            CreateMap<BookResponse, Book>().ForMember(x => x.ShortDescr, opt => opt.MapFrom(src => src.Descr));
        }
    }
}