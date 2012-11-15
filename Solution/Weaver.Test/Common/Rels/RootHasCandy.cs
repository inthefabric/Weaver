using Fabric.Domain.Graph.Items;
using Fabric.Test.Common.Nodes;
using Fabric.Test.Common.RelTypes;

namespace Fabric.Test.Common.Rels {

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