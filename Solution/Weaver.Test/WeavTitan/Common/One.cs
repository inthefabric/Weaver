using Weaver.Titan.Elements;
using Weaver.Core.Elements;

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
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public OneKnowsTwo KnowsTwo {
			get { return NewEdge<OneKnowsTwo>(WeaverEdgeConn.OutZeroOrMore); }
		}

	}

}