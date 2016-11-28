using AutoMapper;
using GwcltdApp.Entities;
using GwcltdApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GwcltdApp.Web.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<ProductionViewModel, Production>()
                //.ForMember(m => m.Image, map => map.Ignore())
                .ForMember(m => m.Option, map => map.Ignore())
                .ForMember(m => m.OptionType, map => map.Ignore())
                .ForMember(m => m.WSystem, map => map.Ignore());
        }
    }
}