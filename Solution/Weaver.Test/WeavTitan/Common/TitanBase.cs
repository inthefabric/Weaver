using Weaver.Core.Elements;
using Weaver.Titan.Elements;

namespace Weaver.Test.WeavTitan.Common {

	/*================================================================================================*/
	[WeaverTitanVertex]
	public class TitanBase : WeaverVertex {

		[WeaverTitanProperty("TiNa")]
		public string TitanName { get; set; }

	}

}