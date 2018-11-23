namespace Gst3.MySql.Instructions
{
	public enum AgregateType
	{
		count,
		min,
		max,
		avg,
		sum
	}

	public class SelectAgregate : ISqlInstruction
	{
		private readonly AgregateType agregateType;
		private readonly string column;

		public SelectAgregate(AgregateType agregateType)
			: this(agregateType, null)
		{
		}

		public SelectAgregate(AgregateType agregateType, string column)
		{
			this.agregateType = agregateType;
			this.column = column;
		}

		public string ToString(ISqlDialect dialect)
		{
			return string.Format("{0}({1})", agregateType, column == null ? "*" : column);
		}

		public object[] GetParameters()
		{
			return new object[0];
		}

		public override string ToString()
		{
			return ToString(MySqlSqlDialect.Instance);
		}
	}
}
