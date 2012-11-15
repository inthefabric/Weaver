using Fabric.Domain.Graph.Items;
using Fabric.Test.Common.Rels;

namespace Fabric.Test.Common.Nodes {

	/*================================================================================================*/
	public class Person : TestNode, IQueryPerson {

		public bool IsMale { get; set; }
		public float Age { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Person() { }

		/*--------------------------------------------------------------------------------------------*/
		public Person(bool pIsRoot, bool pIsFromNode, bool pExpectOneNode) :
			base(pIsRoot, pIsFromNode, pExpectOneNode) { }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IQueryPersonLikesCandy OutLikesCandy {
			get { return new PersonLikesCandy(WeaverRelConn.OutToManyNodes); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public IQueryPersonKnowsPerson OutKnowsPerson {
			get { return new PersonKnowsPerson(WeaverRelConn.OutToManyNodes); }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IQueryRootHasPerson InRootHas {
			get { return new RootHasPerson(WeaverRelConn.InFromOneNode); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public IQueryPersonKnowsPerson InPersonKnows {
			get { return new PersonKnowsPerson(WeaverRelConn.InFromManyNodes); }
		}

	}

}