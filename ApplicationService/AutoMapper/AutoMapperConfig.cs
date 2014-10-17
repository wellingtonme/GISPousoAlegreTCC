using AutoMapper;
using Model.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void RegisterMappers()
        {
            Mapper.CreateMap<SchoolModel, GenericPointModel>();
            Mapper.CreateMap<GenericPointModel, SchoolModel>();

            Mapper.CreateMap<CopsModel, GenericPointModel>();
            Mapper.CreateMap<GenericPointModel, CopsModel>();
        }
    }
}
