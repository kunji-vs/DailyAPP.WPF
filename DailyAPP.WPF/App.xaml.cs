using System;
using Prism;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.DryIoc;
using Prism.Ioc;
using DailyAPP.WPF.Views;
using DailyAPP.WPF.ViewModels;
using Prism.Dialogs;
using Prism.Container.DryIoc;
using DailyAPP.WPF.HttpClient;
using DryIoc;
using MaterialDesignThemes;
using MaterialDesignThemes.Wpf;

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

            containerRegistry.RegisterSingleton<HttpRestClient>();

            containerRegistry.RegisterSingleton<SnackbarMessageQueue>();
        }

        protected override void OnInitialized()
        {
            var dialog = Container.Resolve<IDialogService>();
            dialog.ShowDialog("LoginUC", callback =>
            {
                //登录失败
                if(callback.Result != ButtonResult.OK)
                {
                    //返回到主程序
                    Environment.Exit(0);
                    return;
                }
                base.OnInitialized();
            });
        }
    }
}
