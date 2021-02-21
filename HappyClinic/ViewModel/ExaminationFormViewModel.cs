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
    class ExaminationFormViewModel : BaseViewModel
    {
        private ObservableCollection<ExaminationForm> _List;
        public ObservableCollection<ExaminationForm> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ExaminationForm _SelectedItem;
        public ExaminationForm SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    ID = SelectedItem.ID;
                    EDate = SelectedItem.EDate;
                    Symptom = SelectedItem.Symptom;
                    PatientID = DataProvider.Instance.DB.Patients.Where(x => x.ID == SelectedItem.PatientID).Single().Name;
                    ThisDisease = new ObservableCollection<Disease>(GetDiseases());
                    TempList.Clear();
                    var ViewList = DataProvider.Instance.DB.MedicineDetails.Where(x => x.ExamID == SelectedItem.ID);
                    foreach (var item in ViewList)
                    {
                        var TempMedicineDetail = new TempMedicineDetail()
                        {
                            Name = DataProvider.Instance.DB.Medicines.Where(x => x.ID == item.MedicineID).Single().Name,
                            Usage = DataProvider.Instance.DB.MedicineUsages.Where(x => x.ID == item.UsageID).Single().LongDsc,
                            Qty = item.Quantity,
                            Unit = GetUnit(DataProvider.Instance.DB.Medicines.Where(x => x.ID == item.MedicineID).Single().Name)
                        };
                        TempList.Add(TempMedicineDetail);
                    }
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

        private string _ID = IDGenerator.createID("PK");
        public string ID { get => _ID; set { _ID = value; OnPropertyChanged(); } }

        private DateTime? _EDate = null;
        public DateTime? EDate { get => _EDate; set { _EDate = value; OnPropertyChanged(); } }

        private string _Symptom = string.Empty;
        public string Symptom { get => _Symptom; set { _Symptom = value; OnPropertyChanged(); } }

        private string _PatientID = string.Empty;
        public string PatientID { get => _PatientID; set { _PatientID = value; OnPropertyChanged(); } }

        //disease details
        private Disease _NewDisease;
        public Disease NewDisease { get => _NewDisease; set { _NewDisease = value; OnPropertyChanged(); } }

        private ObservableCollection<Disease> _ThisDisease;
        public ObservableCollection<Disease> ThisDisease { get => _ThisDisease; set { _ThisDisease = value; OnPropertyChanged(); } }

        private ObservableCollection<Patient> _AllPatients;
        public ObservableCollection<Patient> AllPatients { get => _AllPatients; set { _AllPatients = value; OnPropertyChanged(); } }

        private ObservableCollection<Disease> _AllDiseases;
        public ObservableCollection<Disease> AllDiseases { get => _AllDiseases; set { _AllDiseases = value; OnPropertyChanged(); } }

        private ObservableCollection<Medicine> _AllMedicines;
        public ObservableCollection<Medicine> AllMedicines { get => _AllMedicines; set { _AllMedicines = value; OnPropertyChanged(); } }

        private ObservableCollection<MedicineUsage> _AllMedicineUsages;
        public ObservableCollection<MedicineUsage> AllMedicineUsages { get => _AllMedicineUsages; set { _AllMedicineUsages = value; OnPropertyChanged(); } }

        //medicine details
        private Medicine _TempMedicine;
        public Medicine TempMedicine { get => _TempMedicine; set { _TempMedicine = value; OnPropertyChanged(); } }

        private int _TempQty;
        public int TempQty { get => _TempQty; set { _TempQty = value; OnPropertyChanged(); } }

        private MedicineUsage _TempUsage;
        public MedicineUsage TempUsage { get => _TempUsage; set { _TempUsage = value; OnPropertyChanged(); } }

        private ObservableCollection<TempMedicineDetail> _TempList;
        public ObservableCollection<TempMedicineDetail> TempList { get => _TempList; set { _TempList = value; OnPropertyChanged(); } }

        private TempMedicineDetail _TempSelectedItem;
        public TempMedicineDetail TempSelectedItem
        {
            get => _TempSelectedItem;
            set
            {
                _TempSelectedItem = value;
                OnPropertyChanged();
                if (TempSelectedItem != null)
                {
                    TempMedicine = new Medicine() { Name = TempSelectedItem.Name };
                }
            }
        }

        public ICommand ClearCommand { get; set; }
        public ICommand CheckCommand { get; set; }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public ICommand SearchCommand { get; set; }
        public ICommand SelectDiseaseCommand { get; set; }
        public ICommand SelectMedicineCommand { get; set; }
        public ICommand RemoveMedicineCommand { get; set; }

        private void Clear()
        {
            ID = IDGenerator.createID("PK");
            EDate = DateTime.Now;
            Symptom = string.Empty;
            PatientID = string.Empty;
            ThisDisease.Clear();
            TempList.Clear();
        }

        private List<Disease> GetDiseases()
        {
            var result = from dd in DataProvider.Instance.DB.DiseaseDetails
                         join d in DataProvider.Instance.DB.Diseases on dd.DiseaseID equals d.ID
                         where dd.ExamID == SelectedItem.ID
                         select d;
            return result.ToList();
        }

        private string GetUnit(string name)
        {
            var result = (from m in DataProvider.Instance.DB.Medicines
                         join mu in DataProvider.Instance.DB.MedicineUnits on m.UnitID equals mu.ID
                         where m.Name == name
                         select mu).FirstOrDefault();
            return result.Name;
        }

        public ExaminationFormViewModel()
        {
            List = new ObservableCollection<ExaminationForm>(DataProvider.Instance.DB.ExaminationForms);
            AllPatients = new ObservableCollection<Patient>(DataProvider.Instance.DB.Patients);
            AllDiseases = new ObservableCollection<Disease>(DataProvider.Instance.DB.Diseases);
            AllMedicines = new ObservableCollection<Medicine>(DataProvider.Instance.DB.Medicines);
            AllMedicineUsages = new ObservableCollection<MedicineUsage>(DataProvider.Instance.DB.MedicineUsages);
            ThisDisease = new ObservableCollection<Disease>();
            NewDisease = new Disease();
            TempMedicine = new Medicine();
            TempUsage = new MedicineUsage();
            TempQty = 1;
            TempList = new ObservableCollection<TempMedicineDetail>();

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
                if (string.IsNullOrEmpty(Symptom) || ThisDisease.Count() == 0)
                    return false;

                return true;
            }, (p) =>
            {
                var ExaminationForm = new ExaminationForm() { ID = ID, EDate = EDate, Symptom = Symptom, PatientID = PatientID };

                DataProvider.Instance.DB.ExaminationForms.Add(ExaminationForm);
                DataProvider.Instance.DB.SaveChanges();

                List.Add(ExaminationForm);

                int Sum = 0;

                foreach (var item in ThisDisease)
                {
                    var DiseaseDetail = new DiseaseDetail() { ExamID = ID, DiseaseID = DataProvider.Instance.DB.Diseases.Where(x => x.Name == item.Name).Single().ID };
                    Sum += (int)DataProvider.Instance.DB.Diseases.Where(x => x.Name == item.Name).Single().Price;
                    DataProvider.Instance.DB.DiseaseDetails.Add(DiseaseDetail);
                    DataProvider.Instance.DB.SaveChanges();
                }

                foreach (var item in TempList)
                {
                    var MedicineDetail = new MedicineDetail()
                    {
                        ExamID = ID,
                        MedicineID = DataProvider.Instance.DB.Medicines.Where(x => x.Name == item.Name).Single().ID,
                        UsageID = DataProvider.Instance.DB.MedicineUsages.Where(x => x.LongDsc == item.Usage).Single().ID,
                        Quantity = item.Qty
                    };
                    Sum += (int)DataProvider.Instance.DB.Medicines.Where(x => x.Name == item.Name).Single().Price;
                    DataProvider.Instance.DB.MedicineDetails.Add(MedicineDetail);
                    DataProvider.Instance.DB.SaveChanges();
                }

                var NewInvoice = new Invoice()
                {
                    ID = IDGenerator.createID("HD"),
                    IDdate = DateTime.Now,
                    Total = Sum,
                    PatientID = PatientID,
                    Account = "tuan",
                    ExamID = ID,
                    Status = "Chưa thanh toán"
                };
                DataProvider.Instance.DB.Invoices.Add(NewInvoice);
                DataProvider.Instance.DB.SaveChanges();

                Clear();
            });

            SearchCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (Keyword == "")
                {
                    List = new ObservableCollection<ExaminationForm>(DataProvider.Instance.DB.ExaminationForms);
                }
                else
                {
                    var result = from ef in DataProvider.Instance.DB.ExaminationForms.ToList()
                                 join patients in DataProvider.Instance.DB.Patients.ToList() on ef.PatientID equals patients.ID
                                 where (ef.ID.ToLower().AccentRemoved().Contains(Keyword.ToLower().AccentRemoved()) || patients.Name.ToLower().AccentRemoved().Contains(Keyword.ToLower().AccentRemoved()) || patients.Phone.Contains(Keyword))
                                 select ef;

                    List = new ObservableCollection<ExaminationForm>(result);
                }
            });

            SelectDiseaseCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ThisDisease.Add(NewDisease);
                NewDisease = new Disease();
            });

            SelectMedicineCommand = new RelayCommand<object>((p) =>
            {
                var isExisted = DataProvider.Instance.DB.Patients.Where(x => x.ID == ID);

                foreach (var item in TempList)
                {
                    if (item.Name == TempMedicine.Name)
                        return false;
                }

                return true;
            }, (p) =>
            {
                var NewMedicineDetail = new TempMedicineDetail()
                { 
                    Name = TempMedicine.Name, 
                    Usage = DataProvider.Instance.DB.MedicineUsages.Where(x => x.ShortDsc == TempUsage.ShortDsc).Single().LongDsc, 
                    Qty = TempQty, 
                    Unit = GetUnit(TempMedicine.Name)
                };
                TempList.Add(NewMedicineDetail);
                TempMedicine = new Medicine();
                TempUsage = new MedicineUsage();
                TempQty = 1;
            });

            RemoveMedicineCommand = new RelayCommand<object>((p) =>
            {
                if (TempSelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                var item = TempList.Single(x => x.Name == TempMedicine.Name);
                TempList.Remove(item);
                TempMedicine = new Medicine();
            });
        }
    }
    class TempMedicineDetail : BaseViewModel
    {
        private string _Name;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }

        private string _Usage;
        public string Usage { get => _Usage; set { _Usage = value; OnPropertyChanged(); } }

        private int _Qty;
        public int Qty { get => _Qty; set { _Qty = value; OnPropertyChanged(); } }

        private string _Unit;
        public string Unit { get => _Unit; set { _Unit = value; OnPropertyChanged(); } }
    }
}
