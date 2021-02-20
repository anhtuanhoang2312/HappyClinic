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
    class PatientViewModel : BaseViewModel
    {
        private ObservableCollection<Patient> _List;
        public ObservableCollection<Patient> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private Patient _SelectedItem;
        public Patient SelectedItem
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
                    Sex = (SelectedItem.Sex == "Nam" ? "0" : "1");
                    Dob = SelectedItem.Dob;
                    Adr = SelectedItem.Adr;
                    Phone = SelectedItem.Phone;
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

        private string _ID = IDGenerator.createID("BN");
        public string ID { get => _ID; set { _ID = value; OnPropertyChanged(); } }

        private string _Name = string.Empty;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }

        private string _Sex = string.Empty;
        public string Sex { get => _Sex; set { _Sex = value; OnPropertyChanged(); } }

        private DateTime? _Dob = null;
        public DateTime? Dob { get => _Dob; set { _Dob = value; OnPropertyChanged(); } }

        private string _Adr = string.Empty;
        public string Adr { get => _Adr; set { _Adr = value; OnPropertyChanged(); } }

        private string _Phone = string.Empty;
        public string Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }

        public ICommand ClearCommand { get; set; }
        public ICommand CheckCommand { get; set; }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public ICommand SearchCommand { get; set; }


        private void Clear()
        {
            ID = IDGenerator.createID("BN");
            Name = string.Empty;
            Sex = string.Empty;
            Dob = null;
            Adr = string.Empty;
            Phone = string.Empty;
        }

        public PatientViewModel()
        {
            List = new ObservableCollection<Patient>(DataProvider.Instance.DB.Patients);

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
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Sex) || string.IsNullOrEmpty(Dob.ToString()) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Adr))
                    return false;

                var isExisted = DataProvider.Instance.DB.Patients.Where(x => x.ID == ID);
                if (isExisted.Count() != 0)
                    return false;

                return true;
            }, (p) =>
            {
                var Patient = new Patient() { ID = ID, Name = Name, Sex = (Sex == "0" ? "Nam" : "Nữ"), Dob = Dob, Phone = Phone, Adr = Adr };

                DataProvider.Instance.DB.Patients.Add(Patient);
                DataProvider.Instance.DB.SaveChanges();

                List.Add(Patient);

                Clear();
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;

                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Sex) || string.IsNullOrEmpty(Dob.ToString()) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Adr))
                    return false;

                var isExisted = DataProvider.Instance.DB.Patients.Where(x => x.ID == ID);
                if (isExisted.Count() == 0)
                    return false;

                return true;

            }, (p) =>
            {
                var Patient = DataProvider.Instance.DB.Patients.Where(x => x.ID == SelectedItem.ID).Single();
                Patient.Name = Name;
                Patient.Sex = (Sex == "0" ? "Nam" : "Nữ");
                Patient.Dob = Dob;
                Patient.Phone = Phone;
                Patient.Adr = Adr;

                DataProvider.Instance.DB.SaveChanges();

                List = new ObservableCollection<Patient>(DataProvider.Instance.DB.Patients);
            });

            SearchCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if(Keyword == "")
                {
                    List = new ObservableCollection<Patient>(DataProvider.Instance.DB.Patients);
                }
                else
                {
                    var result = from patient in DataProvider.Instance.DB.Patients.ToList()
                                 where (patient.Name.ToLower().AccentRemoved().Contains(Keyword.ToLower().AccentRemoved()) || patient.Phone.Contains(Keyword))
                                 select patient;

                    List = new ObservableCollection<Patient>(result);
                }
            });
        }
    }
}
