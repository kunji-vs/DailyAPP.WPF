using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DailyAPP.WPF.Extensions
{
    internal class PasswordBoxExtend
    {
        // 定义附加属性：BindablePassword
        public static readonly DependencyProperty BindablePasswordProperty =
            DependencyProperty.RegisterAttached(
                "BindablePassword",
                typeof(string),
                typeof(PasswordBoxExtend),
                new FrameworkPropertyMetadata(string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnBindablePasswordChanged));

        // 获取附加属性值
        public static string GetBindablePassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BindablePasswordProperty);
        }

        // 设置附加属性值
        public static void SetBindablePassword(DependencyObject obj, string value)
        {
            obj.SetValue(BindablePasswordProperty, value);
        }

        // 附加属性变更时的处理逻辑
        private static void OnBindablePasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not PasswordBox passwordBox) return;

            // 移除旧事件（避免重复绑定）
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            // 如果是代码侧（VM）修改了值，同步到PasswordBox
            if (e.NewValue != null)
            {
                string newPassword = e.NewValue.ToString();
                if (passwordBox.Password != newPassword)
                {
                    passwordBox.Password = newPassword;
                }
            }

            // 绑定PasswordChanged事件：UI侧输入密码时同步到附加属性
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }

        // PasswordBox输入变化时，同步到附加属性（进而同步到VM）
        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                SetBindablePassword(passwordBox, passwordBox.Password);
            }
        }
    }
}
