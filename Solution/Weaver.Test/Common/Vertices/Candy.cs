using Weaver.Core.Elements;
using Weaver.Test.Common.Edges;
using Weaver.Test.Common.Schema;

namespace Weaver.Test.Common.Vertices {

	/*================================================================================================*/
	[WeaverVertex]
	public class Candy : TestVertex {

		[WeaverProperty(TestSchema.Candy_CandyId)]
		public int CandyId { get; set; }

		[WeaverProperty(TestSchema.Candy_IsChocolate)]
		public bool IsChocolate { get; set; }

		[WeaverProperty(TestSchema.Candy_Calories)]
		public int Calories { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RootHasPerson InRootHas {
			get { return NewEdge<RootHasPerson>(WeaverEdgeConn.InOne); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public PersonLikesCandy InPersonLikes {
			get { return NewEdge<PersonLikesCandy>(WeaverEdgeConn.InZeroOrMore); }
		}
		
	}

}