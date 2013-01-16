using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;
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
		public void New() {
			IWeaverQuery q = new Mock<WeaverQuery>().Object;
			var p = new WeaverPath<Root>(q);

			Assert.Null(p.BaseNode, "BaseNode should be null.");
			Assert.AreEqual(q, p.Query, "Incorrect Query.");
			Assert.AreEqual(0, p.Length, "Incorrect Length.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewRoot() {
			IWeaverQuery q = new Mock<WeaverQuery>().Object;
			var r = new Root();
			var p = new WeaverPath<Root>(q, r);

			Assert.NotNull(p.BaseNode, "BaseNode should be filled.");
			Assert.AreEqual(p, p.BaseNode.Path, "Incorrect BaseNode.Path.");
			Assert.AreEqual(q, p.Query, "Incorrect Query.");
			Assert.AreEqual(1, p.Length, "Incorrect Length.");
			Assert.AreEqual(r, p.ItemAtIndex(0), "Incorrect item at index 0.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewVarAlias() {
			IWeaverQuery q = new Mock<WeaverQuery>().Object;
			const string varName = "_V0";

			var mockVar = new Mock<IWeaverVarAlias<Person>>();
			mockVar.SetupGet(x => x.Name).Returns(varName);

			var p = new WeaverPathFromVarAlias<Person>(q, mockVar.Object);

			Assert.NotNull(p.BaseNode, "BaseNode should be filled.");
			Assert.AreEqual(p, p.BaseNode.Path, "Incorrect BaseNode.Path.");
			
			Assert.AreEqual(q, p.Query, "Incorrect Query.");
			Assert.AreEqual(mockVar.Object, p.BaseVar, "Incorrect BaseVar.");

			Assert.AreEqual(0, p.Length, "Incorrect Length.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewManualIndex() {
			const int perId = 99;
			const string indexName = "Person";

			IWeaverQuery q = new Mock<WeaverQuery>().Object;
			var p = new WeaverPathFromManualIndex<Person>(q, "Person", x => x.PersonId, perId);

			Assert.NotNull(p.BaseNode, "BaseNode should be filled.");
			Assert.AreEqual(p, p.BaseNode.Path, "Incorrect BaseNode.Path.");
			Assert.NotNull(p.BaseIndex, "BaseIndex should be filled.");

			Assert.AreEqual(indexName, p.BaseIndex.IndexName, "Incorrect BaseIndex.IndexName.");
			Assert.AreEqual(indexName+"Id", p.BaseIndex.PropertyName,
				"Incorrect BaseIndex.PropertyName.");
			Assert.AreEqual(perId, p.BaseIndex.Value, "Incorrect BaseIndex.Value.");

			Assert.AreEqual(q, p.Query, "Incorrect Query.");

			Assert.AreEqual(1, p.Length, "Incorrect Length.");
			Assert.AreEqual(p.BaseIndex, p.ItemAtIndex(0), "Incorrect item at index 0.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewKeyIndex() {
			const int perId = 99;

			IWeaverQuery q = new Mock<WeaverQuery>().Object;
			var p = new WeaverPathFromKeyIndex<Person>(q, x => x.PersonId, perId);

			Assert.NotNull(p.BaseNode, "BaseNode should be filled.");
			Assert.AreEqual(p, p.BaseNode.Path, "Incorrect BaseNode.Path.");
			Assert.NotNull(p.BaseIndex, "BaseIndex should be filled.");

			Assert.AreEqual("PersonId", p.BaseIndex.IndexName, "Incorrect BaseIndex.IndexName.");
			Assert.AreEqual(perId, p.BaseIndex.Value, "Incorrect BaseIndex.Value.");

			Assert.AreEqual(q, p.Query, "Incorrect Query.");

			Assert.AreEqual(1, p.Length, "Incorrect Length.");
			Assert.AreEqual(p.BaseIndex, p.ItemAtIndex(0), "Incorrect item at index 0.");
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddItem() {
			IWeaverPath<Root> p = GetPathWithRootNode();

			var candy = new Candy();
			p.AddItem(candy);

			const int len = 2;
			Assert.AreEqual(len, p.Length, "Incorrect Length.");
			Assert.AreEqual(candy, p.ItemAtIndex(len-1), "Incorrect item at index "+(len-1)+".");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParamScriptBase() {
			IWeaverFuncAs<Person> personAlias;
			IWeaverQuery q = new Mock<WeaverQuery>().Object;
			IWeaverPath<Root> path = GetPathWithRootNode(q);

			var lastItem = path.BaseNode
				.OutHasPerson.ToNode
				.InRootHas.FromNode
					.Limit(0, 6)
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
				".inE('RootHasPerson').outV[0..6]"+
				".outE('RootHasPerson').inV"+
					".as('step8')"+
				".outE('PersonKnowsPerson').inV"+
					".has('PersonId',Tokens.T.lte,5)"+
				".inE('PersonKnowsPerson').outV"+
					".back('step8')"+
				".outE('PersonLikesCandy')"+
					".has('Enjoyment',Tokens.T.gte,0.2D)"+
					".inV"+
				".inE('PersonLikesCandy').outV"+
					".property('Name')";
			
			Assert.AreEqual(expect, path.BuildParameterizedScript(), "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParamScriptIndex() {
			IWeaverQuery q = new Mock<WeaverQuery>().Object;
			var path = new WeaverPathFromManualIndex<Person>(q, "Person", p => p.PersonId, 123);

			var lastItem = path.BaseNode
				.OutKnowsPerson.ToNode
				.OutLikesCandy.ToNode
				.Prop(p => p.Name);

			const string expect = "g.idx(_P0).get('PersonId',123)[0]"+
				".outE('PersonKnowsPerson').inV"+
				".outE('PersonLikesCandy').inV"+
					".property('Name')";

			Assert.AreEqual(expect, path.BuildParameterizedScript(), "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParamScriptVarAlias() {
			IWeaverQuery q = new Mock<WeaverQuery>().Object;
			const string varName = "_V0";

			var mockVar = new Mock<IWeaverVarAlias<Person>>();
			mockVar.SetupGet(x => x.Name).Returns(varName);

			var path = new WeaverPathFromVarAlias<Person>(q, mockVar.Object);

			var lastItem = path.BaseNode
				.OutLikesCandy.ToNode
					.Has(h => h.IsChocolate, WeaverFuncHasOp.EqualTo, false);

			const string expect = varName+
				".outE('PersonLikesCandy').inV"+
					".has('IsChocolate',Tokens.T.eq,false)";

			Assert.AreEqual(expect, path.BuildParameterizedScript(), "Incorrect result.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Length1() {
			IWeaverPath<Root> p = GetPathWithRootNode();
			Assert.AreEqual(1, p.Length, "Incorrect Length.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Length3() {
			IWeaverPath<Root> p = GetPathWithRootNode();
			p.AddItem(new Root());
			p.AddItem(new Root());
			Assert.AreEqual(3, p.Length, "Incorrect Length.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ItemAtIndex() {
			var r = new Root();
			IWeaverPath<Root> p = GetPathWithRootNode(null, r);
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
			IWeaverPath<Root> p = GetTestPathLength3();

			WeaverTestUtil.CheckThrows<WeaverPathException>(true,
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
			IWeaverPath<Root> p = GetTestPathLength5();

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
			IWeaverPath<Root> p = GetTestPathLength3();

			WeaverTestUtil.CheckThrows<WeaverPathException>(pThrows,
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
			IWeaverPath<Root> p = GetTestPathLength5();

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
			IWeaverPath<Root> p = GetTestPathLength3();

			WeaverTestUtil.CheckThrows<WeaverPathException>(pThrows,
				() => p.PathFromIndex(pIndex, pInclusive)
			);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void IndexOfItem() {
			IWeaverPath<Root> p = GetPathWithRootNode();
			var n2 = p.BaseNode.OutHasPerson.ToNode;
			var n4 = n2.OutLikesCandy.ToNode;

			Assert.AreEqual(5, p.Length, "Incorrect Path.Length.");
			Assert.AreEqual(2, p.IndexOfItem(n2), "Incorrect item index.");
			Assert.AreEqual(4, p.IndexOfItem(n4), "Incorrect item index.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private IWeaverPath<Root> GetPathWithRootNode(IWeaverQuery pQuery=null, Root pRoot=null) {
			var mockQ = new Mock<IWeaverQuery>();
			IWeaverQuery q = (pQuery ?? mockQ.Object);
			Root r = (pRoot ?? new Root());
			return new WeaverPath<Root>(q, r);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private IWeaverPath<Root> GetTestPathLength3() {
			IWeaverPath<Root> p = GetPathWithRootNode();
			var n = p.BaseNode.OutHasPerson.ToNode;
			Assert.AreEqual(3, p.Length, "Incorrect Length.");
			return p;
		}

		/*--------------------------------------------------------------------------------------------*/
		private IWeaverPath<Root> GetTestPathLength5() {
			IWeaverPath<Root> p = GetPathWithRootNode();
			var n = p.BaseNode.OutHasPerson.ToNode.OutLikesCandy.ToNode;
			Assert.AreEqual(5, p.Length, "Incorrect Length.");
			return p;
		}

	}
	
}