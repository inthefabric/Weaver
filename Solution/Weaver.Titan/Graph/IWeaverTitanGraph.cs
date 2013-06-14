using Weaver.Core.Graph;
using Weaver.Core.Pipe;
using Weaver.Core.Query;
using Weaver.Core.Schema;
using Weaver.Titan.Schema;

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
		IWeaverQuery TypeGroupOf(WeaverVertexSchema pVertex, int pId);

		/*--------------------------------------------------------------------------------------------*/
		IWeaverPathPipeEnd MakePropertyKey(WeaverVertexSchema pVertex, WeaverTitanPropSchema pProperty, 
																		IWeaverVarAlias pGroupVar=null);

		/*--------------------------------------------------------------------------------------------*/
		IWeaverPathPipeEnd MakePropertyKey(WeaverEdgeSchema pEdge, WeaverTitanPropSchema pProperty,
																		IWeaverVarAlias pGroupVar=null);

		/*--------------------------------------------------------------------------------------------*/
		IWeaverPathPipeEnd BuildEdgeLabel(WeaverEdgeSchema pEdge);

	}

}