using Weaver.Core.Elements;
using Weaver.Test.Common.Edges;

namespace Weaver.Test.Common.Vertices {

	/*================================================================================================*/
	public class Root : TestVertex<Root> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override bool IsRoot { get { return (Path == null || PathIndex == 0); } }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RootHasCandy OutHasCandy {
			get { return NewEdge<RootHasCandy>(WeaverEdgeConn.OutToOneOrMore); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public RootHasPerson OutHasPerson {
			get { return NewEdge<RootHasPerson>(WeaverEdgeConn.OutToOneOrMore); }
		}

	}

}