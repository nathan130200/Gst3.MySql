using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gst3.MySql.Instructions
{
	public class SqlDelete : ISqlInstruction
	{
		private readonly string table;
		private Exp where = Exp.Empty;

		public SqlDelete(string table)
		{
			this.table = table;
		}

		public string ToString(ISqlDialect dialect)
		{
			var sql = new StringBuilder();
			sql.AppendFormat("delete from {0}", table);
			if (where != Exp.Empty) sql.AppendFormat(" where {0}", where.ToString(dialect));
			return sql.ToString();
		}

		public object[] GetParameters()
		{
			return where != Exp.Empty ? where.GetParameters() : new object[0];
		}

		public SqlDelete Where(Exp where)
		{
			this.where = this.where & where;
			return this;
		}

		public SqlDelete Where(string column, object value)
		{
			return Where(Exp.Eq(column, value));
		}

		public override string ToString()
		{
			return ToString(MySqlSqlDialect.Instance);
		}
	}
}
