using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Steps.Parameters;

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

		/*--------------------------------------------------------------------------------------------*/
		public T ElasticIndex<T>(params IWeaverParamElastic<T>[] pParams) where T : IWeaverEdge, new() {
			return ElasticIndexInner(pParams);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return (ForSpecificId ? "e" : "E");
		}

	}

}