using Weaver.Items;
using Weaver.Test.Common.Rels;

namespace Weaver.Test.Common.Nodes {

	/*================================================================================================*/
	public class Person : TestNode {

		public int PersonId { get; set; }
		public bool IsMale { get; set; }
		public float Age { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public PersonLikesCandy OutLikesCandy {
			get { return NewRel<PersonLikesCandy>(WeaverRelConn.OutToZeroOrMore); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public PersonKnowsPerson OutKnowsPerson {
			get { return NewRel<PersonKnowsPerson>(WeaverRelConn.OutToZeroOrMore); }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RootHasPerson InRootHas {
			get { return NewRel<RootHasPerson>(WeaverRelConn.InFromOne); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public PersonKnowsPerson InPersonKnows {
			get { return NewRel<PersonKnowsPerson>(WeaverRelConn.InFromZeroOrMore); }
		}

	}

}