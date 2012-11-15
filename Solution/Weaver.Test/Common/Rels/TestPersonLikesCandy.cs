using Fabric.Domain.Graph.Items;
using Fabric.Test.Common.Nodes;
using Fabric.Test.Common.RelTypes;

namespace Fabric.Test.Common.Rels {

	/*================================================================================================*/
	public class TestPersonLikesCandy : 
						WeaverRel<IQueryTestPerson, TestPerson, TestLikes, IQueryTestCandy, TestCandy> {

		public int TimesEaten { get; set; }
		public float Enjoyment { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestPersonLikesCandy(WeaverRelConn pConn) : base(pConn) {}

		/*--------------------------------------------------------------------------------------------*/
		public override string Label { get { return "PersonLikesCandy"; } }

	}

}