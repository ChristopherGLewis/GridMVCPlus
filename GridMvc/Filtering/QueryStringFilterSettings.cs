using System;
using System.Linq;
using System.Web;

namespace GridMvc.Filtering
{
	/// <summary>
	///     Object gets filter settings from query string
	/// </summary>
	public class QueryStringFilterSettings : IGridFilterSettings
	{
		public const string DefaultFilterQueryParameter = "grid-filter";
		public const string DefaultFilterQueryParameterSuffix = "{0}-filter";
		private const string FilterDataDelimeter = "__";

		//TODO - Check to see if this need to use GridName
		public const string DefaultFilterInitQueryParameter = "gridinit";
		// ****

		private readonly DefaultFilterColumnCollection _filterValues = new DefaultFilterColumnCollection();
		//public readonly HttpContext Context;
		public HttpContext Context { get; private set; }

		#region Ctor's

		public QueryStringFilterSettings()
			//: this(HttpContext.Current)
		{
			CurrentFilterQueryParameter = DefaultFilterQueryParameter;

			Initialize(HttpContext.Current);
		}

		//pass in our gridname
		public QueryStringFilterSettings(string GridName)
			//: this(HttpContext.Current)
		{
			//Set our base querystring using GridName
			if (!string.IsNullOrEmpty(GridName)) {
				CurrentFilterQueryParameter = string.Format(DefaultFilterQueryParameterSuffix, GridName);
			} else {
				CurrentFilterQueryParameter = DefaultFilterQueryParameter;
			}
			Initialize(HttpContext.Current);
		}

		public QueryStringFilterSettings(HttpContext context)
		{
			Initialize(context);
		}

		private	void Initialize(HttpContext context)
		{
			if (context == null)
				throw new ArgumentException("No http context here!");
			Context = context;

			string[] filters = Context.Request.QueryString.GetValues(this.CurrentFilterQueryParameter);

			if (filters != null) {
				foreach (string filter in filters) {
					//Need to look for ',' here - hsGrid-filter=HostSystemDesc__2__new,HostSystemName__2__1
					foreach (var item in filter.Split(',') ) {
						ColumnFilterValue column = CreateColumnData(item);
						if (column != ColumnFilterValue.Null)
							_filterValues.Add(column);

					}

				}
			}
		}

		#endregion


		public string CurrentFilterQueryParameter { get; private set; }

		private ColumnFilterValue CreateColumnData(string queryParameterValue)
		{
			if (string.IsNullOrEmpty(queryParameterValue))
				return ColumnFilterValue.Null;

			string[] data = queryParameterValue.Split(new[] { FilterDataDelimeter }, StringSplitOptions.RemoveEmptyEntries);
			if (data.Length != 3)
				return ColumnFilterValue.Null;
			GridFilterType type;
			if (!Enum.TryParse(data[1], true, out type))
				type = GridFilterType.Equals;

			return new ColumnFilterValue { ColumnName = data[0], FilterType = type, FilterValue = data[2] };
		}

		#region IGridFilterSettings Members

		public IFilterColumnCollection FilteredColumns
		{
			get { return _filterValues; }
		}

		public bool IsInitState
		{
			get {
				if (FilteredColumns.Any()) return false;
				return Context.Request.QueryString[this.CurrentFilterQueryParameter] != null;
			}
		}

		#endregion
	}
}