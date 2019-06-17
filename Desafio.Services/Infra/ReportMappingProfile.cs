using AutoMapper;
using DataDesafio.Models;
using Desafio.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio.Services.Infra
{
    public class ReportMappingProfile : Profile
    {
        public ReportMappingProfile()
        {
            CreateMap<Report, ReportDTO>().ReverseMap();
            CreateMap<ReportItem, ReportItemDTO>().ReverseMap();
        }
    }
}
