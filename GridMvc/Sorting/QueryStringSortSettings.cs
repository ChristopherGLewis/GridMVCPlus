using System;
using System.Web;

namespace GridMvc.Sorting
{
	/// <summary>
	///     Grid sort settings takes from query string
	/// </summary>
	public class QueryStringSortSettings : IGridSortSettings
	{
		public const string DefaultDirectionQueryParameter = "grid-dir";
		public const string DefaultDirectionQueryParameterSuffix = "{0}-dir";
		public const string DefaultColumnQueryParameter = "grid-column";
		public const string DefaultColumnQueryParameterSuffix = "{0}-column";
		public readonly HttpContext Context;
		private string _columnQueryParameterName;
		private string _directionQueryParameterName;


		#region ctors
		public QueryStringSortSettings()
			: this(HttpContext.Current)
		{
		}

		//pass in our gridname
		public QueryStringSortSettings(string GridName)
			: this(HttpContext.Current)
		{
			//Set our base querystring using GridName
			if (!string.IsNullOrEmpty(GridName)) {
				ColumnQueryParameterName = string.Format(DefaultDirectionQueryParameterSuffix, GridName);
				DirectionQueryParameterName = string.Format(DefaultColumnQueryParameterSuffix, GridName);
			}
		}

		public QueryStringSortSettings(HttpContext context)
		{
			if (context == null)
				throw new ArgumentException("No http context here!");
			Context = context;
			ColumnQueryParameterName = DefaultColumnQueryParameter;
			DirectionQueryParameterName = DefaultDirectionQueryParameter;
		}

		#endregion

		public string ColumnQueryParameterName
		{
			get { return _columnQueryParameterName; }
			set
			{
				_columnQueryParameterName = value;
				RefreshColumn();
			}
		}

		public string DirectionQueryParameterName
		{
			get { return _directionQueryParameterName; }
			set
			{
				_directionQueryParameterName = value;
				RefreshDirection();
			}
		}

		#region IGridSortSettings Members

		public string ColumnName { get; set; }
		public GridSortDirection Direction { get; set; }

		#endregion

		private void RefreshColumn()
		{
			//Columns
			string currentSortColumn = Context.Request.QueryString[ColumnQueryParameterName] ?? string.Empty;
			ColumnName = currentSortColumn;
			if (string.IsNullOrEmpty(currentSortColumn))
			{
				Direction = GridSortDirection.Ascending;
			}
		}

		private void RefreshDirection()
		{
			//Direction
			string currentDirection = Context.Request.QueryString[DirectionQueryParameterName] ??
									  string.Empty;
			if (string.IsNullOrEmpty(currentDirection))
			{
				Direction = GridSortDirection.Ascending;
				return;
			}
			GridSortDirection dir;
			Enum.TryParse(currentDirection, true, out dir);
			Direction = dir;
		}
	}
}