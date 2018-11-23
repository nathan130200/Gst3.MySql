using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gst3.MySql.Instructions
{
	/// <summary>
	/// Representa a função "Existe"
	/// </summary>
	public class ExistsExp : Exp
	{
		private readonly SqlQuery query;
		
		public ExistsExp(SqlQuery query)
		{
			this.query = query;
		}

		public override string ToString(ISqlDialect dialect)
		{
			return string.Format("{0}exists({1})", Not ? "not " : string.Empty, query.ToString(dialect));
		}

		public override object[] GetParameters()
		{
			if (query != null) return query.GetParameters();
			return new object[0];
		}
	}
}
