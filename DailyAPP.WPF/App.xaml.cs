using AutoMapper;
using DailyAPP.WPF.AutoMappers;
using DailyAPP.WPF.DTOs;
using DailyAPP.WPF.HttpClient;
using DailyAPP.WPF.ViewModels;
using DailyAPP.WPF.ViewModels.Dialogs;
using DailyAPP.WPF.Views;
using DailyAPP.WPF.Views.Dialogs;
using DryIoc;
using MaterialDesignThemes;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging.Abstractions;
using Prism;
using Prism.Container.DryIoc;
using Prism.Dialogs;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DailyAPP.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWin>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainWin, MainWinViewModel>();
            containerRegistry.RegisterDialog<LoginUC, LoginUCViewModel>();
            containerRegistry.RegisterForNavigation<HomeUC, HomeUCModel>("HomeUC");
            containerRegistry.RegisterForNavigation<WaitUC, WaitUCModel>("WaitUC");
            containerRegistry.RegisterForNavigation<MemoUC, MemoUCModel>("MemoUC");
            containerRegistry.RegisterForNavigation<SettingsUC, SettingsUCModel>("SettingsUC");
            containerRegistry.RegisterForNavigation<PersonalUC, PersonalUCModel>();

            containerRegistry.RegisterForNavigation<SysSetUC>();
            containerRegistry.RegisterForNavigation<AboutUs>();

            containerRegistry.RegisterDialog<AddWaitUC, AddWaitUCModel>();
            containerRegistry.RegisterDialog<AddMemoUC, AddMemoUCModel>();
            containerRegistry.RegisterDialog<CustomMessageBox, CustomMessageBoxModel>();

            containerRegistry.RegisterSingleton<HttpRestClient>();
            containerRegistry.RegisterSingleton<SnackbarMessageQueue>();

            var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(AutoMapperSettings).Assembly),new NullLoggerFactory());
            config.AssertConfigurationIsValid(); // 验证配置，开发环境必加
            containerRegistry.RegisterInstance<IMapper>(config.CreateMapper());
        }


        protected override void OnInitialized()
        {
            var dialog = Container.Resolve<IDialogService>();
            dialog.ShowDialog("LoginUC", callback =>
            {
                //登录失败
                if (callback.Result != ButtonResult.OK)
                {
                    //返回到主程序
                    Environment.Exit(0);
                    return;
                }

                var mainVM = Current.MainWindow.DataContext as MainWinViewModel;
                if(mainVM != null)
                {
                    if(callback.Parameters.ContainsKey("userInfo"))
                    {
                        AccountInfoDTO accountInfo = callback.Parameters.GetValue<AccountInfoDTO>("userInfo");
                        mainVM.CreateMenuList(accountInfo);
                    }
                }

                base.OnInitialized();
            });
        }
    }
}
