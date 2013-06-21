using Weaver.Core.Elements;
using Weaver.Test.Common.Schema;

namespace Weaver.Test.Common.Vertices {

	/*================================================================================================*/
	public abstract class TestVertex : WeaverVertex {

		[WeaverProperty(TestSchema.Vertex_Name)]
		public string Name { get; set; }

	}

}