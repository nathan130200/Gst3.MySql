using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gst3.MySql.Instructions
{
	/// <summary>
	/// Representa a condição "X(A) == Y(A), sendo A a coluna, X e Y valores, que serão comparados."
	/// </summary>
	public class EqExp : Exp
	{
		private readonly string column;
		private readonly object value;

		/// <summary>
		/// Construtor
		/// </summary>
		/// <param name="column">Coluna que será verificada.</param>
		/// <param name="value">Valor que será comparado com o que está no banco.</param>
		public EqExp(string column, object value)
		{
			this.column = column;
			this.value = value;
		}

		public override string ToString(ISqlDialect dialect)
		{
			return string.Format("{0} {1}", column, value != null ? 
				(Not ? "<> ?" : "= ?") : (Not ? "is not null" : "is null"));
		}

		public override object[] GetParameters()
		{
			return value == null ? new object[0] : new[] { value };
		}
	}
}
