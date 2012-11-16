namespace Weaver.Exceptions {

	/*================================================================================================*/
	public class WeaverQueryException : WeaverException {

		public WeaverQuery Query { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverQueryException(WeaverQuery pQuery, string pMessage) : base(pMessage) {
			Query = pQuery;
		}

	}

}