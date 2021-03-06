﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Query;

namespace Weaver.Core.Util {

	/*================================================================================================*/
	public static class WeaverUtil {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static T GetElementAttribute<T>(Type pType) where T : WeaverElementAttribute {
			object[] atts = pType.GetCustomAttributes(typeof(T), false);
			return (atts.Length == 0 ? null : (T)atts[0]);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IList<WeaverPropPair> GetElementPropertyAttributes(Type pType) {
			PropertyInfo[] props = pType.GetProperties();
			var list = new List<WeaverPropPair>();

			foreach ( PropertyInfo prop in props ) {
				WeaverPropPair wpp = GetPropertyAttribute(prop);

				if ( wpp != null ) {
					list.Add(wpp);
				}
			}

			return list;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string BuildPropList<T>(IWeaverConfig pConfig, IWeaverQuery pQuery, T pElem,
								bool pIncludeId=false) where T : IWeaverElement {
			IList<WeaverPropPair> props = GetElementPropertyAttributes(typeof(T));
			var sb = new StringBuilder();

			foreach ( WeaverPropPair wpp in props ) {
				string dbName = wpp.Attrib.DbName;

				if ( !pIncludeId && dbName == "id" ) {
					continue;
				}

				object val = wpp.Info.GetValue(pElem, null);

				if ( val != null ) {
					sb.Append((sb.Length > 0 ? "," : "")+
						dbName+":"+pQuery.AddParam(new WeaverQueryVal(val)));
				}
			}

			return sb.ToString();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static WeaverPropPair GetPropertyAttribute<T>(Expression<Func<T, object>> pProp)
																			where T : IWeaverElement {
			MemberExpression me = GetMemberExpr(pProp);

			if ( me != null ) {
				PropertyInfo pi = (me.Member as PropertyInfo);
				WeaverPropPair wpp = (pi == null ? null : GetPropertyAttribute(pi));

				if ( wpp == null ) {
					throw new WeaverException("Unknown property: "+me.Member.Name);
				}

				return wpp;
			}

			throw new WeaverException("Item property expression body was of type "+
				pProp.Body.GetType().Name+", but must be of type MemberExpression.");
		}

		/*--------------------------------------------------------------------------------------------*/
		private static WeaverPropPair GetPropertyAttribute(PropertyInfo pProp) {
			object[] atts = pProp.GetCustomAttributes(typeof(WeaverPropertyAttribute), true);
			return (atts.Length == 0 ? null : 
				new WeaverPropPair((WeaverPropertyAttribute)atts[0], pProp));
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static string GetPropertyDbName<T>(Expression<Func<T, object>> pProp)
																			where T : IWeaverElement {
			return GetPropertyAttribute(pProp).Attrib.DbName;
		}

		/*--------------------------------------------------------------------------------------------*/
		private static MemberExpression GetMemberExpr<T>(Expression<Func<T, object>> pProp) {

			var me = (pProp.Body as MemberExpression);
			if ( me != null ) { return me; }

			var ue = (pProp.Body as UnaryExpression);
			return (ue != null ? (ue.Operand as MemberExpression) : null);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string[] GetScriptAndParamJson(string pScript,
														IDictionary<string, IWeaverQueryVal> pParams) {
			string q = JsonUnquote(pScript);

			if ( pParams == null || pParams.Keys.Count == 0 ) {
				return new [] { q };
			}

			const string end = @"(?=$|[^\d])";
			var sb = new StringBuilder();

			foreach ( string key in pParams.Keys ) {
				sb.Append((sb.Length > 0 ? "," : "")+"\""+JsonUnquote(key)+"\":");
				IWeaverQueryVal qv = pParams[key];

				if ( qv.IsString ) {
					sb.Append("\""+JsonUnquote(qv.FixedText)+"\"");
					continue;
				}

				sb.Append(qv.FixedText);

				//Explicitly cast certain parameter types
				//See: https://github.com/tinkerpop/rexster/issues/295

				if ( qv.Original is int ) {
					q = Regex.Replace(q, key+end, key+".toInteger()");
				}
				else if ( qv.Original is byte ) {
					q = Regex.Replace(q, key+end, key+".byteValue()");
				}
				else if ( qv.Original is float ) {
					q = Regex.Replace(q, key+end, key+".toFloat()");
				}
			}

			return new [] { q, "{"+sb+"}" };
		}

		/*--------------------------------------------------------------------------------------------*/
		private static string JsonUnquote(string pText) {
			return pText.Replace("\"", "\\\"");
		}

	}

}