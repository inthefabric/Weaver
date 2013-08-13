using System;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using Weaver.Core;
using Weaver.Core.Path;
using Weaver.Core.Steps;
using Weaver.Test.Common.Vertices;

namespace Weaver.Test.WeavCore.Steps {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverStepAsColumn : WeaverTestBase {

		private string vLabel;
		private string vProp;
		private WeaverStepAsColumn<Person> vObjCol;
		private WeaverStepAsColumn<Person> vPropCol;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			vLabel = "MyColumn";
			vProp = "PerName";

			var mockPath = new Mock<IWeaverPath>();
			var mockPerson = new Mock<Person>();
			mockPerson.SetupGet(x => x.Path).Returns(mockPath.Object);

			Expression<Func<Person, object>> exp = (x => x.Name);

			var mockCfg = new Mock<IWeaverConfig>();
			mockCfg.Setup(x => x.GetPropertyDbName(exp)).Returns(vProp);

			vObjCol = new WeaverStepAsColumn<Person>(mockPerson.Object, mockCfg.Object, vLabel);
			vPropCol = new WeaverStepAsColumn<Person>(mockPerson.Object, mockCfg.Object, vLabel, exp);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewObj() {
			Assert.AreEqual(vLabel, vObjCol.Label, "Incorrect Label.");
			Assert.Null(vObjCol.PropName, "PropName should be null.");
			Assert.AreEqual("", vObjCol.AppendScript, "Incorrect AppendScript.");
			Assert.Null(vObjCol.ReplaceScript, "ReplaceScript should be null.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewProp() {
			Assert.AreEqual(vLabel, vPropCol.Label, "Incorrect Label.");
			Assert.AreEqual(vProp, vPropCol.PropName, "Incorrect PropName.");
			Assert.AreEqual("", vPropCol.AppendScript, "Incorrect AppendScript.");
			Assert.Null(vPropCol.ReplaceScript, "ReplaceScript should be null.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedStringObj() {
			Assert.AreEqual("as('"+vLabel+"')", vObjCol.BuildParameterizedString(),
				"Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedStringProp() {
			Assert.AreEqual("as('"+vLabel+"')", vPropCol.BuildParameterizedString(),
				"Incorrect result.");
		}

	}

}