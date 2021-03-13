using HappyClinic.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HappyClinic.ViewModel
{
    class AccountViewModel : BaseViewModel
    {
        private ObservableCollection<Account> _List;
        public ObservableCollection<Account> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private Account _SelectedItem;
        public Account SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    Clear();
                    Username = SelectedItem.Username;
                    Pass = SelectedItem.Pass;
                    isAdmin = (SelectedItem.isAdmin == false ? "0" : "1");
                    Status = (SelectedItem.Status == "Đang hoạt động" ? "0" : "1");
                }
            }
        }

        private string _Keyword;
        public string Keyword
        {
            get => _Keyword;
            set
            {
                _Keyword = value;
                OnPropertyChanged();
            }
        }

        private string _Username = string.Empty;
        public string Username { get => _Username; set { _Username = value; OnPropertyChanged(); } }

        private string _Pass = string.Empty;
        public string Pass { get => _Pass; set { _Pass = value; OnPropertyChanged(); } }

        private string _NewPass = string.Empty;
        public string NewPass { get => _NewPass; set { _NewPass = value; OnPropertyChanged(); } }

        private string _isAdmin;
        public string isAdmin { get => _isAdmin; set { _isAdmin = value; OnPropertyChanged(); } }

        private string _Status = string.Empty;
        public string Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }

        public ICommand ClearCommand { get; set; }
        public ICommand CheckCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SearchCommand { get; set; }


        private void Clear()
        {
            Username = string.Empty;
            Pass = string.Empty;
            NewPass = string.Empty;
            _isAdmin = string.Empty;
            Status = string.Empty;
        }

        public AccountViewModel()
        {
            List = new ObservableCollection<Account>(DataProvider.Instance.DB.Accounts);

            ClearCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                Clear();
            });

            CheckCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
            });

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Pass) || string.IsNullOrEmpty(Status))
                    return false;

                var isExisted = DataProvider.Instance.DB.Accounts.Where(x => x.Username == Username);
                if (isExisted.Count() != 0)
                    return false;

                return true;
            }, (p) =>
            {
                var Account = new Account() { Username = Username, Pass = Pass, isAdmin = (isAdmin == "0" ? false : true), Status = "Đang hoạt động" };

                DataProvider.Instance.DB.Accounts.Add(Account);
                DataProvider.Instance.DB.SaveChanges();

                List.Add(Account);

                Clear();

                MessageBox.Show($"Thêm thông tin tài khoản '{Username}' thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;

                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(isAdmin.ToString()) || string.IsNullOrEmpty(Status))
                    return false;

                return true;

            }, (p) =>
            {
                var Account = DataProvider.Instance.DB.Accounts.Where(x => x.Username == SelectedItem.Username).Single();
                Account.Username = Username;
                if (!string.IsNullOrEmpty(NewPass))
                {
                    Account.Pass = NewPass;
                }
                Account.isAdmin = (isAdmin == "0" ? false : true);
                Account.Status = (Status == "0" ? "Đang hoạt động" : "Vô hiệu hóa");

                DataProvider.Instance.DB.SaveChanges();

                List = new ObservableCollection<Account>(DataProvider.Instance.DB.Accounts);

                MessageBox.Show($"Lưu thông tin tài khoản '{Username}' thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            SearchCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (Keyword == "")
                {
                    List = new ObservableCollection<Account>(DataProvider.Instance.DB.Accounts);
                }
                else
                {
                    var result = from a in DataProvider.Instance.DB.Accounts.ToList()
                                 where (a.Username.ToLower().AccentRemoved().Contains(Keyword.ToLower().AccentRemoved()))
                                 select a;

                    List = new ObservableCollection<Account>(result);
                }
            });
        }
    }
}
