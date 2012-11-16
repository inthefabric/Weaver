using Weaver.Items;
using Weaver.Test.Common.Rels;

namespace Weaver.Test.Common.Nodes {

	/*================================================================================================*/
	public class Person : TestNode, IQueryPerson {

		public int PersonId { get; set; }
		public bool IsMale { get; set; }
		public float Age { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IQueryPersonLikesCandy OutLikesCandy {
			get { return NewRel<PersonLikesCandy>(WeaverRelConn.OutToManyNodes); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public IQueryPersonKnowsPerson OutKnowsPerson {
			get { return NewRel<PersonKnowsPerson>(WeaverRelConn.OutToManyNodes); }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IQueryRootHasPerson InRootHas {
			get { return NewRel<RootHasPerson>(WeaverRelConn.InFromOneNode); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public IQueryPersonKnowsPerson InPersonKnows {
			get { return NewRel<PersonKnowsPerson>(WeaverRelConn.InFromManyNodes); }
		}

	}

}