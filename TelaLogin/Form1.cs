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
using System.IO;
using System.Globalization;
using System.Security.Cryptography;

namespace TelaLogin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Acces acces = new Acces();
            //acces.Login("Guilherme", "123");
            InitializeComponent();
            if (File.Exists("senhasalva.txt"))
            {
                var lines = File.ReadAllText("senhasalva.txt");
                tbUsuario.Text = lines[0].ToString();
                tbsenha.Text = lines[1].ToString();
                cbSalvarSenha.Checked = true;
            }
            else
            {
                File.Delete("senhasalva.txt");
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cbMostrarSenha_CheckedChanged(object sender, EventArgs e)
        {
            if (cbMostrarSenha.Checked)
            {
                tbsenha.PasswordChar = '\0';
            }
            else
            {
                tbsenha.PasswordChar = '●';
            }
        }

        private void cbSalvarSenha_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string senhaEncriptada = "";

            using (HashAlgorithm alg = SHA1.Create())
            {
                var encoding = new UnicodeEncoding();
                byte[] bytes = alg.ComputeHash(encoding.GetBytes(tbsenha.Text));
                StringBuilder sb = new StringBuilder();
                foreach (var b in bytes)
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", b);
                }
                senhaEncriptada = sb.ToString();
            }

            AulaSenaiEntities entities = new AulaSenaiEntities();
            var user = entities.Usuario.FirstOrDefault(u => u.Nome == tbUsuario.Text && u.Senha == senhaEncriptada);


            if (user != null)
            {
                if (cbSalvarSenha.Checked)
                {
                    File.WriteAllText("senhasalva.txt", $"{user.Nome}\n{tbsenha.Text}");
                }
                MessageBox.Show($"Bem-vindo {user.Nome}!", "Bem-vindo!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Senha ou Usuario Incorretos!", "Erro!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbUsuario_TextChanged(object sender, EventArgs e)
        {
            btnLogin.Visible = tbUsuario.Text.Length > 0 && tbsenha.Text.Length > 0;
        }

        private void tbSenha_TextChanged(object sender, EventArgs e)
        {
            btnLogin.Visible = tbUsuario.Text.Length > 0 && tbsenha.Text.Length > 0;
        }
    }
}
