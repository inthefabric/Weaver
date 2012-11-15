using Fabric.Domain.Graph.Items;

namespace Fabric.Test.Common.Nodes {

	/*================================================================================================*/
	public abstract class TestNode : WeaverNode {

		private readonly bool vIsRoot;
		private readonly bool vIsFromNode;
		private readonly bool vExpectOneNode;

		public string Name { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected TestNode() {}

		/*--------------------------------------------------------------------------------------------*/
		protected TestNode(bool pIsRoot, bool pIsFromNode, bool pExpectOneNode) {
			vIsRoot = pIsRoot;
			vIsFromNode = pIsFromNode;
			vExpectOneNode = pExpectOneNode;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override bool IsRoot { get { return vIsRoot; } }
		public override bool IsFromNode { get { return vIsFromNode; } }
		public override bool ExpectOneNode { get { return vExpectOneNode; } }

	}

}