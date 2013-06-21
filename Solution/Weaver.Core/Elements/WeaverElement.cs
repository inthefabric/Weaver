using Weaver.Core.Pipe;

namespace Weaver.Core.Elements {

	/*================================================================================================*/
	public abstract class WeaverElement : WeaverPathPipe, IWeaverElement {

		[WeaverProperty("id")]
		public string Id { get; set; }

	}

}