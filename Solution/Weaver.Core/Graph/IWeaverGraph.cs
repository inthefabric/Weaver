using Weaver.Core.Items;
using Weaver.Core.Query;

namespace Weaver.Core.Graph {
	
	/*================================================================================================*/
	public interface IWeaverGraph {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverAllVertices V { get; }

		/*--------------------------------------------------------------------------------------------*/
		IWeaverAllEdges E { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery AddVertex<T>(T pVertex) where T : IWeaverVertex;

		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery AddEdge<TOut, TEdge, TIn>(TOut pOutVertex, TEdge pEdgeType, TIn pInVertex)
						where TOut : IWeaverVertex where TEdge : IWeaverEdge where TIn : IWeaverVertex;

		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery AddEdge<TEdge>(IWeaverVarAlias pOutVertex, TEdge pEdgeType,
												IWeaverVarAlias pInVertex) where TEdge : IWeaverEdge;

	}

}