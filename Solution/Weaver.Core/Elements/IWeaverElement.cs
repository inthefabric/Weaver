using Weaver.Core.Path;

namespace Weaver.Core.Elements {

	/*================================================================================================*/
	public interface IWeaverElement : IWeaverPathItem {

		string Id { get; set; }

	}


	/*================================================================================================*/
	public interface IWeaverElement<T> : IWeaverPathPipe<T> where T : class, IWeaverElement {

	}

}