using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gst3.MySql.Instructions
{
	public class LgtExp : Exp
	{
		private readonly SqlIdentifier column;
		private readonly bool equal;
		private readonly object value;

		public LgtExp(string column, object value, bool equal)
		{
			this.column = (SqlIdentifier)column;
			this.value = value;
			this.equal = equal;
		}

		public override string ToString(ISqlDialect dialect)
		{
			return Not ? string.Format("{0} >{1} ?", column.ToString(dialect), !equal ? "=" : string.Empty) :
				string.Format("{0} <{1} ?", column.ToString(dialect), equal ? "=" : string.Empty);
		}

		public override object[] GetParameters()
		{
			return new[] { value };
		}
	}
}
