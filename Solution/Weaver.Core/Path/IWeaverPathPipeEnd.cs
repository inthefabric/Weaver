using Weaver.Core.Elements;
using Weaver.Core.Query;

namespace Weaver.Core.Path {
	
	/*================================================================================================*/
	public interface IWeaverPathPipeEnd<T> : IWeaverPathItem where T : IWeaverElement {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery ToQuery();

	}

}