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
                .ForMember(vm => vm.WSystemId, map => map.MapFrom(m => m.WSystem.ID))
                .ForMember(vm => vm.GwclStation, map => map.MapFrom(m => m.GwclStation.Name))
                .ForMember(vm => vm.StationCode, map => map.MapFrom(m => m.GwclStation.StationCode))
                .ForMember(vm => vm.GwclStationId, map => map.MapFrom(m => m.GwclStation.ID));

            Mapper.CreateMap<HourlyProduction, ProductionViewModel>()
               .ForMember(vm => vm.DailyActual, map => map.MapFrom(m => m.HourlyActual))
               .ForMember(vm => vm.Option, map => map.MapFrom(m => m.Option.Name))
               .ForMember(vm => vm.OptionId, map => map.MapFrom(m => m.Option.ID))
               .ForMember(vm => vm.OptionType, map => map.MapFrom(m => m.OptionType.Name))
               .ForMember(vm => vm.OptionTypeId, map => map.MapFrom(m => m.OptionType.ID))
               .ForMember(vm => vm.WSystem, map => map.MapFrom(m => m.WSystem.Name))
               .ForMember(vm => vm.WSystemCode, map => map.MapFrom(m => m.WSystem.Code))
               .ForMember(vm => vm.WSystemId, map => map.MapFrom(m => m.WSystem.ID))
               .ForMember(vm => vm.GwclStation, map => map.MapFrom(m => m.GwclStation.Name))
               .ForMember(vm => vm.StationCode, map => map.MapFrom(m => m.GwclStation.StationCode))
               .ForMember(vm => vm.GwclStationId, map => map.MapFrom(m => m.GwclStation.ID));

            Mapper.CreateMap<PlantDowntime, PlantDowntimeViewModel>()
                .ForMember(vm => vm.WSystem, map => map.MapFrom(m => m.WSystem.Name))
                .ForMember(vm => vm.WSystemId, map => map.MapFrom(m => m.WSystem.ID));

            Mapper.CreateMap<User, RegistrationViewModel>()
                .ForMember(vm => vm.Password, map => map.MapFrom(m => m.HashedPassword))
                .ForMember(vm => vm.GwclRegion, map => map.MapFrom(m => m.GwclRegion.Name))
                .ForMember(vm => vm.GwclStation, map => map.MapFrom(m => m.GwclStation.Name))
                .ForMember(vm => vm.GwclRegionId, map => map.MapFrom(m => m.GwclRegion.ID))
                .ForMember(vm => vm.GwclStationId, map => map.MapFrom(m => m.GwclStation.ID))
                .ForMember(vm => vm.Role, map => map.MapFrom(m => m.Role.Name))
                .ForMember(vm => vm.RoleId, map => map.MapFrom(m => m.Role.ID));

            Mapper.CreateMap<GwclRegion, GwclRegionViewModel>()
                .ForMember(vm => vm.GwclArea, map => map.MapFrom(m => m.GwclArea.Name))
                .ForMember(vm => vm.GwclAreaID, map => map.MapFrom(m => m.GwclArea.ID));

            Mapper.CreateMap<GwclStation, GwclStationViewModel>()
                .ForMember(vm => vm.GwclRegion, map => map.MapFrom(m => m.GwclRegion.Name))
                .ForMember(vm => vm.GwclRegionId, map => map.MapFrom(m => m.GwclRegion.ID));

            Mapper.CreateMap<Role, RoleViewModel>()
               .ForMember(vm => vm.Name, map => map.MapFrom(m => m.Name));

            Mapper.CreateMap<Option, OptionViewModel>();

            Mapper.CreateMap<GwclArea, GwclAreaViewModel>();

            Mapper.CreateMap<OptionType, OptionTypeViewModel>();

            Mapper.CreateMap<WSystem, WSystemViewModel>()
                .ForMember(vm => vm.GwclStation, map => map.MapFrom(m => m.GwclStation.Name))
                .ForMember(vm => vm.GwclStationId, map => map.MapFrom(m => m.GwclStation.ID));
        }
    }
}