using System;
using System.Linq.Expressions;
using Weaver.Functions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public static class WeaverItemExt {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static T As<T>(this T pCallingItem, out T pAlias) where T : IWeaverIndexableItem {
			var func = new WeaverFuncAs<T>(pCallingItem.Path);
			pCallingItem.Path.AddItem(func);
			pAlias = pCallingItem;
			return pCallingItem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static TBack Back<T, TBack>(this T pCallingItem, TBack pAlias)
									where T : IWeaverIndexableItem where TBack : IWeaverIndexableItem {
			var func = new WeaverFuncBack<TBack>(pCallingItem.Path, pAlias);
			pCallingItem.Path.AddItem(func);
			return pAlias;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverProp Prop<T>(this T pCallingItem, 
							Expression<Func<T, object>> pItemProperty) where T : IWeaverIndexableItem {
			var func = new WeaverFuncProp<T>(pItemProperty);
			pCallingItem.Path.AddItem(func);
			return func;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T Has<T>(this T pCallingItem, Expression<Func<T, object>> pItemProperty,
							WeaverFuncHasOp pOperation, object pValue) where T : IWeaverIndexableItem {
			var func = new WeaverFuncHas<T>(pItemProperty, pOperation, pValue);
			pCallingItem.Path.AddItem(func);
			return pCallingItem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T UpdateEach<T>(this T pCallingItem, WeaverUpdates<T> pUpdates) 
																		where T : IWeaverIndexableItem {
			var func = new WeaverFuncUpdateEach<T>(pUpdates);
			pCallingItem.Path.AddItem(func);
			return pCallingItem;
		}

	}

}