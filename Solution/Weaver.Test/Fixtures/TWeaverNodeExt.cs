using Moq;
using NUnit.Framework;
using Weaver.Functions;
using Weaver.Interfaces;
using Weaver.Test.Common.Nodes;

namespace Weaver.Test.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverNodeExt : WeaverTestBase {
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ToNodeVar() {
			var person = new Person();
			
			var mockPath = new Mock<IWeaverPath>();
			person.Path = mockPath.Object;

			var mockVar = new Mock<IWeaverVarAlias<Person>>();
			mockVar.SetupGet(x => x.Name).Returns("_V0");

			Person result = person.ToNodeVar(mockVar.Object);

			Assert.AreEqual(person, result, "Incorrect result.");
			mockPath.Verify(x => x.AddItem(It.IsAny<WeaverFuncToNodeVar<Person>>()), Times.Once());
		}

	}

}