﻿using Moq;
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
		[TestCase(1)]
		[TestCase(22)]
		[TestCase(333)]
		public void Gremlin(int pItemIndex) {
			Person perAlias = new Person();

			var pathMock = new Mock<IWeaverPath>();
			pathMock.Setup(x => x.IndexOfItem(It.IsAny<IWeaverItem>())).Returns(pItemIndex);

			var f = new WeaverFuncBack<Person>(pathMock.Object, perAlias);

			Assert.AreEqual("step"+pItemIndex, f.Label, "Incorrect Label.");
			Assert.AreEqual("back('step"+pItemIndex+"')", f.Script, "Incorrect GremlinCode.");
			Assert.AreEqual(perAlias, f.BackToItem, "Incorrect BackToItem.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(-1)]
		[TestCase(-22)]
		public void NotInPath(int pItemIndex) {
			Person perAlias = new Person();
			WeaverFuncBack<Person> f = null;

			var pathMock = new Mock<IWeaverPath>();
			pathMock.Setup(x => x.IndexOfItem(It.IsAny<IWeaverItem>())).Returns(pItemIndex);

			WeaverTestUtils.CheckThrows<WeaverFuncException>(true, () => {
				f = new WeaverFuncBack<Person>(pathMock.Object, perAlias);
			});
		}

	}

}