using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyClinic.Model
{ 
    class IDGenerator
    {
        public static string createID(string prefix)
        {
            var count = 0;

            if (prefix == "BN")
            {
                var table = DataProvider.Instance.DB.Patients;
                if (table.Count() > 0)
                {
                    count = int.Parse(table.ToList()[table.Count() - 1].ID.ToString().Substring(2, 3));
                }
            }
            else if (prefix == "PK")
            {
                var table = DataProvider.Instance.DB.ExaminationForms;
                if (table.Count() > 0)
                {
                    count = int.Parse(table.ToList()[table.Count() - 1].ID.ToString().Substring(2, 3));
                }
            }
            else if (prefix == "HD")
            {
                var table = DataProvider.Instance.DB.Invoices;
                if (table.Count() > 0)
                {
                    count = int.Parse(table.ToList()[table.Count() - 1].ID.ToString().Substring(2, 3));
                }
            }
            else if (prefix == "T")
            {
                var table = DataProvider.Instance.DB.Medicines;
                if (table.Count() > 0)
                {
                    count = int.Parse(table.ToList()[table.Count() - 1].ID.ToString().Substring(1, 3));
                }
            }
            else if (prefix == "CD")
            {
                var table = DataProvider.Instance.DB.MedicineUsages;
                if (table.Count() > 0)
                {
                    count = int.Parse(table.ToList()[table.Count() - 1].ID.ToString().Substring(2, 3));
                }
            }
            else if (prefix == "DV")
            {
                var table = DataProvider.Instance.DB.MedicineUnits;
                if (table.Count() > 0)
                {
                    count = int.Parse(table.ToList()[table.Count() - 1].ID.ToString().Substring(2, 3));
                }
            }
            else if (prefix == "B")
            {
                var table = DataProvider.Instance.DB.Diseases;
                if (table.Count() > 0)
                {
                    count = int.Parse(table.ToList()[table.Count() - 1].ID.ToString().Substring(1, 3));
                }
            }

            return prefix + (count + 1).ToString("000");
        }
    }
}
