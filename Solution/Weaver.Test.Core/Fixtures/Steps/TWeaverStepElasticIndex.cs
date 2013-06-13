using Moq;
using NUnit.Framework;
using Weaver.Core.Exceptions;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Core.Steps.Parameters;
using Weaver.Test.Core.Common.Schema;
using Weaver.Test.Core.Common.Vertices;
using Weaver.Test.Core.Utils;

namespace Weaver.Test.Core.Fixtures.Steps {

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
			var se = new WeaverStepElasticIndex<Person>();
			WeaverTestUtil.CheckThrows<WeaverStepException>(true, () => se.BuildParameterizedString());
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(1)]
		[TestCase(4)]
		public void BuildParameterizedString(int pCount) {
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

			var ei = new WeaverStepElasticIndex<Person>(list);
			ei.Path = vMockPath.Object;

			Assert.AreEqual(expect, ei.BuildParameterizedString(), "Incorrect result.");
		}

	}

}