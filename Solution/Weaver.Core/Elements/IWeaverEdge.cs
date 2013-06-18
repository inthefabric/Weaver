using System;

namespace Weaver.Core.Elements {
	
	/*================================================================================================*/
	public interface IWeaverEdge : IWeaverElement {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		string Label { get; }

		/*--------------------------------------------------------------------------------------------*/
		WeaverEdgeConn Connection { get; set; }

		/*--------------------------------------------------------------------------------------------*/
		IWeaverEdgeType EdgeType { get; }
		bool IsFromManyVertices { get; }
		bool IsToManyVertices { get; }
		bool IsOutgoing { get; }

		/*--------------------------------------------------------------------------------------------*/
		Type OutVertexType { get; }
		Type InVertexType { get; }

		/*--------------------------------------------------------------------------------------------*/
		bool IsValidOutVertexType(Type pType);
		bool IsValidInVertexType(Type pType);

	}


	/*================================================================================================*/
	public interface IWeaverEdge<out TOut, out TIn> : IWeaverEdge
												where TOut : IWeaverVertex where TIn : IWeaverVertex {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		TOut OutVertex { get; }
		TIn InVertex { get; }

	}

}