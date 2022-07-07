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
    public partial class productos : Form
    {
        SqlConnection con;
        public productos()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void productos_Load(object sender, EventArgs e)
        {
            String str = "Server=DESKTOP-I9SU5QR\\LOCAL;Integrated Security=true;Database=ferreteria;";
            con = new SqlConnection(str);
            con.Open();
            String sql = "SELECT * from producto";
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);
            dgvProductos.DataSource = dt;
            dgvProductos.Refresh();

            String sql2 = "SELECT * from proveedor";
            SqlCommand cmd2 = new SqlCommand(sql2, con);
            SqlDataReader reader2 = cmd2.ExecuteReader();
  
            while (reader2.Read())
            {
                int pname = reader2.GetInt32(0);
                txtProveedor.Items.Add(pname);
            }
            con.Close();
        }

        public class Proveedor
        {
            public string codigo { get; set; }
            public string nombre { get; set; }

            public string ruc { get; set; }
            public string direccion { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            String sql = String.Format("SELECT * FROM producto WHERE nombre = '{0}'", txtbuscarnombre.Text);
            Console.WriteLine(sql);
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);
            dgvProductos.DataSource = dt;
            dgvProductos.Refresh();
            con.Close();
        }

        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProductos.SelectedRows.Count > 0)
            {
                txtCodigo.Text = dgvProductos.SelectedRows[0].Cells[0].Value.ToString();
                txtNombre.Text = dgvProductos.SelectedRows[0].Cells[1].Value.ToString();
                txtMarca.Text = dgvProductos.SelectedRows[0].Cells[2].Value.ToString();
                txtCategoria.Text = dgvProductos.SelectedRows[0].Cells[3].Value.ToString();
                txtPreventa.Text = dgvProductos.SelectedRows[0].Cells[4].Value.ToString();
                txtPrecompra.Text = dgvProductos.SelectedRows[0].Cells[5].Value.ToString();
                txtProveedor.Text = dgvProductos.SelectedRows[0].Cells[6].Value.ToString();
                txtUnidad.Text = dgvProductos.SelectedRows[0].Cells[7].Value.ToString();
                txtStock.Text = dgvProductos.SelectedRows[0].Cells[8].Value.ToString();
                dtFecha.Text = dgvProductos.SelectedRows[0].Cells[9].Value.ToString();
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            con.Open();
            String sql = String.Format("INSERT INTO producto(nombre, marca, categoria, precio_venta, precio_compra, proveedor_id, unidad, stock, fecha_ingreso) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", 
                txtNombre.Text, txtMarca.Text, txtCategoria.Text, txtPreventa.Text, txtPrecompra.Text,
                txtProveedor.Text, txtUnidad.Text, txtStock.Text, dtFecha.Text);
            Console.WriteLine(sql);
            SqlCommand cmd = new SqlCommand(sql, con);
            int codigo = Convert.ToInt32(cmd.ExecuteScalar());
            MessageBox.Show("Se ha registrado un nuevo producto");
            String sql2 = "SELECT * from producto";
            SqlCommand cmd2 = new SqlCommand(sql2, con);
            SqlDataReader reader = cmd2.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            dgvProductos.DataSource = dt;
            dgvProductos.Refresh();
            con.Close();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtMarca.Text = "";
            txtCategoria.Text = "";
            txtPreventa.Text = "";
            txtPrecompra.Text = "";
            txtProveedor.Text = "";
            txtUnidad.Text = "";
            txtStock.Text = "";
            dtFecha.Text = "";
            txtbuscarnombre.Text = "";

            con.Open();
            String sql = "SELECT * from producto";
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);
            dgvProductos.DataSource = dt;
            dgvProductos.Refresh();
            con.Close();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            con.Open();
            String sql = String.Format("UPDATE producto SET nombre='{0}', marca='{1}', categoria='{2}', precio_venta='{3}', precio_compra='{4}', proveedor_id='{5}', unidad='{6}', stock='{7}', fecha_ingreso='{8}' WHERE codigo='{9}'",
                txtNombre.Text, txtMarca.Text, txtCategoria.Text, txtPreventa.Text, txtPrecompra.Text,
                txtProveedor.Text, txtUnidad.Text, txtStock.Text, dtFecha.Text, txtCodigo.Text);
            Console.WriteLine(sql);
            SqlCommand cmd = new SqlCommand(sql, con);
            int codigo = Convert.ToInt32(cmd.ExecuteScalar());
            MessageBox.Show("Se ha editado el producto");
            String sql2 = "SELECT * from producto";
            SqlCommand cmd2 = new SqlCommand(sql2, con);
            SqlDataReader reader = cmd2.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            dgvProductos.DataSource = dt;
            dgvProductos.Refresh();
            con.Close();

        }
    }
}
