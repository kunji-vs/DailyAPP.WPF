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
    public class AddMemoUCModel: BindableBase, IDialogAware
    {
        private string _title;

        private AccountInfoDTO _accountInfo;

        public AccountInfoDTO AccountInfo
        {
            get { return _accountInfo; }
            set { _accountInfo = value; }
        }

        private MemoInfoDTO _memoInfo;

        public MemoInfoDTO MemoInfo
        {
            get { return _memoInfo; }
            set { _memoInfo = value; }
        }


        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(); }
        }

        public AddMemoUCModel()
        {
            MemoInfo = new MemoInfoDTO()
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
            if (string.IsNullOrEmpty(MemoInfo.Title) && string.IsNullOrEmpty(MemoInfo.Content))
            {
                MessageBox.Show("请至少输入标题或内容");
                return;
            }


            DialogResult result = new DialogResult(ButtonResult.OK);
            result.Parameters.Add("memoInfo", MemoInfo);
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
                MemoInfo.AccountId = AccountInfo.AccountId;
            }
            if (parameters.ContainsKey("memoInfo"))
            {
                MemoInfo = parameters.GetValue<MemoInfoDTO>("memoInfo");
            }
            if (parameters.ContainsKey("title"))
            {
                Title = parameters.GetValue<string>("title");
            }
        }
    }
}
