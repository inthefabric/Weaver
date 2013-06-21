using Weaver.Core.Elements;
using Weaver.Test.Common.EdgeTypes;
using Weaver.Titan.Elements;

namespace Weaver.Test.WeavTitan.Common {

	/*================================================================================================*/
	[WeaverTitanEdge("OKT", WeaverEdgeConn.OutZeroOrOne, typeof(One), WeaverEdgeConn.InOne,typeof(Two))]
	public class OneKnowsTwo : WeaverEdge<One, Knows, Two> {

	}

}