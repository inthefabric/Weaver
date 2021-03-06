﻿using Moq;
using NUnit.Framework;
using Weaver.Core.Exceptions;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Core.Steps.Statements;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;
using Weaver.Test.Utils;

namespace Weaver.Test.WeavCore.Steps {

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
		public void BuildParameterizedStringFail() {
			var se = new WeaverStepSideEffect<Person>();
			WeaverTestUtil.CheckThrows<WeaverStepException>(true, () => se.BuildParameterizedString());
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(1)]
		[TestCase(4)]
		public void BuildParameterizedString(int pCount) {
			var statements = new IWeaverStatement<Person>[pCount];
			string expect = "sideEffect{";

			for ( int i = 0 ; i < pCount ; ++i ) {
				string script = "statement"+i;

				var mockState = new Mock<IWeaverStatement<Person>>();
				mockState.Setup(x => x.BuildParameterizedString(vMockPath.Object)).Returns(script);

				statements[i] = mockState.Object;
				expect += script+";";
			}

			var se = new WeaverStepSideEffect<Person>(statements);
			se.Path = vMockPath.Object;
			Assert.AreEqual(expect+"}", se.BuildParameterizedString(), "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		[Category("Integration")]
		public void BuildParameterizedStringInteg() {
			var s0 = new WeaverStatementSetProperty<Person>(x => x.Name, "name");
			var s1 = new WeaverStatementSetProperty<Person>(x => x.Age, 99.9);
			var s2 = new WeaverStatementSetProperty<Person>(x => x.Note, "note");

			var se = new WeaverStepSideEffect<Person>(s0, s1, s2);
			se.Path = vMockPath.Object;

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