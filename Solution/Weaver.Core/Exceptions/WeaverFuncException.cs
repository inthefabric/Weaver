using Weaver.Core.Func;

namespace Weaver.Core.Exceptions {

	/*================================================================================================*/
	public class WeaverFuncException : WeaverException {

		public IWeaverFunc Func { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncException(IWeaverFunc pFunc, string pMessage) :
														base("Func", pFunc.ItemIdentifier, pMessage) {
			Func = pFunc;
		}

	}

}