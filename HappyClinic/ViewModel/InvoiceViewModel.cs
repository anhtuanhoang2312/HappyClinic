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
    class InvoiceViewModel : BaseViewModel
    {
        private ObservableCollection<Invoice> _List;
        public ObservableCollection<Invoice> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private Invoice _SelectedItem;
        public Invoice SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    ID = SelectedItem.ID;
                    IDate = SelectedItem.IDdate;
                    Total = (int)SelectedItem.Total;
                    PatientID = SelectedItem.PatientID;
                    Account = SelectedItem.Account;
                    ExamID = SelectedItem.ExamID;
                    Status = (SelectedItem.Status == "Đã thanh toán" ? "0" : "1");
                    PatientInfo = DataProvider.Instance.DB.Patients.Where(x => x.ID == SelectedItem.PatientID).Single();
                    ExamInfo = DataProvider.Instance.DB.ExaminationForms.Where(x => x.ID == SelectedItem.ExamID).Single();

                    int Sum = 0;
                    var DiseaseList = DataProvider.Instance.DB.DiseaseDetails.Where(x => x.ExamID == ExamInfo.ID);
                    foreach (var item in DiseaseList)
                    {
                        var Detail = new IDiseaseDetail()
                        {
                            Name = DataProvider.Instance.DB.Diseases.Where(x => x.ID == item.DiseaseID).Single().Name,
                            Qty = 1,
                            Price = (int)DataProvider.Instance.DB.Diseases.Where(x => x.ID == item.DiseaseID).Single().Price,
                            Total = (int)DataProvider.Instance.DB.Diseases.Where(x => x.ID == item.DiseaseID).Single().Price
                        };
                        Sum += Detail.Total;
                        AllDiseases.Add(Detail);
                    }

                    var MedicineList = DataProvider.Instance.DB.MedicineDetails.Where(x => x.ExamID == ExamInfo.ID);
                    foreach (var item in MedicineList)
                    {
                        var Detail = new IMedicineDetail()
                        {
                            Name = DataProvider.Instance.DB.Medicines.Where(x => x.ID == item.MedicineID).Single().Name,
                            Qty = item.Quantity,
                            Price = (int)DataProvider.Instance.DB.Medicines.Where(x => x.ID == item.MedicineID).Single().Price,
                            Total = item.Quantity * (int)DataProvider.Instance.DB.Medicines.Where(x => x.ID == item.MedicineID).Single().Price
                        };
                        Sum += Detail.Total;
                        AllMedicines.Add(Detail);
                    }

                    Total = Sum;
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

        private string _ID = IDGenerator.createID("HD");
        public string ID { get => _ID; set { _ID = value; OnPropertyChanged(); } }

        private DateTime? _IDate = null;
        public DateTime? IDate { get => _IDate; set { _IDate = value; OnPropertyChanged(); } }

        private int _Total = 0;
        public int Total { get => _Total; set { _Total = value; OnPropertyChanged(); } }

        private string _PatientID = string.Empty;
        public string PatientID { get => _PatientID; set { _PatientID = value; OnPropertyChanged(); } }

        private string _Account = string.Empty;
        public string Account { get => _Account; set { _Account = value; OnPropertyChanged(); } }

        private string _ExamID = string.Empty;
        public string ExamID { get => _ExamID; set { _ExamID = value; OnPropertyChanged(); } }

        private string _Status = string.Empty;
        public string Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }

        private Patient _PatientInfo;
        public Patient PatientInfo { get => _PatientInfo; set { _PatientInfo = value; OnPropertyChanged(); } }

        private ExaminationForm _ExamInfo;
        public ExaminationForm ExamInfo { get => _ExamInfo; set { _ExamInfo = value; OnPropertyChanged(); } }

        private ObservableCollection<IDiseaseDetail> _AllDiseases;
        public ObservableCollection<IDiseaseDetail> AllDiseases { get => _AllDiseases; set { _AllDiseases = value; OnPropertyChanged(); } }

        private ObservableCollection<IMedicineDetail> _AllMedicines;
        public ObservableCollection<IMedicineDetail> AllMedicines { get => _AllMedicines; set { _AllMedicines = value; OnPropertyChanged(); } }

        public ICommand ClearCommand { get; set; }
        public ICommand CheckCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        private void Clear()
        {
            ID = IDGenerator.createID("HD");
            IDate = null;
            Total = 0;
            PatientID = string.Empty;
            Account = string.Empty;
            ExamID = string.Empty;
            Status = string.Empty;
        }

        public InvoiceViewModel()
        {
            List = new ObservableCollection<Invoice>(DataProvider.Instance.DB.Invoices);
            PatientInfo = new Patient();
            ExamInfo = new ExaminationForm();
            AllDiseases = new ObservableCollection<IDiseaseDetail>();
            AllMedicines = new ObservableCollection<IMedicineDetail>();

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

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                var Invoice = DataProvider.Instance.DB.Invoices.Where(x => x.ID == SelectedItem.ID).Single();
                Invoice.Status = (Status == "0" ? "Đã thanh toán" : "Chưa thanh toán");
                Invoice.Total = Total;

                DataProvider.Instance.DB.SaveChanges();

                List = new ObservableCollection<Invoice>(DataProvider.Instance.DB.Invoices);
            });

            SearchCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (Keyword == "")
                {
                    List = new ObservableCollection<Invoice>(DataProvider.Instance.DB.Invoices);
                }
                else
                {
                    var result = from i in DataProvider.Instance.DB.Invoices.ToList()
                                 join patients in DataProvider.Instance.DB.Patients.ToList() on i.PatientID equals patients.ID
                                 where (i.ID.ToLower().AccentRemoved().Contains(Keyword.ToLower().AccentRemoved()) || patients.Name.ToLower().AccentRemoved().Contains(Keyword.ToLower().AccentRemoved()) || patients.Phone.Contains(Keyword))
                                 select i;

                    List = new ObservableCollection<Invoice>(result);
                }
            });
        }
    }

    class IDiseaseDetail : BaseViewModel
    {
        private string _Name;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }

        private int _Qty;
        public int Qty { get => _Qty; set { _Qty = value; OnPropertyChanged(); } }

        private int _Price;
        public int Price { get => _Price; set { _Price = value; OnPropertyChanged(); } }

        private int _Total;
        public int Total { get => _Total; set { _Total = value; OnPropertyChanged(); } }
    }

    class IMedicineDetail : BaseViewModel
    {
        private string _Name;
        public string Name { get => _Name; set { _Name = value; OnPropertyChanged(); } }

        private int _Qty;
        public int Qty { get => _Qty; set { _Qty = value; OnPropertyChanged(); } }

        private int _Price;
        public int Price { get => _Price; set { _Price = value; OnPropertyChanged(); } }

        private int _Total;
        public int Total { get => _Total; set { _Total = value; OnPropertyChanged(); } }
    }
}
