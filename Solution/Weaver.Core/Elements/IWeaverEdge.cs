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
		Type FromVertexType { get; }
		Type ToVertexType { get; }

	}


	/*================================================================================================*/
	public interface IWeaverEdge<T> : IWeaverEdge, IWeaverElement<T> where T : class, IWeaverEdge {

	}
	

	/*================================================================================================*/
	public interface IWeaverEdge<TEdge, out TFrom, out TTo> : IWeaverEdge<TEdge>
																		where TEdge : class, IWeaverEdge
																		where TFrom : IWeaverVertex
																		where TTo : IWeaverVertex {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		TFrom FromVertex { get; }
		TTo ToVertex { get; }

	}

}