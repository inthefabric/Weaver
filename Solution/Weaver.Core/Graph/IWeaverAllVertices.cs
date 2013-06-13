using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Steps.Parameters;

namespace Weaver.Core.Graph {

	/*================================================================================================*/
	public interface IWeaverAllVertices : IWeaverAllItems {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		T Id<T>(string pId) where T : IWeaverVertex, new();

		/*--------------------------------------------------------------------------------------------*/
		T ExactIndex<T>(Expression<Func<T, object>> pProperty, object pValue)
																		where T : IWeaverVertex, new();

		/*--------------------------------------------------------------------------------------------*/
		T ElasticIndex<T>(params IWeaverParamElastic<T>[] pParams) where T : IWeaverVertex, new();

	}

}