﻿using Weaver.Core.Elements;
using Weaver.Test.Common.Edges;
using Weaver.Test.Common.Schema;

namespace Weaver.Test.Common.Vertices {

	/*================================================================================================*/
	[WeaverVertex]
	public class Person : TestVertex {

		[WeaverProperty(TestSchema.Person_PersonId)]
		public int PersonId { get; set; }

		[WeaverProperty(TestSchema.Person_IsMale)]
		public bool IsMale { get; set; }

		[WeaverProperty(TestSchema.Person_Age)]
		public float Age { get; set; }

		[WeaverProperty(TestSchema.Person_Note)]
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