using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Security.Cryptography;
using System.Globalization;


namespace TelaLogin
{
    internal class Acces
    {
        private SqlConnection _conection;

        public Acces()
        {
            _conection = new SqlConnection();
            _conection.ConnectionString = Properties.Settings.Default.Configuracao;
            _conection.Open();
        }
        public Usuario Login(string nome, string senha)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = _conection;
            command.CommandType = System.Data.CommandType.Text;

            //string senhaEncriptada = "";
            //string query = $"select * from Usuario where Nome = '{nome}' and Senha = '{senhaEncriptada}'";
            //command.CommandText = query;

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Load(command.ExecuteReader());
            if (dt.Rows.Count == 0)
            {
                return null;
            }

            var dados = dt.Rows[0].ItemArray;
            Usuario usuarioLogado = new Usuario();
            usuarioLogado.ID = (int)dt.Rows[0]["ID"];
            usuarioLogado.Nome = dt.Rows[0]["Nome"].ToString();
            usuarioLogado.Senha = dt.Rows[0]["Senha"].ToString();
            return usuarioLogado;
        }
    }
}
