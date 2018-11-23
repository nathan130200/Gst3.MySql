using System.Data;
using System.Text;

namespace Gst3.MySql.Instructions
{
	public class SqlCreateColumn : SqlCreate
	{
		private DbType type;
		private int size;
		private int precision;
		private bool notNull;
		private bool primaryKey;
		private bool autoinc;
		private object defaultValue;


		public SqlCreateColumn(string name, DbType type)
			: this(name, type, 0, 0)
		{
			this.type = type;
		}

		public SqlCreateColumn(string name, DbType type, int size)
			: this(name, type, size, 0)
		{
			this.type = type;
		}

		public SqlCreateColumn(string name, DbType type, int size, int precision)
			: base(name)
		{
			this.type = type;
			this.size = size;
			this.precision = precision;
		}


		public SqlCreateColumn NotNull(bool notNull)
		{
			this.notNull = notNull;
			return this;
		}

		public SqlCreateColumn Autoincrement(bool autoincrement)
		{
			this.autoinc = autoincrement;
			return this;
		}

		public SqlCreateColumn PrimaryKey(bool primaryKey)
		{
			this.primaryKey = primaryKey;
			return this;
		}

		public SqlCreateColumn Default(object value)
		{
			this.defaultValue = value;
			return this;
		}

		public override string ToString(ISqlDialect dialect)
		{
			var sql = new StringBuilder().AppendFormat("{0} {1} {2} ", Name, dialect.DbTypeToString(type, size, precision), notNull ? "not null" : "null");
			if (defaultValue != null) sql.AppendFormat("default {0} ", defaultValue);
			if (primaryKey) sql.Append("primary key ");
			if (autoinc) sql.Append(dialect.Autoincrement);

			return sql.ToString().Trim();
		}
	}
}
