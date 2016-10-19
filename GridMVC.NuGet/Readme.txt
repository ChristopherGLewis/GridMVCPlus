Project Description
Grid.Mvc.Plus is a fork of http://gridmvc.codeplex.com/ which appears to be abandoned.  It has
some additions that were added in https://gridmvcredux.codeplex.com/ and fixes some issues that 
were in the GridMVC Issues and Discussions.
GridMVC was updated to updated to .Net 4.5 and MVC 5.

From http://gridmvc.codeplex.com/ 
Grid.Mvc adds functionality for creating GridView controls in your ASP.NET MVC 5 web application.
It is a component that allows you easy construction of HTML tables for displaying, paging, filtering and sorting 
data from a collection of your Model objects.

Assumptions
While this NuGet package is called GridMVC.Plus, all namespaces retain the GridMVC name to facilitate updates.

Release Notes:
- Grid.Mvc 3.1.0
    Fixed Date filter per https://gridmvcredux.codeplex.com/
    Added changing the filter glyph to X when filter is on
    Added removing the sort from a column
	Added data annotations from here: http://gridmvc.codeplex.com/discussions/449498
	Fixed an issue with using RouteValueDictionary to pass query strings to a page with a grid.
	  For example, a URL that is http://localhost/Site//UI/MyPage?grid-filter=Col1__2__new&grid-filter=Col2__2__1
	  would look like http://localhost/Site//UI/MyPage?grid-filter=Col1__2__new,Col2__2__1 when passed through a 
	  RouteValueDictionary call
	Added an Html Helper to add the current page's query strings to the RouteValueDictionary to pass grid 
	  filter & pager settings through CRUD subpages



