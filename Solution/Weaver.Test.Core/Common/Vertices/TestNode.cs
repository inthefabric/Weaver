using Weaver.Core.Elements;

namespace Weaver.Test.Core.Common.Vertices {

	/*================================================================================================*/
	public abstract class TestVertex : WeaverVertex {

		[WeaverItemProperty]
		public string Name { get; set; }

	}

}