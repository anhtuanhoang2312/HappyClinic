using HandyControl.Controls;
using HappyClinic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFCustomMessageBox;


namespace HappyClinic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = $" HappyClinic (Bạn đang đăng nhập với tư cách {CurrentUser.Instance.User.Username.ToUpper()})";
            if (CurrentUser.Instance.User.isAdmin == true)
            {
                Setting.Visibility = Visibility.Visible;
            }
            else
            {
                Setting.Visibility = Visibility.Collapsed;
            }
        }

        private void sideMenu_Selected(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as SideMenuItem;
            if (item != null)
            {
                if (item.Name == "Patient")
                {
                    ViewSelector.Select(MainContent, new PatientWindow());
                }
                else if (item.Name == "ExaminationForm")
                {
                    ViewSelector.Select(MainContent, new ExaminationFormWindow());
                }
                else if (item.Name == "Invoice")
                {
                    ViewSelector.Select(MainContent, new InvoiceWindow());
                }
                else if (item.Name == "Medicine")
                {
                    ViewSelector.Select(MainContent, new MedicineWindow());
                }
                else if (item.Name == "MedicineUsage")
                {
                    ViewSelector.Select(MainContent, new MedicineUsageWindow());
                }
                else if (item.Name == "MedicineUnit")
                {
                    ViewSelector.Select(MainContent, new MedicineUnitWindow());
                }
                else if (item.Name == "Disease")
                {
                    ViewSelector.Select(MainContent, new DiseaseWindow());
                }
                else if (item.Name == "Account")
                {
                    ViewSelector.Select(MainContent, new AccountWindow());
                }
                else if (item.Name == "LogOut")
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        LogInWindow login = new LogInWindow();
                        login.Show();
                        this.Close();                      
                    }
                }
            }
        }
    }
}
