using AutoMapper;
using GwcltdApp.Entities;
using GwcltdApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GwcltdApp.Web.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Production, ProductionViewModel>()
                .ForMember(vm => vm.Option, map => map.MapFrom(m => m.Option.Name))
                .ForMember(vm => vm.OptionId, map => map.MapFrom(m => m.Option.ID))
                .ForMember(vm => vm.OptionType, map => map.MapFrom(m => m.OptionType.Name))
                .ForMember(vm => vm.OptionTypeId, map => map.MapFrom(m => m.OptionType.ID))
                .ForMember(vm => vm.WSystem, map => map.MapFrom(m => m.WSystem.Name))
                .ForMember(vm => vm.WSystemCode, map => map.MapFrom(m => m.WSystem.Code))
                .ForMember(vm => vm.WSystemId, map => map.MapFrom(m => m.WSystem.ID));

            Mapper.CreateMap<Option, OptionViewModel>();

            Mapper.CreateMap<OptionType, OptionTypeViewModel>();

            Mapper.CreateMap<WSystem, WSystemViewModel>();
        }
    }
}