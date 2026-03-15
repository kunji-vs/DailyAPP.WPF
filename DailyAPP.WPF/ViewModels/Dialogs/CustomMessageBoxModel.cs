using Prism.Commands;
using Prism.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyAPP.WPF.ViewModels.Dialogs
{
    public class CustomMessageBoxModel : IDialogAware
    {
        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }


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
            if(parameters.ContainsKey("message"))
            {
                Message = parameters.GetValue<string>("message");
            }
        }

        public DelegateCommand ConfirmCommand => new DelegateCommand(() =>
        {
            RequestClose.Invoke(new DialogResult(ButtonResult.OK));
        });
         public DelegateCommand CancelCommand => new DelegateCommand(() =>
         {
             RequestClose.Invoke(new DialogResult(ButtonResult.Cancel));
         });

    }
}
