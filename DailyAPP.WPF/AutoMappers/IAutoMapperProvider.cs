using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAPP.WPF.AutoMappers
{
    public interface IAutoMapperProvider
    {
        IMapper GetMapper();
    }

    public class AutoMapperProvider : IAutoMapperProvider
    {
        private readonly MapperConfiguration _configuration;

        public AutoMapperProvider(IContainerProvider container)
        {
            _configuration = new MapperConfiguration(configure =>
            {
                configure.ConstructServicesUsing(container.Resolve);

                //扫描profile文件
                configure.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
            },new NullLoggerFactory());
        }

        public IMapper GetMapper()
        {
            return _configuration.CreateMapper();
        }
    }
}
