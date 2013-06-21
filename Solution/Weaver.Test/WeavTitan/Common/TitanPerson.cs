using Weaver.Core.Elements;
using Weaver.Test.Common.Edges;
using Weaver.Test.Common.Schema;
using Weaver.Titan.Elements;

namespace Weaver.Test.WeavTitan.Common {

	/*================================================================================================*/
	[WeaverTitanVertex]
	public class TitanPerson : WeaverVertex {

		[WeaverTitanProperty(TestSchema.Person_PersonId)]
		public int PersonId { get; set; }

		[WeaverTitanProperty(TestSchema.Person_IsMale)]
		public bool IsMale { get; set; }

		[WeaverTitanProperty(TestSchema.Person_Age, TitanIndex=true, TitanElasticIndex=true)]
		public float Age { get; set; }

		[WeaverTitanProperty(TestSchema.Person_Note)]
		public string Note { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public PersonLikesCandy OutLikesCandy {
			get { return NewEdge<PersonLikesCandy>(WeaverEdgeConn.OutZeroOrMore); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public PersonKnowsPerson OutKnowsPerson {
			get { return NewEdge<PersonKnowsPerson>(WeaverEdgeConn.OutZeroOrMore); }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RootHasPerson InRootHas {
			get { return NewEdge<RootHasPerson>(WeaverEdgeConn.InOne); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public PersonKnowsPerson InPersonKnows {
			get { return NewEdge<PersonKnowsPerson>(WeaverEdgeConn.InZeroOrMore); }
		}

	}

}