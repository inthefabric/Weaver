namespace Fabric.Domain.Graph.Schema {

	/*================================================================================================*/
	public class WeaverRelSchema {

		public WeaverNodeSchema FromNode { get; private set; }
		public string Name { get; private set; }
		public WeaverNodeSchema ToNode { get; private set; }

		public bool Many { get; set; }
		public bool RevMany { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverRelSchema(WeaverNodeSchema pFromNode, string pName, WeaverNodeSchema pToNode) {
			FromNode = pFromNode;
			Name = pName;
			ToNode = pToNode;
		}

	}

}