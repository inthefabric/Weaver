using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Steps.Parameters;

namespace Weaver.Core.Graph {

	/*================================================================================================*/
	public interface IWeaverAllEdges : IWeaverAllItems {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		T Id<T>(string pId) where T : IWeaverEdge, new();

		/*--------------------------------------------------------------------------------------------*/
		T ExactIndex<T>(Expression<Func<T, object>> pProperty, object pValue) 
																		where T : IWeaverEdge, new();

		/*--------------------------------------------------------------------------------------------*/
		T ElasticIndex<T>(params IWeaverParamElastic<T>[] pParams) where T : IWeaverEdge, new();
		
	}

}