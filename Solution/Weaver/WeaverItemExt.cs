using System;
using System.Linq.Expressions;
using Weaver.Functions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public static class WeaverItemExt {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static T As<T>(this T pCallingItem, out IWeaverFuncAs<T> pAlias)
																	where T : IWeaverItemIndexable {
			var func = new WeaverFuncAs<T>(pCallingItem);
			pCallingItem.Path.AddItem(func);
			pAlias = func;
			return pCallingItem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static TBack Back<T, TBack>(this T pCallingItem, IWeaverFuncAs<TBack> pAlias)
									where T : IWeaverItemIndexable where TBack : IWeaverItemIndexable {
			var func = new WeaverFuncBack<TBack>(pAlias);
			pCallingItem.Path.AddItem(func);
			return pAlias.Item;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverItemWithPath Prop<T>(this T pCallingItem, 
							Expression<Func<T, object>> pItemProperty) where T : IWeaverItemIndexable {
			var func = new WeaverFuncProp<T>(pItemProperty);
			pCallingItem.Path.AddItem(func);
			return func;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T Has<T>(this T pCallingItem, Expression<Func<T, object>> pItemProperty,
							WeaverFuncHasOp pOperation, object pValue) where T : IWeaverItemIndexable {
			var func = new WeaverFuncHas<T>(pItemProperty, pOperation, pValue);
			pCallingItem.Path.AddItem(func);
			return pCallingItem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T UpdateEach<T>(this T pCallingItem, WeaverUpdates<T> pUpdates) 
																		where T : IWeaverItemIndexable {
			var func = new WeaverFuncUpdateEach<T>(pUpdates);
			pCallingItem.Path.AddItem(func);
			return pCallingItem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T RemoveEach<T>(this T pCallingItem) where T : IWeaverItemIndexable {
			var func = new WeaverFuncRemoveEach<T>();
			pCallingItem.Path.AddItem(func);
			return pCallingItem;
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		//TEST: WeaverItemExt.Aggregate()
		/*--------------------------------------------------------------------------------------------*/
		public static T Aggregate<T>(this T pCallingItem, IWeaverVarAlias pVar)
																		where T : IWeaverItemIndexable {
			var func = new WeaverFuncCustom("aggregate("+pVar.Name+")");
			pCallingItem.Path.AddItem(func);
			return pCallingItem;
		}
		
		//TEST: WeaverItemExt.Retain()
		/*--------------------------------------------------------------------------------------------*/
		public static T Retain<T>(this T pCallingItem, IWeaverVarAlias pVar)
																		where T : IWeaverItemIndexable {
			var func = new WeaverFuncCustom("retain("+pVar.Name+")");
			pCallingItem.Path.AddItem(func);
			return pCallingItem;
		}
		
		//TEST: WeaverItemExt.Except()
		/*--------------------------------------------------------------------------------------------*/
		public static T Except<T>(this T pCallingItem, IWeaverVarAlias pVar)
																		where T : IWeaverItemIndexable {
			var func = new WeaverFuncCustom("except("+pVar.Name+")");
			pCallingItem.Path.AddItem(func);
			return pCallingItem;
		}
		
		//TEST: WeaverItemExt.Iterate()
		/*--------------------------------------------------------------------------------------------*/
		public static T Iterate<T>(this T pCallingItem) where T : IWeaverItemIndexable {
			var func = new WeaverFuncCustom("iterate()");
			pCallingItem.Path.AddItem(func);
			return pCallingItem;
		}
		
		//TEST: WeaverItemExt.Dedup()
		/*--------------------------------------------------------------------------------------------*/
		public static T Dedup<T>(this T pCallingItem) where T : IWeaverItemIndexable {
			var func = new WeaverFuncCustom("dedup");
			pCallingItem.Path.AddItem(func);
			return pCallingItem;
		}
		
		//TEST: WeaverItemExt.Limit()
		/*--------------------------------------------------------------------------------------------*/
		public static T Limit<T>(this T pCallingItem, int pStartIndex, int pEndIndex)
																		where T : IWeaverItemIndexable {
			var func = new WeaverFuncCustom("["+pStartIndex+".."+pEndIndex+"]", true);
			pCallingItem.Path.AddItem(func);
			return pCallingItem;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		//TEST: WeaverItemExt.AtIndex()
		public static T AtIndex<T>(this T pCallingItem, int pStartIndex)
																		where T : IWeaverItemIndexable {
			var func = new WeaverFuncCustom("["+pStartIndex+"]", true);
			pCallingItem.Path.AddItem(func);
			return pCallingItem;
		}

		/*--------------------------------------------------------------------------------------------*/
		//TEST: WeaverItemExt.Count()
		public static T Count<T>(this T pCallingItem) where T : IWeaverItemIndexable {
			var func = new WeaverFuncCustom("count()", true);
			pCallingItem.Path.AddItem(func);
			return pCallingItem;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static T CustomStep<T>(this T pCallingItem, string pScript, bool pSkipDotPrefix=false)
																		where T : IWeaverItemIndexable {
			var func = new WeaverFuncCustom(pScript, pSkipDotPrefix);
			pCallingItem.Path.AddItem(func);
			return pCallingItem;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery End<T>(this T pCallingItem) where T : IWeaverItemWithPath {
			IWeaverPath p = pCallingItem.Path;
			p.Query.FinalizeQuery(p.BuildParameterizedScript());
			return p.Query;
		}

	}

}