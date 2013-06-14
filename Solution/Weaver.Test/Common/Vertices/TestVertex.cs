using Weaver.Core.Elements;

namespace Weaver.Test.Common.Vertices {

	/*================================================================================================*/
	public abstract class TestVertex<T> : WeaverVertex<T> where T : class, IWeaverVertex {

		[WeaverItemProperty]
		public string Name { get; set; }

	}

}