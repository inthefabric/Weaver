using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Graph;
using Weaver.Core.Pipe;
using Weaver.Core.Query;

namespace Weaver.Titan.Graph {
	
	/*================================================================================================*/
	public interface IWeaverTitanGraph : IWeaverGraph {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverTitanGraphQuery QueryV();

		/*--------------------------------------------------------------------------------------------*/
		IWeaverTitanGraphQuery QueryE();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery AddEdgeVci<TOut, TEdge, TIn>(TOut pOutVertex, TEdge pEdge, TIn pInVertex)
						where TOut : IWeaverVertex where TEdge : IWeaverEdge where TIn : IWeaverVertex;

		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery AddEdgeVci<TEdge>(IWeaverVarAlias pOutVertexVar, TEdge pEdge,
												IWeaverVarAlias pInVertexVar) where TEdge : IWeaverEdge;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery TypeGroupOf<T>(int pId) where T : IWeaverVertex;

		
		/*--------------------------------------------------------------------------------------------*/
		IWeaverPathPipeEnd MakeVertexPropertyKey<T>(Expression<Func<T, object>> pProperty,
												IWeaverVarAlias pGroupVar=null) where T : IWeaverVertex;

		/*--------------------------------------------------------------------------------------------*/
		IWeaverPathPipeEnd MakeEdgePropertyKey<T>(Expression<Func<T, object>> pProperty,
												IWeaverVarAlias pGroupVar=null) where T : IWeaverEdge;

		/*--------------------------------------------------------------------------------------------*/
		IWeaverPathPipeEnd BuildEdgeLabel<T>(Func<string, IWeaverVarAlias> pGetPropVarAliasByDbName) 
																				where T : IWeaverEdge;

	}

}