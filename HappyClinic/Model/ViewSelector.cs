using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HappyClinic.Model
{
    class ViewSelector
    {
        public static void Select(Grid g, UserControl uc)
        {
            if (g.Children.Count > 0)
            {
                g.Children.Clear();
                g.Children.Add(uc);
            }
            else
            {
                g.Children.Add(uc);
            }
        }
    }
}
