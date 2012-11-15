using Weaver.Items;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.RelTypes;

namespace Weaver.Test.Common.Rels {

	/*================================================================================================*/
	public class RootHasCandy :
							WeaverRel<IQueryRoot, Root, Has, IQueryCandy, Candy>,
																				IQueryRootHasCandy {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RootHasCandy(WeaverRelConn pConn) : base(pConn) {}

		/*--------------------------------------------------------------------------------------------*/
		//public override string Label { get { return "RootHasCandy"; } }

	}

}