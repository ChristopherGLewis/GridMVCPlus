/***
* This script demonstrates how you can build you own custom filter widgets:
* 1. Specify widget type for column:
*       columns.Add(o => o.Customers.CompanyName)
*           .SetFilterWidgetType("CustomCompanyNameFilterWidget")
* 2. Register script with custom widget on the page:
*       <script src="@Url.Content("~/Scripts/gridmvc.customwidgets.js")" type="text/javascript"> </script>
* 3. Register your own widget in Grid.Mvc:
*       GridMvc.addFilterWidget(new CustomersFilterWidget());
*
* For more documentation see: http://gridmvc.codeplex.com/documentation
*/

/***
* CustomersFilterWidget - Provides filter user interface for customer name column in this project
* This widget onRenders select list with available customers.
*/

function CustomersFilterWidget() {
    //This is the list on the page to fill - note must be unique
    var filterWidgetType = "CustomCompanyNameFilterWidget";
    var listDesc = "Select a customer to filter:";
    var listName = "customerslist";
    var listNameCssTag = "." + listName;
    var url = "/Home/GetCustomersNames";
    var urlMethod = "POST"; //$.ajax - method to use for the request (e.g. "POST", "GET", "PUT").

    /***
    * This method must return type of registered widget type in 'SetFilterWidgetType' method
    */
    this.getAssociatedTypes = function () {
        return [filterWidgetType];
    };
    /***
    * This method invokes when filter widget was shown on the page
    */
    this.onShow = function () {
        /* Place your on show logic here */
    };

    this.showClearFilterButton = function () {
        return true;
    };

    /***
    * This method will invoke when user was clicked on filter button.
    * container - html element, which must contain widget layout;
    * lang - current language settings;
    * typeName - current column type (if widget assign to multiple types, see: getAssociatedTypes);
    * values - current filter values. Array of objects [{filterValue: '', filterType:'1'}];
    * cb - callback function that must invoked when user want to filter this column. Widget must pass filter type and filter value.
    * data - widget data passed from the server
    */
    this.onRender = function (container, lang, typeName, values, cb, data) {
        //store parameters:
        this.cb = cb;
        this.container = container;
        this.lang = lang;

        //this filterwidget demo supports only 1 filter value for column column
        this.value = values.length > 0 ? values[0] : { filterType: 1, filterValue: "" };

        this.renderWidget(listDesc, listName); //onRender filter widget
        this.loadData(listNameCssTag, url, urlMethod); //load customer's list from the server
        this.registerEvents(); //handle events
    };

    /***
     * Method that renders the html for the widget.  Note that 
     */
    this.renderWidget = function (desc, listClassName) {
        var html = '<p><i>This is custom filter widget demo.</i></p>\
                    <p>' + desc + '</p>\
                    <select style="width:250px;" class="grid-filter-type ' + listClassName + ' form-control">\
                    </select>';
        this.container.append(html);
    };

    /***
    * Method loads all customers from the server via Ajax.  Note that we don't know the format in which the data is 
    * returned, so fillList(data) needs to understand that
    */
    this.loadData = function (listNameCssTag, url, urlMethod) {
        var $this = this;
        $.ajax({
            url:url,
            method: urlMethod,
            data: function (data) {
                $this.fillList(listNameCssTag, data);
            }
        } );
    };

    /***
    * Method fill customers select list by data
    */
    this.fillList = function (listNameCssTag, data) {
        var customerList = this.container.find(listNameCssTag);
        for (var i = 0; i < data.items.length; i++) {
            customerList.append('<option ' + (items[i] == this.value.filterValue ? 'selected="selected"' : '') + ' value="' + items[i] + '">' + items[i] + '</option>');
        }
    };

    /***
    * Internal method that register event handlers for 'apply' button.
    */
    this.registerEvents = function () {
        //get list with customers
        var list = this.container.find(listNameCssTag);
        //save current context:
        var $context = this;
        //register onclick event handler
        list.change(function () {
            //invoke callback with selected filter values:
            var values = [{ filterValue: $(this).val(), filterType: 1 /* Equals */ }];
            $context.cb(values);
        });
    };

}
