using Weaver.Interfaces;

namespace Weaver.Exceptions {

	/*================================================================================================*/
	public class WeaverNodeException : WeaverException {

		public IWeaverNode Node { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverNodeException(IWeaverNode pNode, string pMessage) :
														base("Node", pNode.ItemIdentifier, pMessage) {
			Node = pNode;
		}

	}

}