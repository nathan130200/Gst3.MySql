using System.Data;

namespace Gst3.MySql
{
	/// <summary>
	/// Representa a interface para dialeto MYSQL.
	/// </summary>
	public interface ISqlDialect
	{
		/// <summary>
		/// Identificador de Consulta
		/// </summary>
		string IdentityQuery { get; }
		string Autoincrement { get; }
		string InsertIgnore { get; }

		/// <summary>
		/// Converter o tipo global de banco de dados, pelo tipo específico de um banco de dados.
		/// </summary>
		/// <param name="type">Tipo que será convertido.</param>
		/// <param name="size">Tamanho no banco de dados.</param>
		/// <param name="precision">Precisão no banco de dados.</param>
		/// <returns></returns>
		string DbTypeToString(DbType type, int size, int precision);
	}
}
