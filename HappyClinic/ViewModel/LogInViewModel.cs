using HappyClinic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HappyClinic.ViewModel
{
    class LogInViewModel : BaseViewModel
    {
        private string _Username = string.Empty;
        public string Username { get => _Username; set { _Username = value; OnPropertyChanged(); } }

        private string _Pass = string.Empty;
        public string Pass { get => _Pass; set { _Pass = value; OnPropertyChanged(); } }

        public ICommand CheckCommand { get; set; }

        public LogInViewModel()
        {
            CheckCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Pass))
                    return false;

                return true;
            }, (p) =>
            {
                var isExisted = DataProvider.Instance.DB.Accounts.Where(x => x.Username == Username);
                if (isExisted.Count() == 0)
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng. Vui lòng kiểm tra lại.", "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    var isValid = DataProvider.Instance.DB.Accounts.Where(x => x.Username == Username).Single();
                    if (isValid != null)
                    {
                        if (isValid.Pass != Pass)
                        {
                            MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng. Vui lòng kiểm tra lại.", "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else if (isValid.Status == "Vô hiệu hóa")
                        {
                            MessageBox.Show("Tài khoản này đã bị vô hiệu hóa. Vui lòng liên hệ với quản lý để biết thêm chi tiết.", "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            CurrentUser.Instance.User = isValid;
                            MainWindow main = new MainWindow();
                            main.Show();
                            Username = string.Empty;
                            Pass = string.Empty;
                            foreach (Window item in Application.Current.Windows)
                            {
                                if (item.DataContext == this) item.Close();
                            }
                        }
                    }
                }
            });
        }
    }
}
