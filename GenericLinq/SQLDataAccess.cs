using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GenericLinq
{
    public class SQLDataAcces : ISQLDataAcces, IDisposable
    {

        [Serializable]
        public class Parametro
        {
            private string _Nombre;
            private SqlDbType _DbType;
            private int _Size;
            private object _Value;
            private ParameterDirection _Direction;
            public string Nombre
            {
                get
                {
                    return this._Nombre;
                }
                set
                {
                    this._Nombre = value;
                }
            }
            public int DbType
            {
                get
                {
                    return (int)this._DbType;
                }
                set
                {
                    this._DbType = (SqlDbType)value;
                }
            }
            public int Size
            {
                get
                {
                    return this._Size;
                }
                set
                {
                    this._Size = value;
                }
            }
            public object Value
            {
                get
                {
                    return this._Value;
                }
                set
                {
                    this._Value = RuntimeHelpers.GetObjectValue(value);
                }
            }
            public ParameterDirection Direction
            {
                get
                {
                    return this._Direction;
                }
                set
                {
                    this._Direction = value;
                }
            }
            public Parametro()
            {
                this._Nombre = "";
                this._DbType = SqlDbType.VarChar;
                this._Size = 0;
                this._Value = RuntimeHelpers.GetObjectValue(new object());
                this._Direction = ParameterDirection.Input;
            }
            public Parametro(string _Nombre, SqlDbType _DbType, int _Size, object _Value, ParameterDirection _Direction)
            {
                this._Nombre = _Nombre;
                this._DbType = _DbType;
                this._Size = _Size;
                this._Value = RuntimeHelpers.GetObjectValue(_Value);
                this._Direction = _Direction;
            }
            public Parametro(string _Nombre, object _Value)
            {
                this._Nombre = _Nombre;
                this._DbType = SqlDbType.VarChar;
                this._Size = 0;
                this._Value = RuntimeHelpers.GetObjectValue(_Value);
            }
            public Parametro(string _Nombre, SqlDbType _DbType, ParameterDirection _Direction, object _Value)
            {
                this._Nombre = _Nombre;
                this._DbType = _DbType;
                this.Direction = _Direction;
                this.Value = RuntimeHelpers.GetObjectValue(_Value);
                this.Size = 1;
            }
        }
        private string _CadenaConexion;
        private SqlDataAdapter _DA;
        private DataSet _DS;
        private DataView _DV;
        public List<Parametro> ListaParametros;
        private SqlConnection sqlConnection;
        private SqlTransaction transaction;


        public string CadenaConexion
        {
            get
            {
                return this._CadenaConexion;
            }
            set
            {
                this._CadenaConexion = value;
            }
        }
        public Int32 tiempoEspera
        {
            get;
            set;
        }


        public SQLDataAcces()
        {
            this._CadenaConexion = "";
            this._DA = new SqlDataAdapter();
            this._DS = new DataSet();
            this._DV = new DataView();
            this.ListaParametros = new List<Parametro>();
        }

        public SQLDataAcces(IDbConnection conexion)
        {
            this.sqlConnection = conexion as SqlConnection;
            this._DA = new SqlDataAdapter();
            this._DS = new DataSet();
            this._DV = new DataView();
            this.ListaParametros = new List<Parametro>();

        }
        public SQLDataAcces(DataContext conexion)
        {
            this.sqlConnection = conexion.Connection as SqlConnection;
            this._DA = new SqlDataAdapter();
            this._DS = new DataSet();
            this._DV = new DataView();
            this.ListaParametros = new List<Parametro>();
            this.transaction = conexion.Transaction as SqlTransaction;
        }


        public DataSet GetDataSet(CommandType _tipoComando, string _cadenaComando)
        {
            checked
            {
                DataSet result;
                try
                {

                    if (sqlConnection == null)
                        sqlConnection = new SqlConnection(this._CadenaConexion);
                    try
                    {

                        if (sqlConnection.State == ConnectionState.Closed)
                            sqlConnection.Open();
                        SqlCommand sqlCommand = sqlConnection.CreateCommand();
                        try
                        {
                            SqlCommand sqlCommand2 = sqlCommand;
                            sqlCommand2.CommandType = _tipoComando;
                            sqlCommand2.Connection = sqlConnection;
                            sqlCommand2.CommandText = _cadenaComando;

                            if (transaction != null)
                                sqlCommand2.Transaction = this.transaction;


                            if (tiempoEspera != null)
                                sqlCommand2.CommandTimeout = tiempoEspera;


                            bool flag = this.ListaParametros.Count != 0;
                            bool flag2;
                            if (flag)
                            {
                                int arg_6A_0 = 0;
                                int num = this.ListaParametros.Count - 1;
                                int num2 = arg_6A_0;
                                while (true)
                                {
                                    int arg_14D_0 = num2;
                                    int num3 = num;
                                    if (arg_14D_0 > num3)
                                    {
                                        break;
                                    }
                                    SqlParameterCollection parameters = sqlCommand.Parameters;
                                    flag2 = (this.ListaParametros[num2].Size == 0);
                                    if (flag2)
                                    {
                                        parameters.AddWithValue(this.ListaParametros[num2].Nombre, RuntimeHelpers.GetObjectValue(this.ListaParametros[num2].Value));
                                    }
                                    else
                                    {
                                        parameters.Add(this.ListaParametros[num2].Nombre, (SqlDbType)this.ListaParametros[num2].DbType, this.ListaParametros[num2].Size).Value = RuntimeHelpers.GetObjectValue(this.ListaParametros[num2].Value);
                                        sqlCommand.Parameters[num2].Direction = this.ListaParametros[num2].Direction;
                                    }
                                    num2++;
                                }
                            }
                            this._DA = new SqlDataAdapter(sqlCommand);
                            this._DS = new DataSet();
                            this._DA.Fill(this._DS);
                            flag2 = (this.ListaParametros.Count != 0);
                            if (flag2)
                            {
                                int arg_1A3_0 = 0;
                                int num4 = this.ListaParametros.Count - 1;
                                int num5 = arg_1A3_0;
                                while (true)
                                {
                                    int arg_1E0_0 = num5;
                                    int num3 = num4;
                                    if (arg_1E0_0 > num3)
                                    {
                                        break;
                                    }
                                    this.ListaParametros[num5].Value = RuntimeHelpers.GetObjectValue(sqlCommand.Parameters[num5].Value);
                                    num5++;
                                }
                            }
                            flag2 = this._DS != null ? true : false;
                            if (flag2)
                            {
                                result = this._DS;
                            }
                            else
                            {
                                result = null;
                            }
                        }
                        finally
                        {
                            bool flag2 = sqlCommand != null;
                            if (flag2)
                            {
                                //((IDisposable)sqlCommand).Dispose();
                            }
                        }
                        sqlConnection.Close();
                    }
                    finally
                    {
                        bool flag2 = sqlConnection != null;
                        if (flag2)
                        {
                            // ((IDisposable)sqlConnection).Dispose();
                        }
                    }
                }
                catch (Exception expr_23F)
                {
                    throw expr_23F;
                }
                return result;
            }
        }
        public DataView GetDataView(CommandType _tipoComando, string _cadenaComando)
        {
            checked
            {
                DataView result;
                try
                {
                    if (sqlConnection == null)
                        sqlConnection = new SqlConnection(this._CadenaConexion);
                    try
                    {

                        if (sqlConnection.State == ConnectionState.Closed)
                            sqlConnection.Open();
                        SqlCommand sqlCommand = sqlConnection.CreateCommand();
                        try
                        {
                            SqlCommand sqlCommand2 = sqlCommand;
                            sqlCommand2.CommandType = _tipoComando;
                            sqlCommand2.Connection = sqlConnection;
                            sqlCommand2.CommandText = _cadenaComando;

                            if (transaction != null)
                                sqlCommand2.Transaction = this.transaction;



                            if (tiempoEspera != null)
                                sqlCommand2.CommandTimeout = tiempoEspera;

                            bool flag = this.ListaParametros.Count != 0;
                            bool flag2;
                            if (flag)
                            {
                                int arg_6A_0 = 0;
                                int num = this.ListaParametros.Count - 1;
                                int num2 = arg_6A_0;
                                while (true)
                                {
                                    int arg_12A_0 = num2;
                                    int num3 = num;
                                    if (arg_12A_0 > num3)
                                    {
                                        break;
                                    }
                                    SqlParameterCollection parameters = sqlCommand.Parameters;
                                    flag2 = (this.ListaParametros[num2].Size == 0);
                                    if (flag2)
                                    {
                                        parameters.AddWithValue(this.ListaParametros[num2].Nombre, RuntimeHelpers.GetObjectValue(this.ListaParametros[num2].Value));
                                    }
                                    else
                                    {
                                        parameters.Add(this.ListaParametros[num2].Nombre, (SqlDbType)this.ListaParametros[num2].DbType, this.ListaParametros[num2].Size).Value = RuntimeHelpers.GetObjectValue(this.ListaParametros[num2].Value);
                                    }
                                    num2++;
                                }
                            }
                            this._DA = new SqlDataAdapter(sqlCommand);
                            this._DS = new DataSet();
                            this._DA.Fill(this._DS);
                            flag2 = this._DS != null ? true : false;
                            if (flag2)
                            {
                                this._DV.Table = this._DS.Tables[0];
                                result = this._DV;
                            }
                            else
                            {
                                result = null;
                            }
                        }
                        finally
                        {
                            bool flag2 = sqlCommand != null;
                            if (flag2)
                            {
                                ((IDisposable)sqlCommand).Dispose();
                            }
                        }
                        sqlConnection.Close();
                    }
                    finally
                    {
                        bool flag2 = sqlConnection != null;
                        if (flag2)
                        {
                            // ((IDisposable)sqlConnection).Dispose();
                        }
                    }
                }
                catch (Exception expr_1D2)
                {
                    throw expr_1D2;
                }
                return result;
            }
        }
        public object GetRow(CommandType _tipoComando, string _cadenaComando)
        {
            checked
            {
                object result = null;
                try
                {
                    if (sqlConnection == null)
                        sqlConnection = new SqlConnection(this._CadenaConexion);
                    try
                    {

                        if (sqlConnection.State == ConnectionState.Closed)
                            sqlConnection.Open();
                        SqlCommand sqlCommand = sqlConnection.CreateCommand();
                        try
                        {
                            SqlCommand sqlCommand2 = sqlCommand;
                            sqlCommand2.CommandType = _tipoComando;
                            sqlCommand2.Connection = sqlConnection;
                            sqlCommand2.CommandText = _cadenaComando;


                            if (transaction != null)
                                sqlCommand2.Transaction = this.transaction;



                            if (tiempoEspera != null)
                                sqlCommand2.CommandTimeout = tiempoEspera;


                            bool flag = this.ListaParametros.Count != 0;
                            bool flag2;
                            if (flag)
                            {
                                int arg_6A_0 = 0;
                                int num = this.ListaParametros.Count - 1;
                                int num2 = arg_6A_0;
                                while (true)
                                {
                                    int arg_12A_0 = num2;
                                    int num3 = num;
                                    if (arg_12A_0 > num3)
                                    {
                                        break;
                                    }
                                    SqlParameterCollection parameters = sqlCommand.Parameters;
                                    flag2 = (this.ListaParametros[num2].Size == 0);
                                    if (flag2)
                                    {
                                        parameters.AddWithValue(this.ListaParametros[num2].Nombre, RuntimeHelpers.GetObjectValue(this.ListaParametros[num2].Value));
                                    }
                                    else
                                    {
                                        parameters.Add(this.ListaParametros[num2].Nombre, (SqlDbType)this.ListaParametros[num2].DbType, this.ListaParametros[num2].Size).Value = RuntimeHelpers.GetObjectValue(this.ListaParametros[num2].Value);
                                    }
                                    num2++;
                                }
                            }
                            this._DA = new SqlDataAdapter(sqlCommand);
                            this._DS = new DataSet();
                            this._DA.Fill(this._DS);
                            flag2 = this._DS != null ? true : false;
                            if (flag2)
                            {
                                if (this._DS.Tables[0].Rows.Count > 0)
                                    result = this._DS.Tables[0].Rows[0];
                            }
                            else
                            {
                                result = null;
                            }
                        }
                        finally
                        {
                            bool flag2 = sqlCommand != null;
                            if (flag2)
                            {
                                ((IDisposable)sqlCommand).Dispose();
                            }
                        }
                        sqlConnection.Close();
                    }
                    finally
                    {
                        bool flag2 = sqlConnection != null;
                        if (flag2)
                        {
                            // ((IDisposable)sqlConnection).Dispose();
                        }
                    }
                }
                catch (Exception expr_1ED)
                {
                    throw expr_1ED;
                }
                return result;
            }
        }
        public void ExecuteNonQuery(CommandType _tipoComando, string _cadenaComando)
        {
            checked
            {
                try
                {
                    if (sqlConnection == null)
                        sqlConnection = new SqlConnection(this._CadenaConexion);
                    try
                    {

                        if (sqlConnection.State == ConnectionState.Closed)
                            sqlConnection.Open();
                        SqlCommand sqlCommand = sqlConnection.CreateCommand();
                        try
                        {
                            SqlCommand sqlCommand2 = sqlCommand;
                            sqlCommand2.CommandType = _tipoComando;
                            sqlCommand2.Connection = sqlConnection;
                            sqlCommand2.CommandText = _cadenaComando;



                            if (transaction != null)
                                sqlCommand2.Transaction = this.transaction;




                            if (tiempoEspera != null)
                                sqlCommand2.CommandTimeout = tiempoEspera;


                            bool flag = this.ListaParametros.Count != 0;
                            if (flag)
                            {
                                int arg_6A_0 = 0;
                                int num = this.ListaParametros.Count - 1;
                                int num2 = arg_6A_0;
                                while (true)
                                {
                                    int arg_12A_0 = num2;
                                    int num3 = num;
                                    if (arg_12A_0 > num3)
                                    {
                                        break;
                                    }
                                    SqlParameterCollection parameters = sqlCommand.Parameters;
                                    bool flag2 = this.ListaParametros[num2].Size == 0;
                                    if (flag2)
                                    {
                                        parameters.AddWithValue(this.ListaParametros[num2].Nombre, RuntimeHelpers.GetObjectValue(this.ListaParametros[num2].Value));
                                    }
                                    else
                                    {
                                        parameters.Add(this.ListaParametros[num2].Nombre, (SqlDbType)this.ListaParametros[num2].DbType, this.ListaParametros[num2].Size).Value = RuntimeHelpers.GetObjectValue(this.ListaParametros[num2].Value);
                                    }
                                    num2++;
                                }
                            }
                            sqlCommand.ExecuteNonQuery();
                        }
                        finally
                        {
                            bool flag2 = sqlCommand != null;
                            if (flag2)
                            {
                                ((IDisposable)sqlCommand).Dispose();
                            }
                        }
                        sqlConnection.Close();
                    }
                    finally
                    {
                        bool flag2 = sqlConnection != null;
                        if (flag2)
                        {
                            // ((IDisposable)sqlConnection).Dispose();
                        }
                    }
                }
                catch (Exception expr_172)
                {
                    throw expr_172;
                }
            }
        }
        public object GetOutput(CommandType _tipoComando, string _cadenaComando)
        {
            checked
            {
                object result;
                try
                {
                    if (sqlConnection == null)
                        sqlConnection = new SqlConnection(this._CadenaConexion);
                    try
                    {

                        if (sqlConnection.State == ConnectionState.Closed)
                            sqlConnection.Open();
                        SqlCommand sqlCommand = sqlConnection.CreateCommand();
                        try
                        {
                            SqlCommand sqlCommand2 = sqlCommand;
                            sqlCommand2.CommandType = _tipoComando;
                            sqlCommand2.Connection = sqlConnection;
                            sqlCommand2.CommandText = _cadenaComando;


                            if (transaction != null)
                                sqlCommand2.Transaction = this.transaction;


                            if (tiempoEspera != null)
                                sqlCommand2.CommandTimeout = tiempoEspera;


                            bool flag = this.ListaParametros.Count != 0;
                            int index = 0;
                            if (flag)
                            {
                                int arg_6A_0 = 0;
                                int num = this.ListaParametros.Count - 1;
                                int num2 = arg_6A_0;
                                while (true)
                                {
                                    int arg_168_0 = num2;
                                    int num3 = num;
                                    if (arg_168_0 > num3)
                                    {
                                        break;
                                    }
                                    SqlParameterCollection parameters = sqlCommand.Parameters;
                                    bool flag2 = this.ListaParametros[num2].Size == 0;
                                    if (flag2)
                                    {
                                        parameters.AddWithValue(this.ListaParametros[num2].Nombre, RuntimeHelpers.GetObjectValue(this.ListaParametros[num2].Value));
                                    }
                                    else
                                    {
                                        parameters.Add(this.ListaParametros[num2].Nombre, (SqlDbType)this.ListaParametros[num2].DbType, this.ListaParametros[num2].Size).Value = RuntimeHelpers.GetObjectValue(this.ListaParametros[num2].Value);
                                    }
                                    flag2 = (this.ListaParametros[num2].Direction == ParameterDirection.Output);
                                    if (flag2)
                                    {
                                        sqlCommand.Parameters[num2].Direction = ParameterDirection.Output;
                                        index = num2;
                                    }
                                    num2++;
                                }
                            }
                            sqlCommand.ExecuteNonQuery();
                            result = RuntimeHelpers.GetObjectValue(sqlCommand.Parameters[index].Value);
                        }
                        finally
                        {
                            bool flag2 = sqlCommand != null;
                            if (flag2)
                            {
                                ((IDisposable)sqlCommand).Dispose();
                            }
                        }
                        if (this.transaction == null)
                            sqlConnection.Close();
                    }
                    finally
                    {
                        bool flag2 = sqlConnection != null;
                        if (flag2)
                        {
                            // ((IDisposable)sqlConnection).Dispose();
                        }
                    }
                }
                catch (Exception expr_1C7)
                {
                    throw expr_1C7;

                }
                return result;
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}
