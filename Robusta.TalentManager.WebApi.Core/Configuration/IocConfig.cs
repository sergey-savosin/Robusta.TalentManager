using AutoMapper;
using Robusta.TalentManager.Data;
using Robusta.TalentManager.WebApi.Core.Infrastructure;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Robusta.TalentManager.WebApi.Core.Configuration
{
    public static class IocConfig
    {
        public static void RegisterDependencyResolver(HttpConfiguration config)
        {
            var c = new Container(cn =>
            {
                cn.Scan(scan =>
                {
                    scan.WithDefaultConventions();

                    AppDomain.CurrentDomain.GetAssemblies()
                        .Where(a => a.GetName().Name.StartsWith("Robusta.TalentManager"))
                        .ToList()
                        .ForEach(a => scan.Assembly(a));
                });

                cn.For<IMapper>().Use(Mapper.Instance);
                cn.For(typeof(IRepository<>)).Use(typeof(Repository<>));
            });

            config.DependencyResolver = new StructureMapContainer(c);
        }
    }
}
