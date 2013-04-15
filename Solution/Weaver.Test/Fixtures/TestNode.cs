using Weaver.Items;

namespace Weaver.Test.Common.Nodes {

	/*================================================================================================*/
	public abstract class TestNode : WeaverNode {

		[WeaverItemProperty]
		public string Name { get; set; }

	}

}