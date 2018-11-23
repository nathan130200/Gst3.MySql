using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gst3.MySql.Instructions
{
	public class SqlCreateIndex : SqlCreate
	{
		private string table;
		internal string[] columns;
		internal bool unique;
		private bool ifNotExists;


		public SqlCreateIndex(string name, string table, params string[] columns)
			: base(name)
		{
			this.table = table;
			this.columns = columns ?? new string[0];
		}

		public SqlCreateIndex Unique(bool unique)
		{
			this.unique = unique;
			return this;
		}

		public SqlCreateIndex IfNotExists(bool ifNotExists)
		{
			this.ifNotExists = ifNotExists;
			return this;
		}

		public override string ToString(ISqlDialect dialect)
		{
			var sql = new StringBuilder("create ");
			if (unique) sql.Append("unique ");
			sql.Append("index ");
			if (ifNotExists) sql.Append("if not exists ");
			sql.AppendFormat("{0} on {1} (", Name, table);

			foreach (var c in columns)
			{
				sql.AppendFormat("{0}, ", c);
			}

			return sql.Replace(", ", ");", sql.Length - 2, 2).ToString();
		}
	}
}
