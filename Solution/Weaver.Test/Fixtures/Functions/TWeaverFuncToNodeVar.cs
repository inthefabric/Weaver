using Moq;
using NUnit.Framework;
using Weaver.Functions;
using Weaver.Interfaces;
using Weaver.Test.Common.Nodes;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncToNodeVar : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			var mockVar = new Mock<IWeaverVarAlias<Person>>();
			mockVar.SetupGet(x => x.Name).Returns("_V0");

			var f = new WeaverFuncToNodeVar<Person>(mockVar.Object);
			Assert.NotNull(f, "Incorrect new().");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString() {
			var mockVar = new Mock<IWeaverVarAlias<Person>>();
			mockVar.SetupGet(x => x.Name).Returns("_V0");

			var f = new WeaverFuncToNodeVar<Person>(mockVar.Object);
			Assert.AreEqual("each{_V0=g.v(it.id)}", f.BuildParameterizedString(), "Incorrect result.");
		}

	}

}