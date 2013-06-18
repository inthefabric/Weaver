using Weaver.Core.Pipe;

namespace Weaver.Core.Elements {

	/*================================================================================================*/
	public abstract class WeaverElement : WeaverPathPipe, IWeaverElement {

		[WeaverItemProperty]
		public string Id { get; set; }

	}

}