namespace Gst3.MySql.Instructions
{
	/// <summary>
	/// Representa a condição "Igual X(A) == Y(B), sendo A e B colunas, X e Y, valores" 
	/// </summary>
	public class EqColumnsExp : Exp
	{
		private readonly ISqlInstruction column1;
		private readonly ISqlInstruction column2;
		
		/// <summary>
		/// Construtor
		/// </summary>
		/// <param name="column1">Primeira coluna</param>
		/// <param name="column2">Segunda coluna</param>
		public EqColumnsExp(string column1, string column2)
		{
			this.column1 = (SqlIdentifier)column1;
			this.column2 = (SqlIdentifier)column2;
		}

		/// <summary>
		/// Construtor
		/// </summary>
		/// <param name="column1">Coluna que será verificada.</param>
		/// <param name="query">Outra consulta.</param>
		public EqColumnsExp(string column1, SqlQuery query)
		{
			this.column1 = (SqlIdentifier)column1;
			column2 = new SubExp(query);
		}

		public override string ToString(ISqlDialect dialect)
		{
			return string.Format("{0} {1} {2}",
				column1.ToString(dialect),
				Not ? "<>" : "=",
				column2.ToString(dialect));
		}

		public override object[] GetParameters()
		{
			return column2.GetParameters();
		}
	}
}
