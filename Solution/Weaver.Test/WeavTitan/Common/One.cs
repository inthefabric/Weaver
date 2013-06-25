using Weaver.Titan.Elements;

namespace Weaver.Test.WeavTitan.Common {

	/*================================================================================================*/
	[WeaverTitanVertex]
	public class One : TitanBase {

		[WeaverTitanProperty("OA", EdgesForVertexCentricIndexing = new [] { typeof(OneKnowsTwo) })]
		public int A { get; set; }

		[WeaverTitanProperty("OB")]
		public int B { get; set; }

		[WeaverTitanProperty("OC")]
		public int C { get; set; }

		[WeaverTitanProperty("OD", EdgesForVertexCentricIndexing = new[] { typeof(OneKnowsTwo) })]
		public int D { get; set; }

		[WeaverTitanProperty("OE")]
		public int E { get; set; }

	}

}