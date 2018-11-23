using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Gst3.MySql.Instructions
{
	public class SqlCreateTable : SqlCreate
	{
		private bool ifNotExists;
		private List<string> primaryKey = new List<string>();
		private List<SqlCreateColumn> columns = new List<SqlCreateColumn>();
		private List<SqlCreateIndex> indexes = new List<SqlCreateIndex>();


		public SqlCreateTable(string name)
			: base(name)
		{

		}

		public SqlCreateTable(string name, bool ifNotExists)
			: this(name)
		{
			IfNotExists(ifNotExists);
		}

		public SqlCreateTable IfNotExists(bool ifNotExists)
		{
			this.ifNotExists = ifNotExists;
			return this;
		}

		public SqlCreateTable AddColumn(SqlCreateColumn column)
		{
			this.columns.Add(column);
			return this;
		}

		public SqlCreateTable AddColumn(string name, DbType type)
		{
			AddColumn(name, type, 0, false);
			return this;
		}

		public SqlCreateTable AddColumn(string name, DbType type, int size)
		{
			AddColumn(name, type, size, false);
			return this;
		}

		public SqlCreateTable AddColumn(string name, DbType type, bool notNull)
		{
			AddColumn(name, type, 0, notNull);
			return this;
		}

		public SqlCreateTable AddColumn(string name, DbType type, int size, bool notNull)
		{
			AddColumn(new SqlCreateColumn(name, type, size).NotNull(notNull));
			return this;
		}

		public SqlCreateTable PrimaryKey(params string[] columns)
		{
			primaryKey.AddRange(columns);
			return this;
		}

		public SqlCreateTable AddIndex(SqlCreateIndex index)
		{
			index.IfNotExists(ifNotExists);
			this.indexes.Add(index);
			return this;
		}

		public SqlCreateTable AddIndex(string name, params string[] columns)
		{
			AddIndex(new SqlCreateIndex(name, Name, columns));
			return this;
		}

		public override string ToString(ISqlDialect dialect)
		{
			var sql = new StringBuilder("create table ");
			if (ifNotExists) sql.Append("if not exists ");
			sql.AppendFormat("{0} (", Name);

			foreach (var c in columns)
			{
				sql.AppendFormat("{0}, ", c.ToString(dialect));
			}
			if (0 < primaryKey.Count)
			{
				sql.Append("primary key (");
				foreach (var c in primaryKey)
				{
					sql.AppendFormat("{0}, ", c);
				}
				sql.Replace(", ", "), ", sql.Length - 2, 2);
			}
			sql.Replace(", ", ");", sql.Length - 2, 2);
			sql.AppendLine();
			indexes.ForEach(i => sql.AppendLine(i.ToString(dialect)));

			return sql.ToString();
		}
	}
}
