using Weaver.Core.Elements;
using Weaver.Test.Core.Common.Edges;

namespace Weaver.Test.Core.Common.Vertices {

	/*================================================================================================*/
	public class Person : TestVertex<Person> {

		[WeaverItemProperty]
		public int PersonId { get; set; }

		[WeaverItemProperty]
		public bool IsMale { get; set; }

		[WeaverItemProperty]
		public float Age { get; set; }

		[WeaverItemProperty]
		public string Note { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public PersonLikesCandy OutLikesCandy {
			get { return NewEdge<PersonLikesCandy>(WeaverEdgeConn.OutToZeroOrMore); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public PersonKnowsPerson OutKnowsPerson {
			get { return NewEdge<PersonKnowsPerson>(WeaverEdgeConn.OutToZeroOrMore); }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RootHasPerson InRootHas {
			get { return NewEdge<RootHasPerson>(WeaverEdgeConn.InFromOne); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public PersonKnowsPerson InPersonKnows {
			get { return NewEdge<PersonKnowsPerson>(WeaverEdgeConn.InFromZeroOrMore); }
		}

	}

}