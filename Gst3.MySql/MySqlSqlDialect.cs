using System;
using System.Data;

namespace Gst3.MySql
{
	public class MySqlSqlDialect : ISqlDialect
	{
		public static readonly MySqlSqlDialect Instance = new MySqlSqlDialect();

		public string IdentityQuery {
			get { return "last_insert_id()"; }
		}

		public string Autoincrement {
			get { return "auto_increment"; }
		}

		public virtual string InsertIgnore {
			get { return "insert ignore"; }
		}

		public string DbTypeToString(DbType type, int size, int precision)
		{
			switch (type)
			{
				case DbType.Guid:
				return "char(38)";

				case DbType.AnsiString:
				case DbType.String:
				if (size <= 8192) return string.Format("VARCHAR({0})", size);
				else if (8192 < size && size <= ushort.MaxValue) return "TEXT";
				else if (ushort.MaxValue < size && size <= ((int)Math.Pow(2, 24) - 1)) return "MEDIUMTEXT";
				else return "LONGTEXT";

				case DbType.AnsiStringFixedLength:
				case DbType.StringFixedLength:
				return string.Format("CHAR({0})", size);

				case DbType.Xml:
				return "MEDIUMTEXT";

				case DbType.Binary:
				case DbType.Object:
				if(precision > 0)
				{
					if (precision == 1)
						return $"BINARY{size}";

					if (precision == 2)
						return "BLOB";

					else if (precision == 3)
						return "LONGBLOB";

					return "BLOB";
				}
				else
				{
					if (size <= 8192) return string.Format("BINARY({0})", size);
					else if (8192 < size && size <= ushort.MaxValue) return "BLOB";
					else if (ushort.MaxValue < size && size <= ((int)Math.Pow(2, 24) - 1)) return "MEDIUMBLOB";
					else return "LONGBLOB";
				}

				case DbType.Boolean:
				case DbType.Byte:
				return "TINYINY";

				case DbType.SByte:
				return "TINYINY UNSIGNED";

				case DbType.Int16:
				return "SMALLINT";

				case DbType.UInt16:
				return "SMALLINT UNSIGNED";

				case DbType.Int32:
				return "INT";

				case DbType.UInt32:
				return "INT UNSIGNED";

				case DbType.Int64:
				return "BIGINT";

				case DbType.UInt64:
				return "BIGINT UNSIGNED";

				case DbType.Date:
				return "DATE";

				case DbType.DateTime:
				case DbType.DateTime2:
				return "DATETIME";

				case DbType.Time:
				return "TIME";

				case DbType.Decimal:
				return string.Format("DECIMAL({0},{1})", size, precision);

				case DbType.Double:
				return "DOUBLE";

				case DbType.Single:
				return "FLOAT";

				default:
				throw new ArgumentOutOfRangeException(type.ToString());
			}
		}
	}
}
