using Moq;
using NUnit.Framework;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;
using Weaver.Titan;
using Weaver.Titan.Graph;
using Weaver.Titan.Steps;
using Weaver.Titan.Steps.Parameters;

namespace Weaver.Test.WeavTitan.Graph {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverTitanGraphQuery : WeaverTestBase {

		private WeaverTitanGraphQuery vQuery;
		private Mock<IWeaverPath> vMockPath;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();

			var mockQuery = new Mock<IWeaverQuery>();
			vMockPath = new Mock<IWeaverPath>();
			vMockPath.SetupGet(x => x.Query).Returns(mockQuery.Object);

			vQuery = new WeaverTitanGraphQuery(true);
			vQuery.Path = vMockPath.Object;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ElasticIndex() {
			var mockParam = new Mock<IWeaverParamElastic<Person>>();
			var list = new [] { mockParam.Object };

			Person p = vQuery.ElasticIndex(list);

			Assert.NotNull(p, "Result should be filled.");
			vMockPath.Verify(x => x.AddItem(It.IsAny<WeaverStepElasticIndex<Person>>()), Times.Once());
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ElasticIndexText() {
			Person p = vQuery.ElasticIndex<Person>(x => x.Name, "Zach");

			Assert.NotNull(p, "Result should be filled.");
			vMockPath.Verify(x => x.AddItem(It.IsAny<WeaverStepElasticIndex<Person>>()), Times.Once());
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true, "V")]
		[TestCase(false, "E")]
		public void BuildParameterizedString(bool pVertMode, string pExpect) {
			vQuery = new WeaverTitanGraphQuery(pVertMode);
			vQuery.Path = vMockPath.Object;

			Assert.AreEqual(pExpect, vQuery.BuildParameterizedString(), "Incorrect result.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		[Category("Integration")]
		public void ElasticIndexInteg() {
			IWeaverQuery q = WeavInst.TitanGraph()
				.QueryV().ElasticIndex(
					new WeaverParamElastic<Person>(x => x.PersonId, WeaverParamElasticOp.LessThan, 99),
					new WeaverParamElastic<Person>(x => x.Age, WeaverParamElasticOp.GreaterThan, 18)
				)
				.ToQuery();

			const string expect = "g.V"+
				".has('"+TestSchema.Person_PersonId+"',LESS_THAN,_P0)"+
				".has('"+TestSchema.Person_Age+"',GREATER_THAN,_P1);";

			Assert.AreEqual(expect, q.Script, "Incorrect script.");
		}

	}

}