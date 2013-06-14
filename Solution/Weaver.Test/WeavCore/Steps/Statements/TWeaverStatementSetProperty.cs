using Moq;
using NUnit.Framework;
using Weaver.Core.Exceptions;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Steps.Statements;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;
using Weaver.Test.Utils;

namespace Weaver.Test.WeavCore.Steps.Statements {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverStatementSetProperty : WeaverTestBase {

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
		public void BuildParameterizedString() {
			var setProp = new WeaverStatementSetProperty<Person>(x => x.PersonId, 5);

			const string expect = "it.setProperty('"+TestSchema.Person_PersonId+"',_P0)";

			Assert.AreEqual(expect, setProp.BuildParameterizedString(vMockPath.Object),
				"Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedStringInvalid() {
			var setProp = new WeaverStatementSetProperty<Person>(x => (x.PersonId == 99), 5);

			WeaverTestUtil.CheckThrows<WeaverStatementException<Person>>(
				true, () => setProp.BuildParameterizedString(vMockPath.Object)
			);
		}
		

	}

}