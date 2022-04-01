using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Komis
{
    public partial class AddCar : Form
    {
        private static bool exist = false;
        public AddCar()
        {
            InitializeComponent();
            exist = true;
        }

        public static bool GetExist() { return exist; }
    }
}
