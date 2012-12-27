﻿using Moq;
using NUnit.Framework;
using Weaver.Functions;
using Weaver.Interfaces;
using Weaver.Test.Common.Nodes;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncAs : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			const int pathLen = 99;
			const int itemI = pathLen-1;

			var mockPath = new Mock<IWeaverPath>();
			mockPath.SetupGet(x => x.Length).Returns(pathLen);

			var f = new WeaverFuncAs<Person>(mockPath.Object);

			Assert.AreEqual("step"+itemI, f.Label, "Incorrect Label.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString() {
			const int pathLen = 99;
			const int itemI = pathLen-1;

			var mockPath = new Mock<IWeaverPath>();
			mockPath.SetupGet(x => x.Length).Returns(pathLen);

			var f = new WeaverFuncAs<Person>(mockPath.Object);

			Assert.AreEqual("as('step"+itemI+"')", f.BuildParameterizedString(), "Incorrect result.");
		}

	}

}