using Weaver.Items;
using Weaver.Test.Common.Rels;

namespace Weaver.Test.Common.Nodes {

	/*================================================================================================*/
	public class Candy : TestNode, IQueryCandy {

		public bool IsChocolate { get; set; }
		public int Calories { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Candy() {}

		/*--------------------------------------------------------------------------------------------*/
		public Candy(bool pIsRoot, bool pIsFromNode, bool pExpectOneNode) :
														base(pIsRoot, pIsFromNode, pExpectOneNode) {}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IQueryRootHasPerson InRootHas {
			get { return new RootHasPerson(WeaverRelConn.InFromOneNode); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public IQueryPersonLikesCandy InPersonLikes {
			get { return new PersonLikesCandy(WeaverRelConn.InFromManyNodes); }
		}
		
	}

}