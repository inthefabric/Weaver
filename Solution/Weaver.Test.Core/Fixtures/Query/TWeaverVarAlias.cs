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
			const string name = "xyz";
			Type varType = typeof(object);

			WeaverVarAlias va;

			if ( pWithType ) {
				va = new WeaverVarAlias(name);
			}
			else {
				varType = typeof(Person);
				va = new WeaverVarAlias(name, varType);
			}

			Assert.AreEqual(name, va.Name, "Incorrect Name.");
			Assert.AreEqual(varType, va.VarType, "Incorrect VarType.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewT() {
			const string name = "xyz_T";

			var mockTx = new Mock<IWeaverTransaction>();
			mockTx.Setup(x => x.GetNextVarName()).Returns(name);

			WeaverVarAlias va = new WeaverVarAlias<Person>(name);

			Assert.AreEqual(name, va.Name, "Incorrect Name.");
			Assert.AreEqual(typeof(Person), va.VarType, "Incorrect VarType.");
		}

	}

}