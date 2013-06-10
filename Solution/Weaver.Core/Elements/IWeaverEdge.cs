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
		bool IsFromManyNodes { get; }
		bool IsToManyNodes { get; }
		bool IsOutgoing { get; }

		/*--------------------------------------------------------------------------------------------*/
		Type FromNodeType { get; }
		Type ToNodeType { get; }

	}


	/*================================================================================================*/
	public interface IWeaverEdge<T> : IWeaverEdge, IWeaverElement<T> where T : IWeaverEdge {

	}
	

	/*================================================================================================*/
	public interface IWeaverEdge<TEdge, out TFrom, out TTo> : IWeaverEdge<TEdge>
					where TEdge : IWeaverEdge where TFrom : IWeaverVertex where TTo : IWeaverVertex {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		TFrom FromNode { get; }
		TTo ToNode { get; }

	}

}