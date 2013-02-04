using NUnit.Framework;
using Weaver.Functions;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.Rels;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncRemoveEach : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewNode() {
			var f = new WeaverFuncRemoveEach<Person>();
			Assert.True(f.RemoveNode, "Incorrect RemoveNode.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewRel() {
			var f = new WeaverFuncRemoveEach<PersonLikesCandy>();
			Assert.False(f.RemoveNode, "Incorrect RemoveNode.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedStringNode() {
			var f = new WeaverFuncRemoveEach<Person>();
			Assert.AreEqual("sideEffect{g.removeVertex(it)}", f.BuildParameterizedString(),
				"Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedStringRel() {
			var f = new WeaverFuncRemoveEach<PersonLikesCandy>();
			Assert.AreEqual("sideEffect{g.removeEdge(it)}", f.BuildParameterizedString(),
				"Incorrect result.");
		}

	}

}