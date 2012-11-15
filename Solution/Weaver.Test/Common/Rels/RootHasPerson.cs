using Weaver.Items;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.RelTypes;

namespace Weaver.Test.Common.Rels {

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