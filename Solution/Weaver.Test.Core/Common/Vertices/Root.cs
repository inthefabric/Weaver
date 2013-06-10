using Weaver.Core.Elements;
using Weaver.Test.Core.Common.Edges;

namespace Weaver.Test.Core.Common.Vertices {

	/*================================================================================================*/
	public class Root : TestVertex {


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