using Weaver.Items;
using Weaver.Test.Common.Rels;

namespace Weaver.Test.Common.Nodes {

	/*================================================================================================*/
	public class Candy : TestNode {

		public int CandyId { get; set; }
		public bool IsChocolate { get; set; }
		public int Calories { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RootHasPerson InRootHas {
			get { return NewRel<RootHasPerson>(WeaverRelConn.InFromOne); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public PersonLikesCandy InPersonLikes {
			get { return NewRel<PersonLikesCandy>(WeaverRelConn.InFromZeroOrMore); }
		}
		
	}

}