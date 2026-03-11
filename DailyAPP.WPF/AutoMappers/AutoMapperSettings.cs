using AutoMapper;
using DailyAPP.WPF.DTOs;
using DailyAPP.WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAPP.WPF.AutoMappers
{
    public class AutoMapperSettings:Profile
    {
        public AutoMapperSettings()
        {
            CreateMap<MenuInfoDTO, LeftMenuInfo>().ReverseMap();
            CreateMap<LeftMenuInfo, MenuInfoDTO>().ReverseMap();
        }
    }
}
