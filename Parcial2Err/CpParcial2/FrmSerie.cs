using CadParcial2;
using ClnParcial2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpParcial2
{
    public partial class FrmSerie : Form
    {
        bool esNuevo = false;
        public FrmSerie()
        {
            InitializeComponent();
        }

        private void listar()
        {
            var series = SerieCln.listarPa(txtParametro.Text.Trim());
            dgvLista.DataSource = series;
            dgvLista.Columns["id"].Visible = false;
            dgvLista.Columns["estado"].Visible = false;
            dgvLista.Columns["titulo"].HeaderText = "TÍTULO";
            dgvLista.Columns["sinopsis"].HeaderText = "SINOPSIS";
            dgvLista.Columns["director"].HeaderText = "DIRECTOR";
            dgvLista.Columns["duracion"].HeaderText = "DURACIÓN";
            dgvLista.Columns["fechaEstreno"].HeaderText = "FECHA DE ESTRENO";
            dgvLista.Columns["usuarioRegistro"].HeaderText = "USUARIO REGISTRO";
            dgvLista.Columns["fechaRegistro"].HeaderText = "FECHA REGISTRO";
            btnEditar.Enabled = series.Count > 0;
            btnEliminar.Enabled = series.Count > 0;
            if (series.Count > 0) dgvLista.Rows[0].Cells["titulo"].Selected = true;
        }

        

        private void FrmSerie_Load_1(object sender, EventArgs e)
        {
            Size = new Size(1099, 410);
            listar();
        }

        private void btnNuevo_Click_1(object sender, EventArgs e)
        {
            esNuevo = true;
            Size = new Size(1099, 550);
            limpiar();
            txtTitulo.Focus();
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            esNuevo = false;
            Size = new Size(1099, 550);

            int index = dgvLista.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
            var serie = SerieCln.get(id);
            txtTitulo.Text = serie.titulo;
            txtSinopsis.Text = serie.sinopsis;
            txtDirector.Text = serie.director;
            nudDuracion.Value = serie.duracion;
            txtFechaEstreno.Text = Convert.ToString(serie.fechaEstreno);
        }
        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            Size = new Size(1099, 410);
            limpiar();
        }
       
        private void btnCerrar_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            listar();
        }

        private void txtParametro_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }

        private bool validar()
        {
            bool esValido = true;
            erpTitulo.SetError(txtTitulo, "");
            erpSinopsis.SetError(txtSinopsis, "");
            erpDirector.SetError(txtDirector, "");
            erpDuracion.SetError(nudDuracion, "");
            erpFechaEstreno.SetError(txtFechaEstreno, "");
            if (string.IsNullOrEmpty(txtTitulo.Text))
            {
                esValido = false;
                erpTitulo.SetError(txtTitulo, "El campo Título es obligatorio");
            }
            if (string.IsNullOrEmpty(txtSinopsis.Text))
            {
                esValido = false;
                erpSinopsis.SetError(txtSinopsis, "El campo Sinopsis es obligatorio");
            }
            if (string.IsNullOrEmpty(txtDirector.Text))
            {
                esValido = false;
                erpDirector.SetError(txtDirector, "El campo Director es obligatorio");
            }
             if (string.IsNullOrEmpty(nudDuracion.Text))
             {
                 esValido = false;
                 erpDuracion.SetError(nudDuracion, "El campo Duración es obligatorio");
             }
             if (nudDuracion.Value < 0)
             {
                 esValido = false;
                 erpDuracion.SetError(nudDuracion, "El campo Duración debe ser mayor o igual a 0");
             }
             if (string.IsNullOrEmpty(txtFechaEstreno.Text))
             {
                 esValido = false;
                 erpFechaEstreno.SetError(txtFechaEstreno, "El campo Precio fecha de estreno es obligatorio");
             }            

            return esValido;
        }


        private void btnGuardar_Click_1(object sender, EventArgs e)
        {
            if (validar())
            {
                var serie = new Serie();
                serie.titulo = txtTitulo.Text.Trim();
                serie.sinopsis = txtSinopsis.Text.Trim();
                serie.director = txtDirector.Text.Trim();
                serie.duracion = Convert.ToInt32(nudDuracion.Value);
                serie.fechaEstreno = Convert.ToDateTime(txtFechaEstreno.Text.Trim());
                serie.usuarioRegistro = "SIS457-Parcial2";

                if (esNuevo)
                {
                    serie.fechaRegistro = DateTime.Now;
                    serie.estado = 1;
                    SerieCln.insertar(serie);
                }
                else
                {
                    int index = dgvLista.CurrentCell.RowIndex;
                    serie.id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
                    SerieCln.actualizar(serie);
                }
                listar();
                btnCancelar.PerformClick();
                MessageBox.Show("Serie guardado correctamente", "::: Parcial2 - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        

        private void limpiar()
        {
            txtTitulo.Text = string.Empty;
            txtSinopsis.Text = string.Empty;
            txtDirector.Text = string.Empty;
            nudDuracion.Text = string.Empty;
            txtFechaEstreno.Text = string.Empty;
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            int index = dgvLista.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
            string titulo = dgvLista.Rows[index].Cells["titulo"].Value.ToString();
            DialogResult dialog = MessageBox.Show($"¿Está seguro que desea dar de baja la serie {titulo}?",
                "::: Parcial2 - Mensaje :::", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialog == DialogResult.OK)
            {
                SerieCln.eliminar(id, "SIS457-Parcial2");
                listar();
                MessageBox.Show("Serie dado de baja correctamente", "::: Parcial2 - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtTitulo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) { 
                if (validar())
            {
                var serie = new Serie();
                serie.titulo = txtTitulo.Text.Trim();
                serie.sinopsis = txtSinopsis.Text.Trim();
                serie.director = txtDirector.Text.Trim();
                serie.duracion = Convert.ToInt32(nudDuracion.Value);
                serie.fechaEstreno = Convert.ToDateTime(txtFechaEstreno.Text.Trim());
                serie.usuarioRegistro = "SIS457-Parcial2";

                if (esNuevo)
                {
                    serie.fechaRegistro = DateTime.Now;
                    serie.estado = 1;
                    SerieCln.insertar(serie);
                }
                else
                {
                    int index = dgvLista.CurrentCell.RowIndex;
                    serie.id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
                    SerieCln.actualizar(serie);
                }
                listar();
                btnCancelar.PerformClick();
                MessageBox.Show("Serie guardado correctamente", "::: Parcial2 - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            }
        }

        private void txtSinopsis_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (validar())
                {
                    var serie = new Serie();
                    serie.titulo = txtTitulo.Text.Trim();
                    serie.sinopsis = txtSinopsis.Text.Trim();
                    serie.director = txtDirector.Text.Trim();
                    serie.duracion = Convert.ToInt32(nudDuracion.Value);
                    serie.fechaEstreno = Convert.ToDateTime(txtFechaEstreno.Text.Trim());
                    serie.usuarioRegistro = "SIS457-Parcial2";

                    if (esNuevo)
                    {
                        serie.fechaRegistro = DateTime.Now;
                        serie.estado = 1;
                        SerieCln.insertar(serie);
                    }
                    else
                    {
                        int index = dgvLista.CurrentCell.RowIndex;
                        serie.id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
                        SerieCln.actualizar(serie);
                    }
                    listar();
                    btnCancelar.PerformClick();
                    MessageBox.Show("Serie guardado correctamente", "::: Parcial2 - Mensaje :::",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void txtDirector_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (validar())
                {
                    var serie = new Serie();
                    serie.titulo = txtTitulo.Text.Trim();
                    serie.sinopsis = txtSinopsis.Text.Trim();
                    serie.director = txtDirector.Text.Trim();
                    serie.duracion = Convert.ToInt32(nudDuracion.Value);
                    serie.fechaEstreno = Convert.ToDateTime(txtFechaEstreno.Text.Trim());
                    serie.usuarioRegistro = "SIS457-Parcial2";

                    if (esNuevo)
                    {
                        serie.fechaRegistro = DateTime.Now;
                        serie.estado = 1;
                        SerieCln.insertar(serie);
                    }
                    else
                    {
                        int index = dgvLista.CurrentCell.RowIndex;
                        serie.id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
                        SerieCln.actualizar(serie);
                    }
                    listar();
                    btnCancelar.PerformClick();
                    MessageBox.Show("Serie guardado correctamente", "::: Parcial2 - Mensaje :::",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void nudDuracion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (validar())
                {
                    var serie = new Serie();
                    serie.titulo = txtTitulo.Text.Trim();
                    serie.sinopsis = txtSinopsis.Text.Trim();
                    serie.director = txtDirector.Text.Trim();
                    serie.duracion = Convert.ToInt32(nudDuracion.Value);
                    serie.fechaEstreno = Convert.ToDateTime(txtFechaEstreno.Text.Trim());
                    serie.usuarioRegistro = "SIS457-Parcial2";

                    if (esNuevo)
                    {
                        serie.fechaRegistro = DateTime.Now;
                        serie.estado = 1;
                        SerieCln.insertar(serie);
                    }
                    else
                    {
                        int index = dgvLista.CurrentCell.RowIndex;
                        serie.id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
                        SerieCln.actualizar(serie);
                    }
                    listar();
                    btnCancelar.PerformClick();
                    MessageBox.Show("Serie guardado correctamente", "::: Parcial2 - Mensaje :::",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void txtFechaEstreno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (validar())
                {
                    var serie = new Serie();
                    serie.titulo = txtTitulo.Text.Trim();
                    serie.sinopsis = txtSinopsis.Text.Trim();
                    serie.director = txtDirector.Text.Trim();
                    serie.duracion = Convert.ToInt32(nudDuracion.Value);
                    serie.fechaEstreno = Convert.ToDateTime(txtFechaEstreno.Text.Trim());
                    serie.usuarioRegistro = "SIS457-Parcial2";

                    if (esNuevo)
                    {
                        serie.fechaRegistro = DateTime.Now;
                        serie.estado = 1;
                        SerieCln.insertar(serie);
                    }
                    else
                    {
                        int index = dgvLista.CurrentCell.RowIndex;
                        serie.id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
                        SerieCln.actualizar(serie);
                    }
                    listar();
                    btnCancelar.PerformClick();
                    MessageBox.Show("Serie guardado correctamente", "::: Parcial2 - Mensaje :::",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
