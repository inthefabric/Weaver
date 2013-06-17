using Weaver.Core.Elements;
using Weaver.Test.Common.EdgeTypes;
using Weaver.Test.Common.Vertices;

namespace Weaver.Test.Common.Edges {

	/*================================================================================================*/
	public class RootHasCandy : WeaverEdge<RootHasCandy, Root, Has, Candy> {
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RootHasCandy() {}

		/*--------------------------------------------------------------------------------------------*/
		public RootHasCandy(WeaverEdgeConn pConn) : base(pConn) {}

	}

}