using Weaver.Core.Pipe;

namespace Weaver.Core.Elements {

	/*================================================================================================*/
	public abstract class WeaverElement<T> : WeaverPathPipe<T>, IWeaverElement<T>
																	where T : class, IWeaverElement {

		[WeaverItemProperty]
		public string Id { get; set; }

	}

}