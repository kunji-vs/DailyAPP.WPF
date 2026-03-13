using DailyAPP.WPF.DTOs;
using Prism.Commands;
using Prism.Dialogs;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DailyAPP.WPF.ViewModels.Dialogs
{
    public class AddWaitUCModel :BindableBase,IDialogAware
    {
        private string _title;

        private AccountInfoDTO _accountInfo;

        public AccountInfoDTO AccountInfo
        {
            get { return _accountInfo; }
            set { _accountInfo = value; }
        }

        private WaitInfoDTO _waitInfo;

        public WaitInfoDTO WaitInfo
        {
            get { return _waitInfo; }
            set { _waitInfo = value; }
        }


        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(); }
        }

        public AddWaitUCModel()
        {
            WaitInfo = new WaitInfoDTO()
            {
                Status = 0
            };
        }

        public DelegateCommand CancelCommand => new DelegateCommand(() =>
        {
            RequestClose.Invoke(new DialogResult(ButtonResult.Cancel));
        });


        public DelegateCommand OKCommand => new DelegateCommand(() =>
        {
            if(string.IsNullOrEmpty(WaitInfo.Title) && string.IsNullOrEmpty(WaitInfo.Content))
            {
                MessageBox.Show("请至少输入标题或内容");
                return;
            }


            DialogResult result = new DialogResult(ButtonResult.OK);
            result.Parameters.Add("waitInfo", WaitInfo);
            RequestClose.Invoke(result);
        });

        public DialogCloseListener RequestClose { get; }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("userInfo"))
            {
                AccountInfo = parameters.GetValue<AccountInfoDTO>("userInfo");
                WaitInfo.AccountId = AccountInfo.AccountId;
            }
            if(parameters.ContainsKey("title"))
            {
                Title = parameters.GetValue<string>("title");
            }
        }
    }
}
