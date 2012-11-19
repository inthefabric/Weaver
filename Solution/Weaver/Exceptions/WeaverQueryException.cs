using Weaver.Interfaces;

namespace Weaver.Exceptions {

	/*================================================================================================*/
	public class WeaverQueryException : WeaverException {

		public IWeaverQuery Query { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverQueryException(IWeaverQuery pQuery, string pMessage) : base(pMessage) {
			Query = pQuery;
		}

	}

}