using AutoMapper;
using Sicolnet.Models.BD;
using Sicolnet.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sicolnet.Utils
{
    public class AutoMapperAppProfile : Profile
    {

        public AutoMapperAppProfile()
        {
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Departamento, DepartamentoDto>().ReverseMap();
            CreateMap<Municipio, MunicipioDto>().ReverseMap();
            CreateMap<Persona, PersonaDto>().ForMember(f => f.Referente, g => g.Ignore()).ReverseMap();
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            //CreateMap<Course, CourseDto>()
            //    .ForMember(s => s.modules,
            //    op => op.MapFrom(s => sbd.Modules.Where(m => m.courseId == s.courseId).ToList()));

            //CreateMap<CourseDto, Course>()
            //    .ForMember(s => s.modules,
            //    op => op.Ignore());

            //CreateMap<UserCourse, UserCourseDto>()
            //    .ForMember(s => s.user, opt => opt.MapFrom(o => sbd.Users.Where(u => u.userId == o.userId).FirstOrDefault()))
            //    .ReverseMap();

            //CreateMap<Module, ModuleDto>()
            //    .ForMember(s => s.course, opt => opt.Ignore())
            //    .ForMember(s => s.videos,
            //    op => op.MapFrom(s => sbd.Videos.Where(m => m.moduleId == s.moduleId).ToList()));

            //CreateMap<ModuleDto, Module>()
            //   .ForMember(s => s.course, opt => opt.Ignore())
            //   .ForMember(s => s.videos,
            //   op => op.Ignore());

            //CreateMap<Video, VideoDto>()
            //    .ForMember(s => s.module, opt => opt.Ignore())
            //    .ReverseMap();

            //CreateMap<Level, LevelDto>()
            //    .ReverseMap();
            //CreateMap<Category, CategoryDto>()
            //    .ReverseMap();
        }



    }
}
