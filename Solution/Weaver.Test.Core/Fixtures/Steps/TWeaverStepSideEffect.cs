using System;
using Moq;
using NUnit.Framework;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Core.Steps.Statements;
using Weaver.Test.Core.Common.Schema;
using Weaver.Test.Core.Common.Vertices;

namespace Weaver.Test.Core.Fixtures.Steps {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverStepSideEffect : WeaverTestBase {

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
		public void BuildParameterizedString1() {
			var s0 = new WeaverStatementSetProperty<Person>(vMockPath.Object, x => x.Note, "note");
			var se = new WeaverStepSideEffect<Person>(s0);

			const string expect =
				"sideEffect{"+
					"it.setProperty('"+TestSchema.Person_Note+"',_P0);"+
				"}";

			Assert.AreEqual(expect, se.BuildParameterizedString(), "Incorrect result.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString3() {
			var s0 = new WeaverStatementSetProperty<Person>(vMockPath.Object, x => x.Name, "name");
			var s1 = new WeaverStatementSetProperty<Person>(vMockPath.Object, x => x.Age, 99.9);
			var s2 = new WeaverStatementSetProperty<Person>(vMockPath.Object, x => x.Note, "note");
			var se = new WeaverStepSideEffect<Person>(s0, s1, s2);

			const string expect = 
				"sideEffect{"+
					"it.setProperty('"+TestSchema.Vertex_Name+"',_P0);"+
					"it.setProperty('"+TestSchema.Person_Age+"',_P1);"+
					"it.setProperty('"+TestSchema.Person_Note+"',_P2);"+
				"}";

			Assert.AreEqual(expect, se.BuildParameterizedString(), "Incorrect result.");
		}

	}

}