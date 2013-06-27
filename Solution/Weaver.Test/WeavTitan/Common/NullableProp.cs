using Weaver.Core.Elements;
using Weaver.Titan.Elements;

namespace Weaver.Test.WeavTitan.Common {

	/*================================================================================================*/
	[WeaverTitanVertex]
	public class NullableProp : WeaverVertex {

		[WeaverTitanProperty("A")]
		public int? A { get; set; }

		[WeaverProperty("NTA")]
		public string NonTitanAttribute { get; set; }

	}

}