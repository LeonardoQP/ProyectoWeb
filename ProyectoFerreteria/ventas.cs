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
    public partial class ventas : Form
    {
        SqlConnection con;
        public ventas()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void dtFecha_ValueChanged(object sender, EventArgs e)
        {

        }

        private void ventas_Load(object sender, EventArgs e)
        {
            String str = "Server=DESKTOP-I9SU5QR\\LOCAL;Integrated Security=true;Database=ferreteria;";
            con = new SqlConnection(str);
            con.Open();
            String sql2 = "SELECT * from producto";
            SqlCommand cmd2 = new SqlCommand(sql2, con);
            SqlDataReader reader2 = cmd2.ExecuteReader();

            while (reader2.Read())
            {
                int pname = reader2.GetInt32(0);
                txtCodigo.Items.Add(pname);
            }
            con.Close();
        }

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
            con.Open();
            String sql = String.Format("SELECT codigo, nombre, precio_venta, stock FROM producto WHERE codigo = {0}", txtCodigo.Text);
            Console.WriteLine(sql);
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                txtCodigo.Text = reader.GetInt32(0).ToString();
                txtNombre.Text = reader.GetString(1);
                txtPrecio.Text = reader.GetDecimal(2).ToString("0.00");
                txtStock.Text = reader.GetInt32(3).ToString();
            }
            con.Close();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            decimal precio = 0;
            bool producto_existe = false;

            if (int.Parse(txtCodigo.Text) == 0)
            {
                MessageBox.Show("Debe Seleccionar un producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (Convert.ToInt32(txtStock.Text) < Convert.ToInt32(txtCantidad.Text))
            {
                MessageBox.Show("La cantidad no debe ser mayor al stock", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (DataGridViewRow fila in dgvVenta.Rows)
            {
                if (fila.Cells["Producto"].Value?.ToString() == txtNombre.Text)
                {
                    producto_existe = true;
                    break;
                }
            }

            decimal subt = Convert.ToDecimal(txtPrecio.Text) * Convert.ToDecimal(txtCantidad.Text);
            Console.WriteLine(subt);
            if (!producto_existe)
            {
                dgvVenta.Rows.Add(new object[] {
                    txtCodigo.Text,
                    txtNombre.Text,
                    txtPrecio.Text,
                    txtCantidad.Text,
                    subt
                });

                calcularTotal();
            }
        }

        private void calcularTotal()
        {
            decimal total = 0;

            if (dgvVenta.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvVenta.Rows)
                {
                    total += Convert.ToDecimal(row.Cells["SubTotal"].Value?.ToString());
                }
                txtTotal.Text = total.ToString("0.00");
            }
        }

        private void calcularCambio()
        {
            if (txtTotal.Text.Trim() == "")
            {
                MessageBox.Show("No existen productos en la venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            decimal pagacon;
            decimal total = Convert.ToDecimal(txtTotal.Text);

            if (txtPago.Text.Trim() == "")
            {
                txtPago.Text = "0";
            }

            if (decimal.TryParse(txtPago.Text.Trim(), out pagacon))
            {
                if (pagacon < total)
                {
                    txtCambio.Text = "0.00";
                }
                else
                {
                    decimal cambio = pagacon - total;
                    txtCambio.Text = cambio.ToString("0.00");
                }
            }
        }

        private void txtPago_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                calcularCambio();
            }
        }

        private void btnVenta_Click(object sender, EventArgs e)
        {
            bool Respuesta = false;
            string Mensaje = string.Empty;
            con.Open();
            DataTable detalle_venta = new DataTable();

            detalle_venta.Columns.Add("IdProducto", typeof(int));
            detalle_venta.Columns.Add("PrecioVenta", typeof(decimal));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("SubTotal", typeof(decimal));

            foreach (DataGridViewRow row in dgvVenta.Rows)
            {
                detalle_venta.Rows.Add(new object[] {
                    row.Cells["IdProducto"].Value?.ToString(),
                    row.Cells["Precio"].Value?.ToString(),
                    row.Cells["Cantidad"].Value?.ToString(),
                    row.Cells["SubTotal"].Value?.ToString()
                });
            }

            SqlCommand cmd = new SqlCommand("usp_RegistrarVenta", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("MontoPago", Convert.ToDecimal(txtPago.Text));
            cmd.Parameters.AddWithValue("MontoCambio", Convert.ToDecimal(txtCambio.Text));
            cmd.Parameters.AddWithValue("MontoTotal", Convert.ToDecimal(txtTotal.Text));
            cmd.Parameters.AddWithValue("DetalleVenta", detalle_venta);
            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            Respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
            Mensaje = cmd.Parameters["Mensaje"].Value?.ToString();
            Console.WriteLine(Mensaje);
            con.Close();
        }
            
    
    }
}
