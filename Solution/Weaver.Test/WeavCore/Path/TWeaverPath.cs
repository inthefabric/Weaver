using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weaver.Core;
using Weaver.Core.Exceptions;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Test.Common.Edges;
using Weaver.Test.Common.Vertices;
using Weaver.Test.Utils;

namespace Weaver.Test.WeavCore.Path {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverPath : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			var mockConfig = new Mock<IWeaverConfig>();
			var mockQuery = new Mock<IWeaverQuery>();

			var p = new WeaverPath(mockConfig.Object, mockQuery.Object);

			Assert.AreEqual(mockConfig.Object, p.Config, "Incorrect Config.");
			Assert.AreEqual(mockQuery.Object, p.Query, "Incorrect Query.");
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddItem() {
			IWeaverPath p = NewPath();

			var candy = new Candy();
			p.AddItem(candy);

			Assert.AreEqual(1, p.Length, "Incorrect Length.");
			Assert.AreEqual(candy, p.ItemAtIndex(0), "Incorrect item at index 0.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddItemToEndedPath() {
			IWeaverPath p = NewPath();
			p.AddItem(new PathEnder());
			WeaverTestUtil.CheckThrows<WeaverPathException>(true, () => p.AddItem(new Candy()));
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedScript() {
			var i0 = new WeaverStepCustom("first()");
			var i1 = new WeaverStepCustom("second");
			var i2 = new WeaverStepCustom("[0..10]", true);
			var i3 = new PathEnder();

			IWeaverPath p = NewPath();
			p.AddItem(i0);
			p.AddItem(i1);
			p.AddItem(i2);
			p.AddItem(i3);

			const string expect = "first().second[0..10].ender";
			Assert.AreEqual(expect, p.BuildParameterizedScript(), "Incorrect result.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(0)]
		[TestCase(1)]
		[TestCase(3)]
		public void Length(int pLen) {
			IWeaverPath p = NewPath(pLen);
			Assert.AreEqual(pLen, p.Length, "Incorrect Length.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ItemAtIndex() {
			var i0 = new Root();
			var i1 = new Person();
			var i2 = new PersonLikesCandy();
			var i3 = new Candy();

			IWeaverPath p = NewPath();
			p.AddItem(i0);
			p.AddItem(i1);
			p.AddItem(i2);
			p.AddItem(i3);

			Assert.AreEqual(4, p.Length, "Incorrect Length.");
			Assert.AreEqual(i0, p.ItemAtIndex(0), "Incorrect item at index 0.");
			Assert.AreEqual(i1, p.ItemAtIndex(1), "Incorrect item at index 1.");
			Assert.AreEqual(i2, p.ItemAtIndex(2), "Incorrect item at index 2.");
			Assert.AreEqual(i3, p.ItemAtIndex(3), "Incorrect item at index 3.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(-1)]
		[TestCase(3)]
		public void ItemAtIndexBounds(int pIndex) {
			IWeaverPath p = NewPath(3);

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
			IWeaverPath p = NewPath(5);

			IList<IWeaverPathItem> result = p.PathToIndex(pIndex, pInclusive);

			Assert.NotNull(result, "Result should be filled.");
			Assert.AreEqual(pCount, result.Count, "Incorrect Result.Count.");
			Assert.AreEqual(p.ItemAtIndex(0), result[0], "Incorrect Result[0].");

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
			IWeaverPath p = NewPath(3);

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
			IWeaverPath p = NewPath(5);

			IList<IWeaverPathItem> result = p.PathFromIndex(pIndex, pInclusive);

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
			IWeaverPath p = NewPath(3);

			WeaverTestUtil.CheckThrows<WeaverPathException>(pThrows,
				() => p.PathFromIndex(pIndex, pInclusive)
			);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void IndexOfItem() {
			var i0 = new Root();
			var i1 = new Person();
			var i2 = new PersonLikesCandy();
			var i3 = new Candy();

			IWeaverPath p = NewPath();
			p.AddItem(i0);
			p.AddItem(i1);
			p.AddItem(i2);
			p.AddItem(i3);

			Assert.AreEqual(4, p.Length, "Incorrect Path.Length.");
			Assert.AreEqual(0, p.IndexOfItem(i0), "Incorrect item index at 0.");
			Assert.AreEqual(1, p.IndexOfItem(i1), "Incorrect item index at 1.");
			Assert.AreEqual(2, p.IndexOfItem(i2), "Incorrect item index at 2.");
			Assert.AreEqual(3, p.IndexOfItem(i3), "Incorrect item index at 3.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private IWeaverPath NewPath(int pItemsToAdd=0) {
			var p = new WeaverPath(WeavInst.Config, new WeaverQuery());

			for ( int i = 0 ; i < pItemsToAdd ; ++i ) {
				p.AddItem(new Candy());
			}

			return p;
		}

	}


	/*================================================================================================*/
	public class PathEnder : WeaverPathItem, IWeaverPathEnder {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return "ender";
		}

	}
	
}