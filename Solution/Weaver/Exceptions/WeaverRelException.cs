using Weaver.Interfaces;

namespace Weaver.Exceptions {

	/*================================================================================================*/
	public class WeaverRelException : WeaverException {

		public IWeaverRel Rel { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverRelException(IWeaverRel pRel, string pMessage) :
															base("Rel", pRel.ItemIdentifier, pMessage) {
			Rel = pRel;
		}

	}

}