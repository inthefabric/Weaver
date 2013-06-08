using Weaver.Core.Path;

namespace Weaver.Core.Items {

	/*================================================================================================*/
	public interface IWeaverItemWithPath {

		IWeaverPath Path { get; set; }
		int PathIndex { get; }

	}

}