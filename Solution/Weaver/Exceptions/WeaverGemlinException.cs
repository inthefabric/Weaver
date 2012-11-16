using Weaver.Interfaces;

namespace Weaver.Exceptions {

	/*================================================================================================*/
	public class WeaverGremlinException : WeaverException {

		public IWeaverItem Item { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverGremlinException(IWeaverItem pItem, string pMessage) :
													base("At item", pItem.ItemIdentifier, pMessage) {
			Item = pItem;
		}

	}

}