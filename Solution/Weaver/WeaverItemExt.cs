using System;
using System.Linq.Expressions;
using Weaver.Functions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public static class WeaverItemExt {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static T As<T>(this T pCallingItem, out T pAlias) where T : IWeaverItem {
			var func = new WeaverFuncAs<T>(pCallingItem.Path);
			pCallingItem.Path.AddItem(func);
			pAlias = pCallingItem;
			return pCallingItem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static TBack Back<T, TBack>(this T pCallingItem, TBack pAlias) where T : IWeaverItem
																			where TBack : IWeaverItem {
			var func = new WeaverFuncBack<TBack>(pCallingItem.Path, pAlias);
			pCallingItem.Path.AddItem(func);
			return pAlias;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverProp Prop<T>(this T pCallingItem,
									Expression<Func<T, object>> pItemProperty) where T : IWeaverItem {
			var func = new WeaverFuncProp<T>(pItemProperty);
			pCallingItem.Path.AddItem(func);
			return func;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T Has<T>(this T pCallingItem, Expression<Func<T, object>> pItemProperty,
						WeaverFuncHasOp pOperation, object pValue) where T : IWeaverItem {
			var func = new WeaverFuncHas<T>(pItemProperty, pOperation, pValue);
			pCallingItem.Path.AddItem(func);
			return pCallingItem;
		}

		/*--------------------------------------------------------------------------------------------* /
		//TEST: WeaverItemExt.UpdateEach
		public static T UpdateEach<T>(IWeaverPath pPath, WeaverUpdates<T> pUpdates) 
																				where T : IWeaverNode {
			IWeaverQuery q = pPath.Query;
			string update = ".each{";

			for ( int i = 0 ; i < pUpdates.Count ; ++i ) {
				KeyValuePair<string, WeaverQueryVal> pair = pUpdates[i];
				update += (i == 0 ? "" : ";")+"it."+pair.Key+"="+q.AddParamIfString(pair.Value);
			}

			update += "};";
			q.FinalizeQuery(pPath.BuildParameterizedScript()+update);
			return q;
		}*/

	}

}