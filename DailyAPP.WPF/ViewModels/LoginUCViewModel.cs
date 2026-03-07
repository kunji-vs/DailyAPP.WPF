using DailyAPP.WPF.DTOs;
using DailyAPP.WPF.HttpClient;
using DailyAPP.WPF.MsgEvents;
using Prism.Commands;
using Prism.Dialogs;
using Prism.Events;
using Prism.Mvvm;
using MaterialDesignThemes.Wpf;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyAPP.WPF.ViewModels
{
    internal class LoginUCViewModel :BindableBase, IDialogAware
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set 
            { 
                _title = value;
                RaisePropertyChanged();
            }
        }

        public DialogCloseListener RequestClose { get; }

        private readonly HttpRestClient HttpRestClient;

        private readonly IEventAggregator eventAggregator;

        public SnackbarMessageQueue MessageQueue { get; }

        public LoginUCViewModel(HttpRestClient _httpRestClient,IEventAggregator _eventAggregator, SnackbarMessageQueue snackbarMessageQueue)
        {
            Title = "登录";
            ShowRegiest = new DelegateCommand<string>(ShowRegisterTab);
            LoginCommand = new DelegateCommand(Login);
            RegisterAccount = new DelegateCommand(RegisterAccountCmd);
            AccountInfo = new AccountInfoDTO();
            HttpRestClient = _httpRestClient;
            eventAggregator = _eventAggregator;
            MessageQueue = snackbarMessageQueue;
        }


        #region 注册页面切换

        private int _transitionerSelectedIndex = 0;
        public DelegateCommand<string> ShowRegiest { get; set; }
        public DelegateCommand ReturnLogin { get; set; }


        public int TransitionerSelectedIndex
        {
            get { return _transitionerSelectedIndex; }
            set { _transitionerSelectedIndex = value; RaisePropertyChanged(); }
        }

        private void ShowRegisterTab(string index)
        {
            bool r = int.TryParse(index, out int res);
            if (r) TransitionerSelectedIndex = res;
        }

        #endregion


        #region 注册

        private AccountInfoDTO _AccountInfoDTO;

        private string _ConfrimPwd;

        public string ConfrimPwd
        {
            get { return _ConfrimPwd; }
            set { _ConfrimPwd = value; }
        }


        public AccountInfoDTO AccountInfo
        {
            get { return _AccountInfoDTO; }
            set { _AccountInfoDTO = value; RaisePropertyChanged(); }
        }

        public DelegateCommand RegisterAccount { get; set; }

        private void RegisterAccountCmd()
        {
            if(string.IsNullOrEmpty(AccountInfo.Name)|| string.IsNullOrEmpty(AccountInfo.Account)|| string.IsNullOrEmpty(AccountInfo.Pwd)|| string.IsNullOrEmpty(ConfrimPwd))
            {
                //eventAggregator.GetEvent<MsgEvent>().Publish("请输入完成的注册信息");
                MessageQueue.Enqueue("请输入完成的注册信息");
                return;
            }
            if (AccountInfo.Pwd != ConfrimPwd)
            {
                //eventAggregator.GetEvent<MsgEvent>().Publish("两次密码不一致");
                MessageQueue.Enqueue("两次密码不一致");
                return;
            }
            //调用API
            ApiRequest request = new ApiRequest()
            {
                Method = RestSharp.Method.POST,
                Route = "Account/RegiestAccount",
                Parameters = AccountInfo
            };
            var response = HttpRestClient.Excute(request);
            if(response.ResultCode == 200)
            {
                //eventAggregator.GetEvent<MsgEvent>().Publish("注册成功");
                MessageQueue.Enqueue("注册成功");
                TransitionerSelectedIndex = 0;
            }
            else
            {
                MessageBox.Show(response.msg);
            }
        }

        #endregion

        #region 登录

        public DelegateCommand LoginCommand { get; set; }

        private string _account;
        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; RaisePropertyChanged(); }
        }

        public string Account
        {
            get { return _account; }
            set { _account = value; RaisePropertyChanged(); }
        }


        /// <summary>
        /// 登录
        /// </summary>
        private void Login()
        {
            if(string.IsNullOrEmpty(Account) || string.IsNullOrEmpty(Password))
            {
                //MessageBox.Show("请输入账号/密码");
                MessageQueue.Enqueue("请输入账号/密码");
                return;
            }
            ApiRequest req = new ApiRequest()
            {
                Method = RestSharp.Method.POST,
                ContentType = "application/json",
                Route = $"Account/Login?username={Account}&password={Password}"
            };
            var res = HttpRestClient.Excute(req);
            if(res.ResultCode == 200)
            {
                RequestClose.Invoke(ButtonResult.OK);
            }
            else
            {
                MessageBox.Show(res.msg);
            }
        }

        #endregion


        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
 
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}
