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


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		//TEST: WeaverItemExt.ToSingle
		public static IWeaverQuery ToSingle<T>(this T pCallingItem) where T : IWeaverItem {
			IWeaverQuery q = pCallingItem.Path.Query;
			q.FinishPathWithQuantity(WeaverQuery.ResultQuantity.Single);
			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		//TEST: WeaverItemExt.ToList
		public static IWeaverQuery ToList<T>(this T pCallingItem) where T : IWeaverItem {
			IWeaverQuery q = pCallingItem.Path.Query;
			q.FinishPathWithQuantity(WeaverQuery.ResultQuantity.List);
			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		//TEST: WeaverItemExt.Update
		public static IWeaverQuery Update<T,TUp>(this T pCallingItem, IWeaverPath pPath,
							WeaverUpdates<TUp> pUpdates) where T : IWeaverItem where TUp : IWeaverNode {
			IWeaverQuery q = pCallingItem.Path.Query;
			q.FinishPathWithUpdate(pUpdates);
			return q;
		}

	}

}