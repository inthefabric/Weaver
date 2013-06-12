using System;
using Moq;
using NUnit.Framework;
using Weaver.Core.Query;
using Weaver.Test.Core.Common.Vertices;

namespace Weaver.Test.Core.Fixtures.Query {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverVarAlias : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true)]
		[TestCase(false)]
		public void New(bool pWithType) {
			const string varName = "_VAR";
			Type varType = typeof(object);

			var mockTx = new Mock<IWeaverTransaction>();
			mockTx.Setup(x => x.GetNextVarName()).Returns(varName);

			WeaverVarAlias va;

			if ( pWithType ) {
				va = new WeaverVarAlias(mockTx.Object);
			}
			else {
				varType = typeof(Person);
				va = new WeaverVarAlias(mockTx.Object, varType);
			}

			Assert.AreEqual(varName, va.Name, "Incorrect Name.");
			Assert.AreEqual(varType, va.VarType, "Incorrect VarType.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewT() {
			const string varName = "_VAR_T";

			var mockTx = new Mock<IWeaverTransaction>();
			mockTx.Setup(x => x.GetNextVarName()).Returns(varName);

			WeaverVarAlias va = new WeaverVarAlias<Person>(mockTx.Object);

			Assert.AreEqual(varName, va.Name, "Incorrect Name.");
			Assert.AreEqual(typeof(Person), va.VarType, "Incorrect VarType.");
		}

	}

}