using HappyClinic.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HappyClinic.ViewModel
{
    class MedicineUnitViewModel : BaseViewModel
    {
        private ObservableCollection<MedicineUnit> _List;
        public ObservableCollection<MedicineUnit> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private MedicineUnit _SelectedItem;
        public MedicineUnit SelectedItem
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

        private string _ID = IDGenerator.createID("DV");
        public string ID { get => _ID; set { _ID = value; OnPropertyChanged(); } }

        private string _Name = string.Empty;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }

        public ICommand ClearCommand { get; set; }
        public ICommand CheckCommand { get; set; }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public ICommand SearchCommand { get; set; }

        private void Clear()
        {
            ID = IDGenerator.createID("DV");
            Name = string.Empty;
        }

        public MedicineUnitViewModel()
        {
            List = new ObservableCollection<MedicineUnit>(DataProvider.Instance.DB.MedicineUnits);

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
                if (string.IsNullOrEmpty(Name))
                    return false;

                var isExisted = DataProvider.Instance.DB.MedicineUnits.Where(x => x.ID == ID);
                if (isExisted.Count() != 0)
                    return false;

                return true;
            }, (p) =>
            {
                var MedicineUnit = new MedicineUnit() { ID = ID, Name = Name };

                DataProvider.Instance.DB.MedicineUnits.Add(MedicineUnit);
                DataProvider.Instance.DB.SaveChanges();

                List.Add(MedicineUnit);

                Clear();
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;

                if (string.IsNullOrEmpty(Name))
                    return false;

                var isExisted = DataProvider.Instance.DB.MedicineUnits.Where(x => x.ID == ID);
                if (isExisted.Count() == 0)
                    return false;

                return true;

            }, (p) =>
            {
                var MedicineUnit = DataProvider.Instance.DB.MedicineUnits.Where(x => x.ID == SelectedItem.ID).Single();
                MedicineUnit.Name = Name;

                DataProvider.Instance.DB.SaveChanges();

                List = new ObservableCollection<MedicineUnit>(DataProvider.Instance.DB.MedicineUnits);
            });

            SearchCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (Keyword == "")
                {
                    List = new ObservableCollection<MedicineUnit>(DataProvider.Instance.DB.MedicineUnits);
                }
                else
                {
                    var result = from mu in DataProvider.Instance.DB.MedicineUnits.ToList()
                                 where mu.Name.ToLower().AccentRemoved().Contains(Keyword.ToLower().AccentRemoved())
                                 select mu;

                    List = new ObservableCollection<MedicineUnit>(result);
                }
            });
        }
    }
}

