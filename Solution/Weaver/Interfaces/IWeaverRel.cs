using Weaver.Items;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverRel : IWeaverItem {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		long Id { get; set; }
		string Label { get; }

		/*--------------------------------------------------------------------------------------------*/
		WeaverRelConn Connection { get; set; }

		/*--------------------------------------------------------------------------------------------*/
		IWeaverRelType RelType { get; }
		bool IsFromManyNodes { get; }
		bool IsToManyNodes { get; }
		bool IsOutgoing { get; }

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