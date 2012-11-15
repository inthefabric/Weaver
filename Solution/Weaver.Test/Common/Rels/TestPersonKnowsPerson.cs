using Fabric.Domain.Graph.Items;
using Fabric.Test.Common.Nodes;
using Fabric.Test.Common.RelTypes;

namespace Fabric.Test.Common.Rels {

	/*================================================================================================*/
	public class TestPersonKnowsPerson : 
					WeaverRel<IQueryTestPerson, TestPerson, TestKnows, IQueryTestPerson, TestPerson> {

		public string MetOnDate { get; set; }
		public float Amount { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestPersonKnowsPerson(WeaverRelConn pConn) : base(pConn) {}

		/*--------------------------------------------------------------------------------------------*/
		public override string Label { get { return "PersonKnowsPerson"; } }

	}

}