using NUnit.Framework;
using Weaver.Core.Exceptions;
using Weaver.Core.Schema;
using Weaver.Test.Common.Edges;
using Weaver.Test.Common.Schema;
using Weaver.Test.Utils;
using Weaver.Titan.Schema;

namespace Weaver.Test.WeavTitan.Schema {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverTitanPropSchema : WeaverTestBase {

		private TestSchema vSchema;
		private WeaverEdgeSchema vPlc;
		private WeaverTitanPropSchema vProp;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();
			vSchema = new TestSchema();

			vPlc = vSchema.GetEdgeSchema(typeof(PersonLikesCandy).Name);

			vProp = new WeaverTitanPropSchema("TestProp", "TP", typeof(string));
			vProp.AddTitanVertexCentricIndex(vPlc);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddTitanVertexCentricIndex() {
			var pkp = vSchema.GetEdgeSchema(typeof(PersonKnowsPerson).Name);
			var rhp = vSchema.GetEdgeSchema(typeof(RootHasPerson).Name);

			Assert.AreEqual(2, vProp.AddTitanVertexCentricIndex(pkp), "Incorrect count 2.");
			Assert.AreEqual(3, vProp.AddTitanVertexCentricIndex(rhp), "Incorrect count 3.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddTitanVertexCentricIndexFail() {
			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => vProp.AddTitanVertexCentricIndex(vPlc)
			);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void HasTitanVertexCentricIndexEdge() {
			Assert.True(vProp.HasTitanVertexCentricIndex(vPlc), "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void HasTitanVertexCentricIndexDbName() {
			Assert.True(vProp.HasTitanVertexCentricIndex(vPlc.DbName), "Incorrect result.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetTitanTypeName() {
			Assert.AreEqual("String", vProp.GetTitanTypeName(), "Incorrect result.");
		}

	}

}