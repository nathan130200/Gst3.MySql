using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gst3.MySql
{
	/// <summary>
	/// Delegate que será disparado sempre que o MYSQL retornar algum erro.
	/// </summary>
	/// <param name="error"></param>
	public delegate void Gst3MysqlOnErrorEventHandler(MySqlError error);

	/// <summary>
	/// Representa a classe que gerencia instruções e comandos MYSQL.
	/// </summary>
	public class Gst3MySqlWrapper : IDisposable
	{
		private MySqlConnection _connection;
		private bool _reuse;
		private volatile bool _disposed;

		/// <summary>
		/// Evento que será disparado sempre que o MYSQL retornar algum erro.
		/// </summary>
		public event Gst3MysqlOnErrorEventHandler OnError;

		/// <summary>
		/// Construtor
		/// </summary>
		/// <param name="connection_string">Cadeia de conexão que será criado a nova coenxão mysql.</param>
		public Gst3MySqlWrapper(string connection_string)
		{
			this._connection = new MySqlConnection(connection_string);
		}

		/// <summary>
		/// Construtor
		/// </summary>
		/// <param name="connection">Conexão MYSQL que será reutilizada.</param>
		public Gst3MySqlWrapper(MySqlConnection connection)
		{
			this._connection = connection;
			this._connection.InfoMessage += OnMessage;
			this._reuse = true;
		}

		/// <summary>
		/// Abre a conexão mysql.
		/// </summary>
		public void Open()
		{
			if (this._reuse)
				return;

			this._connection.Open();
			this._connection.InfoMessage += OnMessage;
		}

		/// <summary>
		/// Fecha a coenxão MYSQL e não deleta a instância.
		/// </summary>
		public void Close()
		{
			this._connection.Close();
		}

		/// <summary>
		/// Fecha a conexão e deleta todos os recursos usados pela classe.
		/// </summary>
		public void Dispose()
		{
			if (!_disposed)
			{
				this._connection.InfoMessage -= OnMessage;
				this._connection.Dispose();
				this._connection = null;

				_disposed = true;
			}
		}

		/// <summary>
		/// Cria um novo comando a partir da conexão atual.
		/// </summary>
		/// <returns></returns>
		public MySqlCommand CreateCommand()
		{
			return this._connection.CreateCommand();
		}

		/// <summary>
		/// Cria um novo comando a partir da conexão atual e a instrução atual.
		/// </summary>
		/// <param name="sql">Instrução que será convertida no comando.</param>
		/// <returns></returns>
		public MySqlCommand CreateCommand(ISqlInstruction sql)
		{
			var command = GetConnection().CreateCommand();
			var parameters = sql.GetParameters().ToArray();
			var parts = sql.ToString(MySqlSqlDialect.Instance).Split('?');
			var result = new StringBuilder();
			for (var i = 0; i < parts.Length - 1; i++)
			{
				var p = command.CreateParameter();
				p.ParameterName = "p" + i;
				if (parameters[i] == null)
				{
					p.Value = DBNull.Value;
				}
				else if (parameters[i] is Enum)
				{
					p.Value = ((Enum)parameters[i]).ToString("d");
				}
				else
				{
					p.Value = parameters[i];
				}
				command.Parameters.Add(p);
				result.AppendFormat("{0}@{1}", parts[i], p.ParameterName);
			}
			result.Append(parts[parts.Length - 1]);
			command.CommandText = result.ToString();
			return command;
		}

		/// <summary>
		/// Cria uma nova transação.
		/// </summary>
		/// <returns></returns>
		public MySqlTransaction CreateTransaction()
		{
			return this._connection.BeginTransaction();
		}

		/// <summary>
		/// Retorna a instância da conexão mysql atual.
		/// </summary>
		/// <returns></returns>
		public MySqlConnection GetConnection()
		{
			return this._connection;
		}

		/// <summary>
		/// Troca o banco de dados do mysql atual.
		/// </summary>
		/// <param name="database_name"></param>
		public void ChangeDatabase(string database_name)
		{
			this._connection.ChangeDatabase(database_name);
		}

		/// <summary>
		/// Envia uma solcitiação ping ao mysql.
		/// </summary>
		/// <returns></returns>
		public bool Ping()
		{
			return this._connection.Ping();
		}

		/// <summary>
		/// Executa uma instrução mysql e retorna as linhas e/ou as colunas resultantes.
		/// </summary>
		/// <param name="sql">Instrução mysql que será executada.</param>
		/// <returns></returns>
		public IEnumerable<object[]> ExecuteAll(ISqlInstruction sql)
		{
			using (var command = CreateCommand(sql))
			using (var reader = command.ExecuteReader())
			{
				var result = new List<object[]>();
				var fieldCount = reader.FieldCount;
				while (reader.Read())
				{
					var row = new object[fieldCount];
					for (var i = 0; i < fieldCount; i++)
					{
						row[i] = reader[i];
						if (DBNull.Value.Equals(row[i])) row[i] = null;
					}
					result.Add(row);
				}
				return result;
			}
		}

		/// <summary>
		/// Executa uma instrução mysql e retorna apenas a primera coluna.
		/// </summary>
		/// <typeparam name="T">Tipo de valor que será retornado</typeparam>
		/// <param name="sql">Instrução mysql que será executada.</param>
		/// <returns></returns>
		public T ExecuteScalar<T>(ISqlInstruction sql)
		{
			using (var command = CreateCommand(sql))
			{
				var result = command.ExecuteScalar();
				return result == null || result == DBNull.Value ? default(T) : (T)Convert.ChangeType(result, typeof(T));
			}
		}

		/// <summary>
		/// Executa uma instrução mysql sem resultado e retorna os itens afetados.
		/// </summary>
		/// <param name="sql">Instrução mysql que será executado.</param>
		/// <returns></returns>
		public int ExecuteNonQuery(ISqlInstruction sql)
		{
			using (var command = CreateCommand(sql))
			{
				return command.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Executa instruções mysql em lotes e retorna a soma dos itens afetados.
		/// </summary>
		/// <param name="batch">Lotes de instrução mysql que serão executadas</param>
		/// <returns></returns>
		public int ExececuteBatch(IEnumerable<ISqlInstruction> batch)
		{
			if (batch == null)
				throw new ArgumentNullException(nameof(batch), "Lote de instruções não pode ser nulo.");

			var affected = 0;
			using (var tx = CreateTransaction())
			{
				foreach (var sql in batch)
				{
					affected += ExecuteNonQuery(sql);
				}
				tx.Commit();
			}
			return affected;
		}

		void OnMessage(object sender, MySqlInfoMessageEventArgs e)
		{
			if (e.errors == null)
				return;

			foreach (var err in e.errors)
				OnError?.Invoke(err);
		}
	}
}
