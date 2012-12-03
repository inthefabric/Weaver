using NUnit.Framework;
using Weaver.Functions;
using Weaver.Items;
using Weaver.Test.Common;
using Weaver.Test.Common.Nodes;

namespace Weaver.Test.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverPath {


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

			Assert.AreEqual(expect, path.GremlinCode, "Incorrect GrelminCode.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GremlinIndex() {
			var path = new WeaverPath<Person>();
			path.StartAtIndex<Person>("Person", p => p.PersonId, 123);

			path.BaseNode
				.OutKnowsPerson.ToNode
				.OutLikesCandy.ToNode
					.Prop(p => p.Name);

			const string expect = "g.idx(Person).get('PersonId', 123)"+
				".outE('PersonKnowsPerson').inV"+
				".outE('PersonLikesCandy').inV"+
					".Name";

			Assert.AreEqual(expect, path.GremlinCode, "Incorrect GrelminCode.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		//TODO: Length
		//TODO: ItemAtIndex(int pIndex)
		//TODO: PathToIndex(int pIndex, bool pInclusive=true)
		//TODO: PathFromIndex(int pIndex, bool pInclusive=true)
		//TODO: IndexOfItem(IWeaverItem pItem)
		//TODO: FindAsNode<TItem>(string pLabel)


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		[Test]
		public void PrevQueryItem() {
			var q = new TestQuery();
			var n = q.Root.OutHasThing.ToThing;
			var afterN = n.OutHasArtifact;
			Assert.AreEqual(n, afterN.PrevQueryItem, "Incorrect PrevQueryItem.");
		}

		/*--------------------------------------------------------------------------------------------* /
		[Test]
		public void NextQueryItem() {
			var q = new TestQuery();
			var n = q.Root.OutHasThing.ToThing;
			var afterN = n.OutHasArtifact;
			Assert.AreEqual(afterN, n.NextQueryItem, "Incorrect NextQueryItem.");
		}

		/*--------------------------------------------------------------------------------------------* /
		[Test]
		public void QueryPathToThisItem() {
			var q = new TestQuery();
			var n = q.Root.OutHasThing.ToThing;
			int pathCountAtN = q.PathLength();
			var afterN = n.OutHasArtifact.ToArtifact;
			var listToN = n.QueryPathToThisItem;

			Assert.AreEqual(pathCountAtN, listToN.Count, "Incorrect QueryPathToThisItem.Count.");

			for ( int i = 0 ; i < pathCountAtN ; ++i ) {
				Assert.AreEqual(listToN[i], n.QueryPathToThisItem[i],
					"Incorrect QueryPathToThisItem["+i+"].");
			}
		}

		/*--------------------------------------------------------------------------------------------* /
		[Test]
		public void QueryPathFromThisItem() {
			var q = new TestQuery();
			var n = q.Root.OutHasThing.ToThing;
			int pathCountAtN = q.PathLength();
			var afterN = n.OutHasArtifact.ToArtifact;
			var listFromN = n.QueryPathFromThisItem;
			int pathCountAtAfterN = q.PathLength();
			int expectCount = pathCountAtAfterN-pathCountAtN+1;

			Assert.AreEqual(expectCount, listFromN.Count, "Incorrect QueryPathFromThisItem.Count.");

			for ( int i = 0 ; i < expectCount ; ++i ) {
				Assert.AreEqual(listFromN[i], n.QueryPathFromThisItem[i],
					"Incorrect QueryPathFromThisItem["+i+"].");
			}
		}*/

	}

}