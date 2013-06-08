using System;
using System.Linq.Expressions;
using Weaver.Core.Func;
using Weaver.Core.Items;

namespace Weaver.Core.Graph {

	/*================================================================================================*/
	public abstract class WeaverAllItems : WeaverItem, IWeaverAllItems {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected WeaverAllItems() {

		}

		/*--------------------------------------------------------------------------------------------*/
		protected IWeaverItem FromIndex<T>(Expression<Func<T, object>> pProperty, 
									object pExactValue) where T : class, IWeaverItemIndexable, new() {
			var ei = new WeaverFuncExactIndex<T>(pProperty, pExactValue);
			Path.AddItem(ei);
			return ei; //TODO return next item ?
		}

	}

}