using Fabric.Domain.Graph.Items;
using Fabric.Test.Common.Nodes;
using Fabric.Test.Common.RelTypes;

namespace Fabric.Test.Common.Rels {

	/*================================================================================================*/
	public class RootHasPerson : 
							WeaverRel<IQueryRoot, Root, Has, IQueryPerson, Person>,
																			IQueryRootHasPerson {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RootHasPerson(WeaverRelConn pConn) : base(pConn) {}

		/*--------------------------------------------------------------------------------------------*/
		//public override string Label { get { return "RootHasPerson"; } }

	}

}