using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using GridMvc.Columns;
using GridMvc.Filtering;
using GridMvc.Utility;
using System.Web.Routing;

namespace GridMvc.Html
{
	public static class GridExtensions
	{
		internal const string DefaultPartialViewName = "_Grid";

		#region Grid

		public static HtmlGrid<T> Grid<T>(this HtmlHelper helper, IEnumerable<T> items)
			where T : class
		{
			return Grid(helper, items, DefaultPartialViewName);
		}

		public static HtmlGrid<T> Grid<T>(this HtmlHelper helper, string gridName, IEnumerable<T> items)
			where T : class
		{
			return Grid(helper, items, GridRenderOptions.Create(gridName, DefaultPartialViewName));
		}

		public static HtmlGrid<T> Grid<T>(this HtmlHelper helper, IEnumerable<T> items, string viewName)
			where T : class
		{
			return Grid(helper, items, GridRenderOptions.Create(string.Empty, viewName));
		}



		public static HtmlGrid<T> Grid<T>(this HtmlHelper helper, IEnumerable<T> items, GridRenderOptions renderOptions)
			where T : class
		{
			var newGrid = new Grid<T>(items.AsQueryable(), renderOptions);
			////newGrid.RenderOptions = renderOptions;

#if false

			////deal with ?grid-filter=C1__2__text,C2__2__text2 vs ?hsGrid-filter=C1__2__text&grid-filter=C2__2__text2
			////Parse our QueryString looking for filters that have been combined into single QS parameter's
			//System.Collections.Specialized.NameValueCollection qs = helper.ViewContext.HttpContext.Request.QueryString;
			//if (qs != null) {
			//	QueryStringFilterSettings qsfs = newGrid.Settings.FilterSettings as QueryStringFilterSettings;
			//	var cfqp = qsfs.CurrentFilterQueryParameter;

			//	CustomQueryStringBuilder builder = new CustomQueryStringBuilder(qs);
			//	string qsWOFilter = builder.GetQueryStringExcept(new[] { cfqp });

			//	//See if we have our filter
			//	System.Collections.Specialized.NameValueCollection newQSnvc = new System.Collections.Specialized.NameValueCollection();

			//	if (qs.GetValues(cfqp) != null) {
			//		if (qs.GetValues(cfqp).Count() > 0) {
			//			foreach (string item in qs.GetValues(cfqp)) {
			//				if (item.Contains(",")) {
			//					//split by , and add each
			//					foreach (var item2 in item.Split(',')) {
			//						newQSnvc.Add(cfqp, item2);
			//					}
			//				} else {
			//					newQSnvc.Add(cfqp, item);
			//				}
			//			}
			//		}
			//	}

			//	//use our CQSB to get the right ToString()
			//	CustomQueryStringBuilder builder2 = new CustomQueryStringBuilder(newQSnvc);
			//	string newQS = string.Empty;
			//	if (builder2.ToString().Length > 0) {
			//		if (qsWOFilter.Length > 0) {
			//			newQS = string.Format("{0}&{1}", builder2.ToString().Substring(1), qsWOFilter.Substring(1));
			//		} else {
			//			newQS = builder2.ToString().Substring(1);
			//		}
			//	} else {
			//		if (qsWOFilter.Length > 0) {
			//			newQS = qsWOFilter;
			//		}
			//	}
			//	helper.ViewContext.HttpContext.RewritePath(helper.ViewContext.HttpContext.Request.Path, "", newQS);
			//}
#endif

			var htmlGrid = new HtmlGrid<T>(newGrid, helper.ViewContext, renderOptions.ViewName);
			return htmlGrid;
		}

		public static HtmlGrid<T> Grid<T>(this HtmlHelper helper, Grid<T> sourceGrid)
			where T : class
		{
			//wrap source grid:
			var htmlGrid = new HtmlGrid<T>(sourceGrid, helper.ViewContext, DefaultPartialViewName);
			return htmlGrid;
		}

		public static HtmlGrid<T> Grid<T>(this HtmlHelper helper, Grid<T> sourceGrid, string viewName)
			where T : class
		{
			//wrap source grid:
			var htmlGrid = new HtmlGrid<T>(sourceGrid, helper.ViewContext, viewName);
			return htmlGrid;
		}
		#endregion

		//support IHtmlString in RenderValueAs method
		public static IGridColumn<T> RenderValueAs<T>(this IGridColumn<T> column, Func<T, IHtmlString> constraint)
		{
			Func<T, string> valueContraint = a => constraint(a).ToHtmlString();
			return column.RenderValueAs(valueContraint);
		}

		//support WebPages inline helpers
		public static IGridColumn<T> RenderValueAs<T>(this IGridColumn<T> column, Func<T, Func<object, HelperResult>> constraint)
		{
			Func<T, string> valueContraint = a => constraint(a)(null).ToHtmlString();
			return column.RenderValueAs(valueContraint);
		}


		#region GridViewQueryStringRouteWithID
		//Helper to add the current page's query strings to the RouteValueDictionary to pass grid filter & pager
		//settings through CRUD subpages
		
		public static RouteValueDictionary GridViewQueryStringRouteWithID(this HtmlHelper html)
		{
			return GridViewQueryStringRouteWithID(html, null);
		}

		public static RouteValueDictionary GridViewQueryStringRouteWithID(this HtmlHelper html, int? ID)
		{
			string queryString = (new CustomQueryStringBuilder( html.ViewContext.RequestContext.HttpContext.Request.QueryString)) .ToString();
			return GridViewQueryStringRouteWithID(html, ID, queryString);
		}

		public static RouteValueDictionary GridViewQueryStringRouteWithID(this HtmlHelper html, int? ID, string queryString)
		{
			RouteValueDictionary rvd = new RouteValueDictionary();
			if (ID.HasValue) {
				rvd.Add("id", ID.Value);
			}

			//Decode our QueryString
			string qs = HttpUtility.UrlDecode(queryString);
			if (qs.StartsWith("?")) {
				qs = qs.Substring(1);
			}

			if (!string.IsNullOrEmpty(qs)) {
				string[] qaArray = qs.Split('&');
				foreach (var item in qaArray) {
					if (item.Contains('=')) {
						string[] parmArray = item.Split('=');
						if (parmArray.Count() > 1) {
							//add or update?
							if (rvd.Keys.Contains(parmArray[0].ToString())) {
								rvd[parmArray[0].ToString()] += "," + parmArray[1].ToString();
							} else {
								rvd.Add(parmArray[0].ToString(), parmArray[1]);
							}
						}
					}
				}
			}

			return rvd;
		}

		#endregion

	}
}