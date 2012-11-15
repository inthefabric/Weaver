using Fabric.Domain.Graph.Items;

namespace Fabric.Domain.Graph.Interfaces {

	/*================================================================================================*/
	public interface IWeaverRel : IWeaverItem {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		WeaverRelConn Connection { get; }

		/*--------------------------------------------------------------------------------------------*/
		IWeaverRelType RelType { get; }
		bool FromManyNodes { get; }
		bool ToManyNodes { get; }
		bool Outgoing { get; }

		/*--------------------------------------------------------------------------------------------*/
		string Label { get; }

	}
	

	/*================================================================================================*/
	public interface IWeaverRel<out TQueryFrom, out TQueryTo> : IWeaverRel
																	where TQueryFrom : IWeaverQueryNode
																	where TQueryTo : IWeaverQueryNode {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		TQueryFrom FromNode { get; }
		TQueryTo ToNode { get; }

	}

}