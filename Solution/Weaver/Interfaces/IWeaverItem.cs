using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Functions;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverItem {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		WeaverQuery Query { get; set; }

		/*--------------------------------------------------------------------------------------------*/
		IWeaverItem PrevQueryItem { get; }
		IWeaverItem NextQueryItem { get; }
		IList<IWeaverItem> QueryPathToThisItem { get; }
		IList<IWeaverItem> QueryPathFromThisItem { get; }

		/*--------------------------------------------------------------------------------------------*/
		string GremlinCode { get; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		TItem As<TItem>(string pLabel) where TItem : IWeaverItem;
		TToItem Back<TToItem>(string pLabel) where TToItem : IWeaverItem;
		IWeaverProp Prop<TItem>(Expression<Func<TItem, object>> pItemProperty)
																		where TItem : IWeaverItem;
		TItem Has<TItem>(Expression<Func<TItem, object>> pItemProperty,
						WeaverFuncHasOp pOperation, object pValue) where TItem : IWeaverItem;

	}

}