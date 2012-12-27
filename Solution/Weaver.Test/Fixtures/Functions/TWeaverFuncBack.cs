using Moq;
using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncBack : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			var perAlias = new Person();
			const int itemI = 99;

			var pathMock = new Mock<IWeaverPath>();
			pathMock.Setup(x => x.IndexOfItem(It.IsAny<IWeaverItem>())).Returns(itemI);

			var f = new WeaverFuncBack<Person>(pathMock.Object, perAlias);

			Assert.AreEqual("step"+itemI, f.Label, "Incorrect Label.");
			Assert.AreEqual(perAlias, f.BackToItem, "Incorrect BackToItem.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString() {
			var perAlias = new Person();
			const int itemI = 99;

			var pathMock = new Mock<IWeaverPath>();
			pathMock.Setup(x => x.IndexOfItem(It.IsAny<IWeaverItem>())).Returns(itemI);

			var f = new WeaverFuncBack<Person>(pathMock.Object, perAlias);

			Assert.AreEqual("back('step"+itemI+"')", f.BuildParameterizedString(), "Incorrect result.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(-1)]
		[TestCase(-22)]
		public void NotInPath(int pItemIndex) {
			var perAlias = new Person();

			var pathMock = new Mock<IWeaverPath>();
			pathMock.Setup(x => x.IndexOfItem(It.IsAny<IWeaverItem>())).Returns(pItemIndex);

			WeaverTestUtil.CheckThrows<WeaverFuncException>(true, () => {
				var f = new WeaverFuncBack<Person>(pathMock.Object, perAlias);
			});
		}

	}

}