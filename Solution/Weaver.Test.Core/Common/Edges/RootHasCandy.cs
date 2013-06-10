using Weaver.Core.Elements;
using Weaver.Test.Core.Common.EdgeTypes;
using Weaver.Test.Core.Common.Vertices;

namespace Weaver.Test.Core.Common.Edges {

	/*================================================================================================*/
	public class RootHasCandy : WeaverEdge<RootHasCandy, Root, Has, Candy> {
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RootHasCandy() {}

		/*--------------------------------------------------------------------------------------------*/
		public RootHasCandy(WeaverEdgeConn pConn) : base(pConn) {}

	}

}