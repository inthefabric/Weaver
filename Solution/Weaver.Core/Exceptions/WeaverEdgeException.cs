using Weaver.Core.Items;

namespace Weaver.Core.Exceptions {

	/*================================================================================================*/
	public class WeaverEdgeException : WeaverException {

		public IWeaverEdge Edge { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverEdgeException(IWeaverEdge pEdge, string pMessage) :
														base("Edge", pEdge.ItemIdentifier, pMessage) {
			Edge = pEdge;
		}

	}

}