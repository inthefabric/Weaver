using Weaver.Core.Path;

namespace Weaver.Core.Elements {

	/*================================================================================================*/
	public abstract class WeaverGraphElement : WeaverPathItem, IWeaverElement {

		[WeaverItemProperty]
		public string Id { get; set; }

	}

}