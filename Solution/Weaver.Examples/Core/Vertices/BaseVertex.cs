using Weaver.Core.Elements;

namespace Weaver.Examples.Core.Vertices {

	/*================================================================================================*/
	public abstract class BaseVertex : WeaverVertex {

		[WeaverProperty("name")]
		public string Name { get; set; }
		
		[WeaverProperty("type")]
		public string Type { get; set; }

	}

}