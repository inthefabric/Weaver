using Weaver.Titan.Elements;

namespace Weaver.Test.WeavTitan.Common {

	/*================================================================================================*/
	[WeaverTitanVertex]
	public class Two : TitanBase {

		[WeaverTitanProperty("TA")]
		public int A { get; set; }

		[WeaverTitanProperty("TB", EdgesForVertexCentricIndexing = new[] { typeof(OneKnowsTwo) })]
		public int B { get; set; }

		[WeaverTitanProperty("TC", EdgesForVertexCentricIndexing = new[] { typeof(OneKnowsTwo) })]
		public int C { get; set; }

		[WeaverTitanProperty("TD", EdgesForVertexCentricIndexing = new[] { typeof(OneKnowsTwo) })]
		public int D { get; set; }

		[WeaverTitanProperty("TE")]
		public int E { get; set; }

	}

}