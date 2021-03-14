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
    class MedicineUsageViewModel : BaseViewModel
    {
        private ObservableCollection<MedicineUsage> _List;
        public ObservableCollection<MedicineUsage> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private MedicineUsage _SelectedItem;
        public MedicineUsage SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    ID = SelectedItem.ID;
                    ShortDsc = SelectedItem.ShortDsc;
                    LongDsc = SelectedItem.LongDsc;
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

        private string _ID = IDGenerator.createID("CD");
        public string ID { get => _ID; set { _ID = value; OnPropertyChanged(); } }

        private string _ShortDsc = string.Empty;
        public string ShortDsc { get => _ShortDsc; set { _ShortDsc = value; OnPropertyChanged(); } }

        private string _LongDsc = string.Empty;
        public string LongDsc { get => _LongDsc; set { _LongDsc = value; OnPropertyChanged(); } }

        public ICommand ClearCommand { get; set; }
        public ICommand CheckCommand { get; set; }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public ICommand SearchCommand { get; set; }

        private void Clear()
        {
            ID = IDGenerator.createID("DV");
            ShortDsc = string.Empty;
            LongDsc = string.Empty;
        }

        public MedicineUsageViewModel()
        {
            List = new ObservableCollection<MedicineUsage>(DataProvider.Instance.DB.MedicineUsages);

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
                if (string.IsNullOrEmpty(ShortDsc) || string.IsNullOrEmpty(LongDsc))
                    return false;

                var isExisted = DataProvider.Instance.DB.MedicineUsages.Where(x => x.ID == ID);
                if (isExisted.Count() != 0)
                    return false;

                return true;
            }, (p) =>
            {
                var MedicineUsage = new MedicineUsage() { ID = ID, ShortDsc = ShortDsc, LongDsc = LongDsc };

                DataProvider.Instance.DB.MedicineUsages.Add(MedicineUsage);
                DataProvider.Instance.DB.SaveChanges();

                List.Add(MedicineUsage);

                MessageBox.Show("Thêm cách dùng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                Clear();
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;

                if (string.IsNullOrEmpty(ShortDsc) || string.IsNullOrEmpty(LongDsc))
                    return false;

                var isExisted = DataProvider.Instance.DB.MedicineUsages.Where(x => x.ID == ID);
                if (isExisted.Count() == 0)
                    return false;

                return true;

            }, (p) =>
            {
                var MedicineUsage = DataProvider.Instance.DB.MedicineUsages.Where(x => x.ID == SelectedItem.ID).Single();
                MedicineUsage.ShortDsc = ShortDsc;
                MedicineUsage.LongDsc = LongDsc;

                DataProvider.Instance.DB.SaveChanges();

                List = new ObservableCollection<MedicineUsage>(DataProvider.Instance.DB.MedicineUsages);

                MessageBox.Show("Lưu cách dùng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            SearchCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (Keyword == "")
                {
                    List = new ObservableCollection<MedicineUsage>(DataProvider.Instance.DB.MedicineUsages);
                }
                else
                {
                    var result = from mu in DataProvider.Instance.DB.MedicineUsages.ToList()
                                 where mu.ShortDsc.ToLower().AccentRemoved().Contains(Keyword.ToLower().AccentRemoved()) || mu.LongDsc.ToLower().AccentRemoved().Contains(Keyword.ToLower().AccentRemoved())
                                 select mu;

                    List = new ObservableCollection<MedicineUsage>(result);
                }
            });
        }
    }
}
