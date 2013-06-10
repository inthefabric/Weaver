using Weaver.Core.Path;

namespace Weaver.Core.Elements {

	/*================================================================================================*/
	public abstract class WeaverElement<T> : WeaverPathPipe<T>, IWeaverElement<T>
																			where T : IWeaverElement {

		[WeaverItemProperty]
		public string Id { get; set; }

	}

}