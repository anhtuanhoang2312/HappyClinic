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
    class DiseaseViewModel : BaseViewModel
    {
        private ObservableCollection<Disease> _List;
        public ObservableCollection<Disease> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private Disease _SelectedItem;
        public Disease SelectedItem
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

        private string _ID = IDGenerator.createID("B");
        public string ID { get => _ID; set { _ID = value; OnPropertyChanged(); } }

        private string _Name = string.Empty;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }

        private int? _Price = null;
        public int? Price { get => _Price; set { _Price = value; OnPropertyChanged(); } }

        public ICommand ClearCommand { get; set; }
        public ICommand CheckCommand { get; set; }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public ICommand SearchCommand { get; set; }

        private void Clear()
        {
            ID = IDGenerator.createID("B");
            Name = string.Empty;
            Price = null;
        }

        public DiseaseViewModel()
        {
            List = new ObservableCollection<Disease>(DataProvider.Instance.DB.Diseases);

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
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Price.ToString()))
                    return false;

                var isExisted = DataProvider.Instance.DB.Diseases.Where(x => x.ID == ID);
                if (isExisted.Count() != 0)
                    return false;

                return true;
            }, (p) =>
            {
                var Disease = new Disease() { ID = ID, Name = Name, Price = Price };

                DataProvider.Instance.DB.Diseases.Add(Disease);
                DataProvider.Instance.DB.SaveChanges();

                List.Add(Disease);

                Clear();
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;

                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Price.ToString()))
                    return false;

                var isExisted = DataProvider.Instance.DB.Diseases.Where(x => x.ID == ID);
                if (isExisted.Count() == 0)
                    return false;

                return true;

            }, (p) =>
            {
                var Disease = DataProvider.Instance.DB.Diseases.Where(x => x.ID == SelectedItem.ID).Single();
                Disease.Name = Name;
                Disease.Price = Price;

                DataProvider.Instance.DB.SaveChanges();

                List = new ObservableCollection<Disease>(DataProvider.Instance.DB.Diseases);
            });

            SearchCommand = new RelayCommand<object>((p) =>
            {            
                return true;
            }, (p) =>
            {
                if (Keyword == "")
                {
                    List = new ObservableCollection<Disease>(DataProvider.Instance.DB.Diseases);
                }
                else
                {
                    var result = from d in DataProvider.Instance.DB.Diseases.ToList()
                                 where d.Name.ToLower().AccentRemoved().Contains(Keyword.ToLower().AccentRemoved())
                                 select d;

                    List = new ObservableCollection<Disease>(result);
                }
            });
        }
    }
}
