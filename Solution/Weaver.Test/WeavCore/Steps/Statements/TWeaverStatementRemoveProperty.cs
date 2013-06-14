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
	public class TWeaverStatementRemoveProperty : WeaverTestBase {

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
			var remProp = new WeaverStatementRemoveProperty<Person>(x => x.PersonId);

			const string expect = "it.removeProperty('"+TestSchema.Person_PersonId+"')";

			Assert.AreEqual(expect, remProp.BuildParameterizedString(vMockPath.Object),
				"Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedStringInvalid() {
			var remProp = new WeaverStatementRemoveProperty<Person>(x => (x.PersonId == 99));

			WeaverTestUtil.CheckThrows<WeaverStatementException<Person>>(
				true, () => remProp.BuildParameterizedString(vMockPath.Object)
			);
		}
		

	}

}