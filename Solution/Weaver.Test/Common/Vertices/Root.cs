using Weaver.Core.Elements;
using Weaver.Test.Common.Edges;

namespace Weaver.Test.Common.Vertices {

	/*================================================================================================*/
	[WeaverVertex]
	public class Root : TestVertex {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override bool IsRoot { get { return (Path == null || PathIndex == 0); } }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RootHasCandy OutHasCandy {
			get { return NewEdge<RootHasCandy>(WeaverEdgeConn.OutOneOrMore); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public RootHasPerson OutHasPerson {
			get { return NewEdge<RootHasPerson>(WeaverEdgeConn.OutOneOrMore); }
		}

	}

}