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

			const string expect = "g.idx('Person').get('PersonId', 123)"+
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

	}

}