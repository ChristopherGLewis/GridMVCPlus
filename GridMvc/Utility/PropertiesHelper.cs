﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace GridMvc.Utility
{
	/// <summary>
	///     Helper class for reflection operations
	/// </summary>
	internal static class PropertiesHelper
	{
		private const string PropertiesQueryStringDelimeter = ".";

		public static string BuildColumnNameFromMemberExpression(MemberExpression memberExpr)
		{
			var sb = new StringBuilder();
			Expression expr = memberExpr;
			while (true) {
				string piece = GetExpressionMemberName(expr, ref expr);
				if (string.IsNullOrEmpty(piece)) break;
				if (sb.Length > 0)
					sb.Insert(0, PropertiesQueryStringDelimeter);
				sb.Insert(0, piece);
			}
			return sb.ToString();
		}

		private static string GetExpressionMemberName(Expression expr, ref Expression nextExpr)
		{
			if (expr is MemberExpression) {
				var memberExpr = (MemberExpression)expr;
				nextExpr = memberExpr.Expression;
				return memberExpr.Member.Name;
			}
			if (expr is BinaryExpression && expr.NodeType == ExpressionType.ArrayIndex) {
				var binaryExpr = (BinaryExpression)expr;
				string memberName = GetExpressionMemberName(binaryExpr.Left, ref nextExpr);
				if (string.IsNullOrEmpty(memberName))
					throw new InvalidDataException("Cannot parse your column expression");
				return string.Format("{0}[{1}]", memberName, binaryExpr.Right);
			}
			return string.Empty;
		}


		public static PropertyInfo GetPropertyFromColumnName(string columnName, Type type,
															 out IEnumerable<PropertyInfo> propertyInfoSequence)
		{
			string[] properties = columnName.Split(new[] { PropertiesQueryStringDelimeter },
												  StringSplitOptions.RemoveEmptyEntries);
			if (!properties.Any()) {
				propertyInfoSequence = null;
				return null;
			}
			PropertyInfo pi = null;
			var sequence = new List<PropertyInfo>();
			foreach (string propertyName in properties) {
				pi = type.GetProperty(propertyName);
				if (pi == null) {
					propertyInfoSequence = null;
					return null; //no match column
				}

				//check for metadata
				MetadataTypeAttribute[] metas = (MetadataTypeAttribute[])type.GetCustomAttributes(typeof(MetadataTypeAttribute), true);
				if (metas.Count() > 0) {
					foreach (var meta in metas) {
						PropertyInfo metapi = meta.MetadataClassType.GetProperty(propertyName);
						if (metapi == null) {
							//property has no 
							propertyInfoSequence = null;
							return null; //no match column
						}
						sequence.Add(metapi);
						type = metapi.PropertyType;
						pi = metapi;
					}

				} else {
					sequence.Add(pi);
					type = pi.PropertyType;
				}

			}
			propertyInfoSequence = sequence;
			return pi;
		}

		public static Type GetUnderlyingType(Type type)
		{
			Type targetType;
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
				targetType = Nullable.GetUnderlyingType(type);
			} else {
				targetType = type;
			}
			return targetType;
		}

		public static T GetAttribute<T>(this PropertyInfo pi)
		{
			return (T)pi.GetCustomAttributes(typeof(T), true).FirstOrDefault();
		}

		public static T GetAttribute<T>(this Type type)
		{
			return (T)type.GetCustomAttributes(typeof(T), true).FirstOrDefault();
		}
	}
}