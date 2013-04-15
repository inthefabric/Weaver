using System;
using System.Linq.Expressions;
using System.Reflection;
using Weaver.Exceptions;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver {

	/*================================================================================================*/
	public static class WeaverUtil {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string BuildPropList<TItem>(IWeaverQuery pQuery, TItem pItem,
									bool pIncludeId=false, int pStartParamI=0, bool pIncludeNulls=false)
									where TItem : IWeaverItem {
			string list = "";
			int i = pStartParamI;

			foreach ( PropertyInfo prop in pItem.GetType().GetProperties() ) {
				object[] propAtts = prop.GetCustomAttributes(typeof(WeaverItemPropertyAttribute), true);

				if ( propAtts.Length == 0 ) {
					continue;
				}

				if ( !pIncludeId && prop.Name == "Id" ) {
					continue;
				}

				object val = prop.GetValue(pItem, null);

				if ( val == null && !pIncludeNulls ) {
					continue;
				}

				list += (i++ > 0 ? "," : "")+prop.Name+":"+pQuery.AddParam(new WeaverQueryVal(val));
			}

			return list;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string GetPropertyName<T>(IWeaverConfig pConfig, IWeaverFunc pFunc,
									Expression<Func<T, object>> pExp) where T : IWeaverItemIndexable {
			try {
				return GetPropertyName(pConfig, pExp);
			}
			catch ( WeaverException we ) {
				throw new WeaverFuncException(pFunc, we.Message);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public static string GetPropertyName<T>(IWeaverConfig pConfig, Expression<Func<T, object>> pExp)
																		where T : IWeaverItemIndexable {
			MemberExpression me = GetMemberExpr(pExp);

			if ( me != null ) {
				string prop = (me).Member.Name;

				if ( prop == "Id" || prop == "Label" ) { //TEST: lowercase logic
					return prop.ToLower();
				}

				return pConfig.GetPropertyName<T>(prop);
			}

			throw new WeaverException("Item property expression body was of type "+
				pExp.Body.GetType().Name+", but must be of type MemberExpression.");
		}

		/*--------------------------------------------------------------------------------------------*/
		private static MemberExpression GetMemberExpr<T>(Expression<Func<T, object>> pExp) {

			var me = (pExp.Body as MemberExpression);
			if ( me != null ) { return me; }

			var ue = (pExp.Body as UnaryExpression);
			return (ue != null ? (ue.Operand as MemberExpression) : null);
		}

	}

}