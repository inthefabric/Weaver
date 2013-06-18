using Weaver.Core.Elements;

namespace Weaver.Test.Common.Vertices {

	/*================================================================================================*/
	public abstract class TestVertex : WeaverVertex {

		[WeaverItemProperty]
		public string Name { get; set; }

	}

}