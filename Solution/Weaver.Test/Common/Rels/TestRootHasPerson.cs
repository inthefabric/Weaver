using Fabric.Domain.Graph.Items;
using Fabric.Test.Common.Nodes;
using Fabric.Test.Common.RelTypes;

namespace Fabric.Test.Common.Rels {

	/*================================================================================================*/
	public class TestRootHasPerson : 
							WeaverRel<IQueryTestRoot, TestRoot, TestHas, IQueryTestPerson, TestPerson> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestRootHasPerson(WeaverRelConn pConn) : base(pConn) {}

		/*--------------------------------------------------------------------------------------------*/
		public override string Label { get { return "RootHasPerson"; } }

	}

}