using Moq;
using NUnit.Framework;
using Weaver.Core.Graph;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;

namespace Weaver.Test.WeavCore.Graph {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverAllVertices : WeaverTestBase {

		private WeaverAllVertices vAllVerts;
		private Mock<IWeaverPath> vMockPath;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();

			var mockQuery = new Mock<IWeaverQuery>();
			vMockPath = new Mock<IWeaverPath>();
			vMockPath.SetupGet(x => x.Query).Returns(mockQuery.Object);

			vAllVerts = new WeaverAllVertices();
			vAllVerts.Path = vMockPath.Object;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Id() {
			Person p = vAllVerts.Id<Person>("5");

			Assert.NotNull(p, "Result should be filled.");
			vMockPath.Verify(x => x.AddItem(It.IsAny<WeaverStepCustom>()), Times.Once());
			Assert.True(vAllVerts.ForSpecificId, "Incorrect ForSpecificId.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ExactIndex() {
			Person p = vAllVerts.ExactIndex<Person>(x => x.PersonId, 5);
			
			Assert.NotNull(p, "Result should be filled.");
			vMockPath.Verify(x => x.AddItem(It.IsAny<WeaverStepExactIndex<Person>>()), Times.Once());
			Assert.False(vAllVerts.ForSpecificId, "Incorrect ForSpecificId.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ElasticIndex() {
			var mockParam = new Mock<IWeaverParamElastic<Person>>();
			var list = new [] { mockParam.Object };

			Person p = vAllVerts.ElasticIndex(list);

			Assert.NotNull(p, "Result should be filled.");
			vMockPath.Verify(x => x.AddItem(It.IsAny<WeaverStepElasticIndex<Person>>()), Times.Once());
			Assert.False(vAllVerts.ForSpecificId, "Incorrect ForSpecificId.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true, "v")]
		[TestCase(false, "V")]
		public void BuildParameterizedString(bool pForId, string pExpect) {
			if ( pForId ) {
				vAllVerts.Id<Person>("x");
			}

			Assert.AreEqual(pExpect, vAllVerts.BuildParameterizedString(), "Incorrect result.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		[Category("Integration")]
		public void ElasticIndexInteg() {
			IWeaverQuery q = WeavInst.Graph
				.V.ElasticIndex(
					new WeaverParamElastic<Person>(x => x.PersonId, WeaverParamElasticOp.LessThan, 99),
					new WeaverParamElastic<Person>(x => x.Age, WeaverParamElasticOp.GreaterThan, 18)
				)
				.ToQuery();

			const string expect = "g.V"+
				".has('"+TestSchema.Person_PersonId+"',"+
					"com.tinkerpop.blueprints.Query.Compare.LESS_THAN,_P0)"+
				".has('"+TestSchema.Person_Age+"',"+
					"com.tinkerpop.blueprints.Query.Compare.GREATER_THAN,_P1);";

			Assert.AreEqual(expect, q.Script, "Incorrect script.");
		}

	}

}