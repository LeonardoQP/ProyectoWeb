using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProyectoFerreteria
{
    public partial class inicio : Form
    {
        SqlConnection con;
        public inicio()
        {
            InitializeComponent();
        }

        private void inicio_Load(object sender, EventArgs e)
        {
            String str = "Server=DESKTOP-I9SU5QR\\LOCAL;Integrated Security=true;Database=ferreteria;";
            con = new SqlConnection(str);
        }
    }
}
