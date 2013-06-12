using NUnit.Framework;
using Weaver.Core.Exceptions;
using Weaver.Test.Core.Common;
using Weaver.Test.Core.Common.Vertices;
using Weaver.Test.Core.Utils;

namespace Weaver.Test.Core.Fixtures.Path {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverPathItem : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Path() {
			var item = new TestElement();
			Assert.AreEqual(item.MockPath.Object, item.Path, "Incorrect Path.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PathIndex() {
			const int expectIndex = 2;

			var item = new TestElement();
			item.MockPath.Setup(x => x.IndexOfItem(item)).Returns(expectIndex);

			Assert.AreEqual(expectIndex, item.PathIndex, "Incorrect PathIndex.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PathIndexFail() {
			var candy = new Candy();

			WeaverTestUtil.CheckThrows<WeaverException>(true, () => {
				var i = candy.PathIndex;
			});
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ItemIdentifier() {
			var candy = new Candy { Id = "a123" };
			Assert.AreEqual("Candy(Id=a123)", candy.ItemIdentifier, "Incorrect ItemIdentifier.");
		}

	}

}