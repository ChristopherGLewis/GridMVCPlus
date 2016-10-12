using System;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GridMvc.Filtering.Types
{
    /// <summary>
    ///     Object contains some logic for filtering DateTime columns
    /// </summary>
    internal sealed class DateTimeFilterType : FilterTypeBase
    {
        public override Type TargetType
        {
            get { return typeof(DateTime); }
        }

        public override Expression GetFilterExpression(Expression leftExpr, string value, GridFilterType filterType)
        {
            //Equal on date, not date and time
            if (filterType == GridFilterType.Equals) {
                GetValidType(filterType);
                //Parse value into DateTime - note this is date only
                object typedValue = GetTypedValue(value);
                if (typedValue == null)
                    return null; //incorrect filter value;

                //Equal is an issue, since the linq value in leftExpr may be a date with time.
                //convert Equal to use leftExpr >= value and leftExpr < (value + 1day)
                Expression valueExpr = Expression.Constant(typedValue);
                var newLeftExp = Expression.GreaterThanOrEqual(leftExpr, valueExpr);
                DateTime nextDay = DateTime.Parse(value).AddDays(1);
                var dayAfterExp = Expression.Constant(nextDay);
                var newRightExp = Expression.LessThan(leftExpr, dayAfterExp);
                return Expression.And(newLeftExp, newRightExp);
            }

            //var dateExpr = Expression.Property(leftExpr, leftExpr.Type, "Date");

            //if (filterType == GridFilterType.Equals)
            //{
            //    var dateObj = GetTypedValue(value);
            //    if (dateObj == null) return null;//not valid

            //    var startDate = Expression.Constant(dateObj);
            //    var endDate = Expression.Constant(((DateTime)dateObj).AddDays(1));

            //    var left = Expression.GreaterThanOrEqual(leftExpr, startDate);
            //    var right = Expression.LessThan(leftExpr, endDate);

            //    return Expression.And(left, right);
            //}

            return base.GetFilterExpression(leftExpr, value, filterType);
        }

        /// <summary>
        ///     There are filter types that allowed for DateTime column
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override GridFilterType GetValidType(GridFilterType type)
        {
            switch (type)
            {
                case GridFilterType.Equals:
                case GridFilterType.GreaterThan:
                case GridFilterType.GreaterThanOrEquals:
                case GridFilterType.LessThan:
                case GridFilterType.LessThanOrEquals:
                    return type;
                default:
                    return GridFilterType.Equals;
            }
        }

        public override object GetTypedValue(string value)
        {
            DateTime date;
            if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                return null;
            return date;
        }
    }
}