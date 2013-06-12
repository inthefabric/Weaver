using System;
using System.Linq.Expressions;
using System.Reflection;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Query;
using Weaver.Core.Steps;

namespace Weaver.Core.Util {

	/*================================================================================================*/
	public static class WeaverUtil {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string BuildPropList<TItem>(IWeaverConfig pConfig, IWeaverQuery pQuery,
												TItem pItem, bool pIncludeId=false, int pStartParamI=0)
																	where TItem : IWeaverElement {
			string list = "";
			int i = pStartParamI;
			PropertyInfo[] props = pItem.GetType().GetProperties();

			foreach ( PropertyInfo prop in props ) {
				object[] propAtts = prop.GetCustomAttributes(typeof(WeaverItemPropertyAttribute), true);

				if ( propAtts.Length == 0 ) {
					continue;
				}

				if ( !pIncludeId && prop.Name == "Id" ) {
					continue;
				}

				object val = prop.GetValue(pItem, null);

				if ( val == null ) {
					continue;
				}

				list += (i++ > 0 ? "," : "")+
					pConfig.GetPropertyDbName<TItem>(prop.Name)+":"+
					pQuery.AddParam(new WeaverQueryVal(val));
			}

			return list;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string GetPropertyName<T>(IWeaverConfig pConfig, IWeaverStep pStep,
									Expression<Func<T, object>> pExp) where T : IWeaverElement {
			try {
				return GetPropertyName(pConfig, pExp);
			}
			catch ( WeaverException we ) {
				throw new WeaverStepException(pStep, we.Message);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public static string GetPropertyName<T>(IWeaverConfig pConfig, Expression<Func<T, object>> pExp)
																		where T : IWeaverElement {
			MemberExpression me = GetMemberExpr(pExp);

			if ( me != null ) {
				string prop = (me).Member.Name;

				if ( prop == "Id" || prop == "Label" ) {
					return prop.ToLower();
				}

				return pConfig.GetPropertyDbName<T>(prop);
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