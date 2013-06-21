using Weaver.Core.Elements;
using Weaver.Test.Common.EdgeTypes;
using Weaver.Titan.Elements;

namespace Weaver.Test.WeavTitan.Common {

	/*================================================================================================*/
	[WeaverTitanEdge("EHE",
		WeaverEdgeConn.OutZeroOrMore, typeof(Empty),
		WeaverEdgeConn.InOneOrMore, typeof(Empty)
	)]
	public class EmptyHasEmpty : WeaverEdge<Empty, Has, Empty> {

	}

}