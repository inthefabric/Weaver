using Weaver.Core.Elements;
using Weaver.Test.Common.EdgeTypes;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;

namespace Weaver.Test.Common.Edges {

	/*================================================================================================*/
	[WeaverEdge(TestSchema.RootHasCandy, typeof(Root), typeof(Candy))]
	public class RootHasCandy : WeaverEdge<Root, Has, Candy> {
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RootHasCandy() {}

		/*--------------------------------------------------------------------------------------------*/
		public RootHasCandy(WeaverEdgeConn pConn) : base(pConn) {}

	}

}