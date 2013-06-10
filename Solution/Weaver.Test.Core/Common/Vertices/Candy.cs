using Weaver.Core.Elements;
using Weaver.Test.Core.Common.Edges;

namespace Weaver.Test.Core.Common.Vertices {

	/*================================================================================================*/
	public class Candy : TestVertex<Candy> {

		[WeaverItemProperty]
		public int CandyId { get; set; }

		[WeaverItemProperty]
		public bool IsChocolate { get; set; }

		[WeaverItemProperty]
		public int Calories { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RootHasPerson InRootHas {
			get { return NewEdge<RootHasPerson>(WeaverEdgeConn.InFromOne); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public PersonLikesCandy InPersonLikes {
			get { return NewEdge<PersonLikesCandy>(WeaverEdgeConn.InFromZeroOrMore); }
		}
		
	}

}