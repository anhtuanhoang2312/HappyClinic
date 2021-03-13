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
    class MedicineViewModel : BaseViewModel
    {
        private ObservableCollection<Medicine> _List;
        public ObservableCollection<Medicine> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private Medicine _SelectedItem;
        public Medicine SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    ID = SelectedItem.ID;
                    Name = SelectedItem.Name;
                    Price = SelectedItem.Price;
                    Qty = SelectedItem.Qty;
                    Supplier = SelectedItem.Supplier;
                    MfgDate = SelectedItem.MfgDate;
                    ExpDate = SelectedItem.ExpDate;
                    UnitID = DataProvider.Instance.DB.MedicineUnits.Where(x => x.ID == SelectedItem.UnitID).Single().Name;
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

        private string _ID = IDGenerator.createID("T");
        public string ID { get => _ID; set { _ID = value; OnPropertyChanged(); } }

        private string _Name = string.Empty;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }

        private int? _Price = null;
        public int? Price { get => _Price; set { _Price = value; OnPropertyChanged(); } }

        private int? _Qty = null;
        public int? Qty { get => _Qty; set { _Qty = value; OnPropertyChanged(); } }

        private string _Supplier = string.Empty;
        public string Supplier { get => _Supplier; set { _Supplier = value; OnPropertyChanged(); } }

        private DateTime? _MfgDate = null;
        public DateTime? MfgDate { get => _MfgDate; set { _MfgDate = value; OnPropertyChanged(); } }

        private DateTime? _ExpDate = null;
        public DateTime? ExpDate { get => _ExpDate; set { _ExpDate = value; OnPropertyChanged(); } }

        private string _UnitID = string.Empty;
        public string UnitID { get => _UnitID; set { _UnitID = value; OnPropertyChanged(); } }

        private ObservableCollection<MedicineUnit> _AllUnits;
        public ObservableCollection<MedicineUnit> AllUnits { get => _AllUnits; set { _AllUnits = value; OnPropertyChanged(); } }

        public ICommand ClearCommand { get; set; }
        public ICommand CheckCommand { get; set; }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public ICommand SearchCommand { get; set; }

        private void Clear()
        {
            ID = IDGenerator.createID("T");
            Name = string.Empty;
            Price = null;
            Qty = null;
            Supplier = string.Empty;
            MfgDate = null;
            ExpDate = null;
            UnitID = string.Empty;
        }

        public MedicineViewModel()
        {
            List = new ObservableCollection<Medicine>(DataProvider.Instance.DB.Medicines);
            AllUnits = new ObservableCollection<MedicineUnit>(DataProvider.Instance.DB.MedicineUnits);

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
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Price.ToString()) || string.IsNullOrEmpty(Supplier) || string.IsNullOrEmpty(MfgDate.ToString())
                    || string.IsNullOrEmpty(ExpDate.ToString()) || MfgDate > ExpDate || string.IsNullOrEmpty(Qty.ToString()) || string.IsNullOrEmpty(UnitID))
                    return false;

                var isExisted = DataProvider.Instance.DB.Medicines.Where(x => x.ID == ID);
                if (isExisted.Count() != 0)
                    return false;

                return true;
            }, (p) =>
            {
                var Medicine = new Medicine() { ID = ID, Name = Name, Price = Price, Supplier = Supplier, MfgDate = MfgDate, ExpDate = ExpDate, Qty = Qty, UnitID = UnitID };

                DataProvider.Instance.DB.Medicines.Add(Medicine);
                DataProvider.Instance.DB.SaveChanges();

                List.Add(Medicine);

                Clear();

                MessageBox.Show($"Thêm thông tin thuốc '{Name}' thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;

                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Price.ToString()) || string.IsNullOrEmpty(Supplier) || string.IsNullOrEmpty(MfgDate.ToString())
                    || string.IsNullOrEmpty(ExpDate.ToString()) || MfgDate > ExpDate || string.IsNullOrEmpty(Qty.ToString()) || string.IsNullOrEmpty(UnitID))
                    return false;

                return true;

            }, (p) =>
            {
                var Medicine = DataProvider.Instance.DB.Medicines.Where(x => x.ID == SelectedItem.ID).Single();
                Medicine.Name = Name;
                Medicine.Price = Price;
                Medicine.Supplier = Supplier;
                Medicine.MfgDate = MfgDate;
                Medicine.ExpDate = ExpDate;
                Medicine.Qty = Qty;
                Medicine.UnitID = DataProvider.Instance.DB.MedicineUnits.Where(x => x.Name == UnitID).Single().ID;

                DataProvider.Instance.DB.SaveChanges();

                List = new ObservableCollection<Medicine>(DataProvider.Instance.DB.Medicines);

                MessageBox.Show($"Lưu thông tin thuốc '{Name}' thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            SearchCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (Keyword == "")
                {
                    List = new ObservableCollection<Medicine>(DataProvider.Instance.DB.Medicines);
                }
                else
                {
                    var result = from m in DataProvider.Instance.DB.Medicines.ToList()
                                 where (m.Name.ToLower().AccentRemoved().Contains(Keyword.ToLower().AccentRemoved()) || m.ID.ToLower().AccentRemoved().Contains(Keyword.ToLower().AccentRemoved()))
                                 select m;

                    List = new ObservableCollection<Medicine>(result);
                }
            });
        }
    }
}
