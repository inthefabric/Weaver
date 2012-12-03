using NUnit.Framework;
using Weaver.Interfaces;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.Rels;

namespace Weaver.Test.Fixtures.Items {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverNode {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(false, false, false, "inV")]
		[TestCase(false, false, true, "inV(0)")]
		[TestCase(false, true, false, "outV")]
		[TestCase(false, true, true, "outV(0)")]
		[TestCase(true, false, false, "v(0)")]
		[TestCase(true, false, true, "v(0)")]
		[TestCase(true, true, false, "v(0)")]
		[TestCase(true, true, true, "v(0)")]
		public void Gremlin(bool pIsRoot, bool pIsFrom, bool pExpectOne, string pExpectGremlin) {
			IWeaverNode n;
			
			if ( pIsRoot ) {
				n = new Root { IsFromNode = pIsFrom, ExpectOneNode = pExpectOne };
			}
			else {
				n = new Person { IsFromNode = pIsFrom, ExpectOneNode = pExpectOne };
			}

			Assert.AreEqual(pExpectGremlin, n.GremlinCode, "Incorrect GremlinCode.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewRel() {
			var path = new WeaverPath<Root>(new Root());
			var person = new Person() { Path = path };
			int pathLen0 = person.Path.Length;
			Assert.AreEqual(pathLen0, 1, "Incorrect initial Path.Length.");

			PersonLikesCandy rel = person.OutLikesCandy;
			Assert.AreEqual(path, rel.Path, "Incorrect Rel.Path.");

			int pathLen1 = person.Path.Length;
			int relI = pathLen1-1;
			Assert.AreEqual(pathLen0+1, pathLen1, "Incorrect Path.Length.");
			Assert.AreEqual(rel, person.Path.ItemAtIndex(relI), "Incorrect Path Item at index "+relI);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ItemIdentifier() {
			const int id = 99;
			var p = new Person() { Id = id };
			Assert.AreEqual("Person(Id="+id+")", p.ItemIdentifier, "Incorrect Node.ItemIdentifier.");
		}

	}

}