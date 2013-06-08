using Weaver.Core.Path;

namespace Weaver.Core.Exceptions {

	/*================================================================================================*/
	public class WeaverPathException : WeaverException {

		public IWeaverPath Path { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverPathException(IWeaverPath pPath, string pMessage) : base(pMessage) {
			Path = pPath;
		}

	}

}