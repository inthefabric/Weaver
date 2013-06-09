using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Path;
using Weaver.Core.Steps;

namespace Weaver.Core.Graph {

	/*================================================================================================*/
	public abstract class WeaverAllItems : WeaverPathItem, IWeaverAllItems {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected T ExactIndexInner<T>(Expression<Func<T, object>> pProperty, object pValue)
																	where T : IWeaverElement, new() {
			var ei = new WeaverStepExactIndex<T>(pProperty, pValue);
			Path.AddItem(ei);
			return new T { Path = Path };
		}

	}

}