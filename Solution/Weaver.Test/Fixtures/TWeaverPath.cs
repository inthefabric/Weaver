using System.Collections.Generic;
using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;
using Weaver.Items;
using Weaver.Test.Common;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.Rels;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverPath : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void StartEmpty() {
			var p = new WeaverPath<Root>();

			Assert.Null(p.BaseNode, "BaseNode should be null.");
			Assert.Null(p.BaseIndex, "BaseIndex should be null.");
			Assert.AreEqual(0, p.Length, "Incorrect Length.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void StartWithBase() {
			var r = new Root();
			var p = new WeaverPath<Root>(r);

			Assert.NotNull(p.BaseNode, "BaseNode should be filled.");
			Assert.AreEqual(p, p.BaseNode.Path, "Incorrect BaseNode.Path.");
			Assert.Null(p.BaseIndex, "BaseIndex should be null.");

			Assert.AreEqual(1, p.Length, "Incorrect Length.");
			Assert.AreEqual(r, p.ItemAtIndex(0), "Incorrect item at index 0.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void StartWithIndex() {
			const int perId = 99;
			const string indexName = "Person";

			var p = new WeaverPath<Person>();
			p.StartWithIndex<Person>(indexName, (x => x.PersonId), perId);

			Assert.NotNull(p.BaseNode, "BaseNode should be filled.");
			Assert.AreEqual(p, p.BaseNode.Path, "Incorrect BaseNode.Path.");
			Assert.NotNull(p.BaseIndex, "BaseIndex should be filled.");

			Assert.AreEqual(indexName, p.BaseIndex.IndexName, "Incorrect BaseIndex.IndexName.");
			Assert.AreEqual(indexName+"Id", p.BaseIndex.PropertyName,
				"Incorrect BaseIndex.PropertyName.");
			Assert.AreEqual(perId, p.BaseIndex.Value, "Incorrect BaseIndex.Value.");

			Assert.AreEqual(1, p.Length, "Incorrect Length.");
			Assert.AreEqual(p.BaseIndex, p.ItemAtIndex(0), "Incorrect item at index 0.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void StartWithIndexFail() {
			var p = new WeaverPath<Person>(new Person());

			WeaverTestUtils.CheckThrows<WeaverPathException>(true,
				() => p.StartWithIndex<Person>("P", (x => x.PersonId), 1)
			);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddItem() {
			var p = new TestPath();
			var candy = new Candy();
			p.AddItem(candy);

			const int len = 2;
			Assert.AreEqual(len, p.Length, "Incorrect Length.");
			Assert.AreEqual(candy, p.ItemAtIndex(len-1), "Incorrect item at index "+(len-1)+".");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Length0() {
			var p = new WeaverPath<Root>();
			Assert.AreEqual(0, p.Length, "Incorrect Length.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Length1() {
			var p = new WeaverPath<Root>(new Root());
			Assert.AreEqual(1, p.Length, "Incorrect Length.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Length3() {
			var p = new WeaverPath<Root>(new Root());
			Person person = p.BaseNode.OutHasPerson.ToNode;
			Assert.AreEqual(3, p.Length, "Incorrect Length.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ItemAtIndex() {
			var r = new Root();
			var p = new WeaverPath<Root>(r);
			RootHasPerson i1 = p.BaseNode.OutHasPerson;
			Person i2 = i1.ToNode;
			PersonLikesCandy i3 = i2.OutLikesCandy;
			Candy i4 = i3.ToNode;

			Assert.AreEqual(5, p.Length, "Incorrect Length.");
			Assert.AreEqual(r, p.ItemAtIndex(0), "Incorrect item at index 0.");
			Assert.AreEqual(i1, p.ItemAtIndex(1), "Incorrect item at index 1.");
			Assert.AreEqual(i2, p.ItemAtIndex(2), "Incorrect item at index 2.");
			Assert.AreEqual(i3, p.ItemAtIndex(3), "Incorrect item at index 3.");
			Assert.AreEqual(i4, p.ItemAtIndex(4), "Incorrect item at index 4.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(-1)]
		[TestCase(3)]
		public void ItemAtIndexBounds(int pIndex) {
			TestPath p = GetTestPathLength3();

			WeaverTestUtils.CheckThrows<WeaverPathException>(true,
				() => p.ItemAtIndex(pIndex)
			);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(0, true, 1)]
		[TestCase(1, true, 2)]
		[TestCase(1, false, 1)]
		[TestCase(2, true, 3)]
		[TestCase(4, true, 5)]
		[TestCase(4, false, 4)]
		[TestCase(5, false, 5)]
		public void PathToIndex(int pIndex, bool pInclusive, int pCount) {
			TestPath p = GetTestPathLength5();

			IList<IWeaverItem> result = p.PathToIndex(pIndex, pInclusive);

			Assert.NotNull(result, "Result should be filled.");
			Assert.AreEqual(pCount, result.Count, "Incorrect Result.Count.");
			Assert.AreEqual(p.BaseNode, result[0], "Incorrect Result[0].");

			int ri = pCount-1;
			Assert.AreEqual(p.ItemAtIndex(ri), result[ri], "Incorrect Result["+ri+"].");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(-1, true, true)]
		[TestCase(-1, false, true)]
		[TestCase(0, true, false)]
		[TestCase(0, false, true)]
		[TestCase(2, true, false)]
		[TestCase(2, false, false)]
		[TestCase(3, true, true)]
		[TestCase(3, false, false)]
		public void PathToIndexBounds(int pIndex, bool pInclusive, bool pThrows) {
			TestPath p = GetTestPathLength3();

			WeaverTestUtils.CheckThrows<WeaverPathException>(pThrows,
				() => p.PathToIndex(pIndex, pInclusive)
			);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(0, true, 5)]
		[TestCase(0, false, 4)]
		[TestCase(1, true, 4)]
		[TestCase(1, false, 3)]
		[TestCase(2, true, 3)]
		[TestCase(3, true, 2)]
		[TestCase(3, false, 1)]
		[TestCase(4, true, 1)]
		public void PathFromIndex(int pIndex, bool pInclusive, int pCount) {
			TestPath p = GetTestPathLength5();

			IList<IWeaverItem> result = p.PathFromIndex(pIndex, pInclusive);

			Assert.NotNull(result, "Result should be filled.");
			Assert.AreEqual(pCount, result.Count, "Incorrect Result.Count.");

			int pi = pIndex + (pInclusive ? 0 : 1);
			int ri = 0;
			Assert.AreEqual(p.ItemAtIndex(pi), result[ri], "Incorrect Result["+ri+"].");

			pi = p.Length-1;
			ri = pCount-1;
			Assert.AreEqual(p.ItemAtIndex(pi), result[ri], "Incorrect Result["+ri+"].");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(-1, true, true)]
		[TestCase(-1, false, false)]
		[TestCase(0, true, false)]
		[TestCase(0, false, false)]
		[TestCase(2, true, false)]
		[TestCase(2, false, true)]
		[TestCase(3, true, true)]
		[TestCase(3, false, true)]
		public void PathFromIndexBounds(int pIndex, bool pInclusive, bool pThrows) {
			TestPath p = GetTestPathLength3();

			WeaverTestUtils.CheckThrows<WeaverPathException>(pThrows,
				() => p.PathFromIndex(pIndex, pInclusive)
			);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void IndexOfItem() {
			var p = new TestPath();
			var n2 = p.BaseNode.OutHasPerson.ToNode;
			var n4 = n2.OutLikesCandy.ToNode;

			Assert.AreEqual(5, p.Length, "Incorrect Path.Length.");
			Assert.AreEqual(2, p.IndexOfItem(n2), "Incorrect item index.");
			Assert.AreEqual(4, p.IndexOfItem(n4), "Incorrect item index.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void FindAsNode() {
			Person perAlias, perAlias2;

			var p = new TestPath();
			var n2 = p.BaseNode.OutHasPerson.ToNode;
			var n2Again = n2.As(out perAlias);
			var n5 = n2Again.OutLikesCandy.FromNode.As(out perAlias2);
			
			var as3 = (WeaverFuncAs<Person>)p.ItemAtIndex(3);
			Person result = p.FindAsNode<Person>(as3.Label);

			Assert.AreEqual(n2, result, "Incorrect Result.");
			Assert.AreEqual(n2Again, result, "Incorrect Result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void FindAsNodeNone() {
			var p = GetTestPathLength3();
			Person result = p.FindAsNode<Person>("test");
			Assert.Null(result, "Result should be null.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void FindAsNodeWrongType() {
			var p = new TestPath();
			var n = p.BaseNode.OutHasPerson.ToNode;

			var as3 = new WeaverFuncAs<Candy>(p);
			p.AddItem(as3);

			WeaverTestUtils.CheckThrows<WeaverPathException>(true,
				() => p.FindAsNode<Candy>(as3.Label)
			);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GremlinBase() {
			var path = new TestPath();
			Person personAlias;

			path.BaseNode
				.OutHasPerson.ToNode
				.InRootHas.FromNode
				.OutHasPerson.ToNode
					.As(out personAlias)
				.OutKnowsPerson.ToNode
					.Has(p => p.PersonId, WeaverFuncHasOp.LessThanOrEqualTo, 5)
				.InPersonKnows.FromNode
					.Back(personAlias)
				.OutLikesCandy
					.Has(h => h.Enjoyment, WeaverFuncHasOp.GreterThanOrEqualTo, 0.2)
					.ToNode
				.InPersonLikes.FromNode
					.Prop(p => p.Name);

			const string expect = "g.v(0)"+
				".outE('RootHasPerson').inV"+
				".inE('RootHasPerson')[0].outV(0)"+
				".outE('RootHasPerson').inV"+
					".as('step6')"+
				".outE('PersonKnowsPerson').inV"+
					".has('PersonId', Tokens.T.lte, 5)"+
				".inE('PersonKnowsPerson').outV"+
					".back('step6')"+
				".outE('PersonLikesCandy')"+
					".has('Enjoyment', Tokens.T.gte, 0.2)"+
					".inV"+
				".inE('PersonLikesCandy').outV"+
					".Name";

			Assert.AreEqual(expect, path.Script, "Incorrect GrelminCode.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GremlinIndex() {
			var path = new WeaverPath<Person>();
			path.StartWithIndex<Person>("Person", p => p.PersonId, 123);

			path.BaseNode
				.OutKnowsPerson.ToNode
				.OutLikesCandy.ToNode
					.Prop(p => p.Name);

			const string expect = "g.idx(Person).get('PersonId', 123)"+
				".outE('PersonKnowsPerson').inV"+
				".outE('PersonLikesCandy').inV"+
					".Name";

			Assert.AreEqual(expect, path.Script, "Incorrect GrelminCode.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private TestPath GetTestPathLength3() {
			var p = new TestPath();
			var n = p.BaseNode.OutHasPerson.ToNode;
			Assert.AreEqual(3, p.Length, "Incorrect Length.");
			return p;
		}

		/*--------------------------------------------------------------------------------------------*/
		private TestPath GetTestPathLength5() {
			var p = new TestPath();
			var n = p.BaseNode.OutHasPerson.ToNode.OutLikesCandy.ToNode;
			Assert.AreEqual(5, p.Length, "Incorrect Length.");
			return p;
		}

	}

}