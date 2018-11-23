using System;
using System.Linq;

namespace Gst3.MySql.Instructions
{
	public abstract class SqlCreate : ISqlInstruction
	{
		protected string Name {
			get;
			private set;
		}


		protected SqlCreate(string name)
		{
			Name = name;
		}


		public abstract string ToString(ISqlDialect dialect);

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
