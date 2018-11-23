namespace Gst3.MySql.Instructions
{
	/// <summary>
	/// Representa a classe de função exponencial "Entre (X, Y)"
	/// </summary>
	public class BetweenExp : Exp
	{
		private readonly string column;
		private readonly object maxValue;
		private readonly object minValue;


		/// <summary>
		/// Construtor
		/// </summary>
		/// <param name="column">Nome da coluna</param>
		/// <param name="minValue">Valor mínimo</param>
		/// <param name="maxValue">Valor máximo</param>
		public BetweenExp(string column, object minValue, object maxValue)
		{
			this.column = column;
			this.minValue = minValue;
			this.maxValue = maxValue;
		}

		public override string ToString(ISqlDialect dialect)
		{
			return string.Format("{0} {1}between ? and ?", column, Not ? "not " : string.Empty);
		}

		public override object[] GetParameters()
		{
			return new[] { minValue, maxValue };
		}
	}
}
