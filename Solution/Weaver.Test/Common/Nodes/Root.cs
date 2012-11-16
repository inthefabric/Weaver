using Weaver.Items;
using Weaver.Test.Common.Rels;

namespace Weaver.Test.Common.Nodes {

	/*================================================================================================*/
	public class Root : TestNode, IQueryRoot {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override bool IsRoot { get { return (QueryPathIndex == 0); } }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IQueryRootHasCandy OutHasCandy {
			get { return NewRel<RootHasCandy>(WeaverRelConn.OutToManyNodes); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public IQueryRootHasPerson OutHasPerson {
			get { return NewRel<RootHasPerson>(WeaverRelConn.OutToManyNodes); }
		}

	}

}