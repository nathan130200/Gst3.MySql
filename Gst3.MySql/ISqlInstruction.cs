using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gst3.MySql
{
	/// <summary>
	/// Representa a interface de instrução MYSQL.
	/// </summary>
	public interface ISqlInstruction
	{
		/// <summary>
		/// Converte a instrução para a linguagem de consulta SQL.
		/// </summary>
		/// <param name="dialect"></param>
		/// <returns></returns>
		string ToString(ISqlDialect dialect);

		/// <summary>
		/// Solicita a lista de parâmetros da instrução.
		/// </summary>
		/// <returns></returns>
		object[] GetParameters();
	}
}
