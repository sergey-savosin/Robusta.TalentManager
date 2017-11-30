using AutoMapper;
using Robusta.TalentManager.Domain;
using Robusta.TalentManager.WebApi.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robusta.TalentManager.WebApi.Core.Configuration
{
    public class DtoMapperConfig
    {
        public static void CreateMaps()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<EmployeeDto, Employee>();
                cfg.CreateMap<Employee, EmployeeDto>();
            });
            //var mapper = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<EmployeeDto, Employee>();
            //    cfg.CreateMap<Employee, EmployeeDto>();
            //}).CreateMapper();

        }
    }
}
