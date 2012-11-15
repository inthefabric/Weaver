﻿using NUnit.Framework;
using Weaver.Functions;
using Weaver.Test.Common.Nodes;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncBack {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase("a", "back('a')")]
		[TestCase("reallyLongLabelText", "back('reallyLongLabelText')")]
		public void Gremlin(string pLabel, string pExpectGremlin) {
			var f = new WeaverFuncBack<Person>(new Person(), pLabel);

			Assert.AreEqual(pLabel, f.Label, "Incorrect Label.");
			Assert.AreEqual(pExpectGremlin, f.GremlinCode, "Incorrect GremlinCode.");
		}

	}

}