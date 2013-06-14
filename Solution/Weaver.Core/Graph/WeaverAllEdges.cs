using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;

namespace Weaver.Core.Graph {

	/*================================================================================================*/
	public class WeaverAllEdges : WeaverAllItems, IWeaverAllEdges {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public T Id<T>(string pId) where T : IWeaverEdge, new() {
			return IdInner<T>(pId);
		}

		/*--------------------------------------------------------------------------------------------*/
		public T ExactIndex<T>(Expression<Func<T, object>> pProperty, object pValue)
																		where T : IWeaverEdge, new() {
			return ExactIndexInner(pProperty, pValue);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return (ForSpecificId ? "e" : "E");
		}

	}

}