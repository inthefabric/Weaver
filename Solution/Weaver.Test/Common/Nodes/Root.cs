using Weaver.Items;
using Weaver.Test.Common.Rels;

namespace Weaver.Test.Common.Nodes {

	/*================================================================================================*/
	public class Root : TestNode {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override bool IsRoot { get { return (Path == null || PathIndex == 0); } }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RootHasCandy OutHasCandy {
			get { return NewRel<RootHasCandy>(WeaverRelConn.OutToOneOrMore); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public RootHasPerson OutHasPerson {
			get { return NewRel<RootHasPerson>(WeaverRelConn.OutToOneOrMore); }
		}

	}

}