using Weaver.Core.Elements;

namespace Weaver.Test.Core.Common.Vertices {

	/*================================================================================================*/
	public abstract class TestVertex<T> : WeaverVertex<T> where T : IWeaverVertex {

		[WeaverItemProperty]
		public string Name { get; set; }

	}

}