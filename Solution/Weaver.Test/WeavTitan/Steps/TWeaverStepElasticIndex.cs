using Moq;
using NUnit.Framework;
using Weaver.Core.Exceptions;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;
using Weaver.Test.Utils;
using Weaver.Titan.Steps;
using Weaver.Titan.Steps.Parameters;

namespace Weaver.Test.WeavTitan.Steps {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverStepElasticIndex : WeaverTestBase {

		private Mock<IWeaverPath> vMockPath;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();

			vMockPath = new Mock<IWeaverPath>();
			vMockPath.SetupGet(x => x.Query).Returns(new WeaverQuery());
			vMockPath.SetupGet(x => x.Config).Returns(WeavInst.Config);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedStringFail() {
			var se = new WeaverStepElasticIndex<Person>(true);
			WeaverTestUtil.CheckThrows<WeaverStepException>(true, () => se.BuildParameterizedString());
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true, 1, "vertices")]
		[TestCase(true, 4, "vertices")]
		[TestCase(false, 1, "edges")]
		[TestCase(false, 4, "edges")]
		public void BuildParameterizedString(bool pVertMode, int pCount, string pEnd) {
			var list = new IWeaverParamElastic<Person>[pCount];
			string expect = "";

			for ( int i = 0 ; i < pCount ; ++i ) {
				string ops = "oper"+i;

				var mockState = new Mock<IWeaverParamElastic<Person>>();
				mockState.SetupGet(x => x.Property).Returns(x => x.PersonId);
				mockState.Setup(x => x.GetOperationScript()).Returns(ops);

				list[i] = mockState.Object;
				expect += (i == 0 ? "" : ".")+"has('"+TestSchema.Person_PersonId+"',"+ops+",_P"+i+")";
			}

			var ei = new WeaverStepElasticIndex<Person>(pVertMode, list);
			ei.Path = vMockPath.Object;

			expect += "."+pEnd+"()";
			Assert.AreEqual(expect, ei.BuildParameterizedString(), "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true, "vertices", "", new [] { "" })]
		[TestCase(false, "edges", "", new [] { "" })]
		[TestCase(true, "vertices", "zach", new[] { "zach" })]
		[TestCase(false, "edges", "Zach Testing Kinstner", new[] { "Zach", "Testing", "Kinstner" })]
		public void BuildParameterizedStringText(bool pVertMode, string pEnd, string pText, 
																					string[] pExpect) {
			string expect = "";

			for ( int i = 0 ; i < pExpect.Length ; ++i ) {
				expect += (i == 0 ? "" : ".")+"has('"+TestSchema.Vertex_Name+"',"+
					WeaverParamElastic.ContainsScript+",_P"+i+")";
			}

			var ei = new WeaverStepElasticIndex<Person>(pVertMode, x => x.Name, pText);
			ei.Path = vMockPath.Object;

			expect += "."+pEnd+"()";
			Assert.AreEqual(expect, ei.BuildParameterizedString(), "Incorrect result.");
		}

	}

}