using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Steps;

namespace Weaver.Core.Graph {

	/*================================================================================================*/
	public abstract class WeaverAllItems : WeaverPathItem, IWeaverAllItems {

		public bool ForSpecificId { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public T IdInner<T>(string pId) where T : IWeaverElement, new() {
			ForSpecificId = true;
			var idParam = Path.Query.AddParam(new WeaverQueryVal(pId));

			var sc = new WeaverStepCustom("("+idParam+")", true);
			Path.AddItem(sc);
			return new T { Path = Path };
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected T ExactIndexInner<T>(Expression<Func<T, object>> pProperty, object pValue)
																	where T : IWeaverElement, new() {
			var ei = new WeaverStepExactIndex<T>(pProperty, pValue);
			Path.AddItem(ei);
			return new T { Path = Path };
		}

	}

}