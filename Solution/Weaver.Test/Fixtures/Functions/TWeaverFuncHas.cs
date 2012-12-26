﻿using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncHas : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(WeaverFuncHasOp.EqualTo, "test", "eq, 'test'")]
		[TestCase(WeaverFuncHasOp.NotEqualTo, null, "neq, null")]
		[TestCase(WeaverFuncHasOp.GreaterThan, 123u, "gt, 123")]
		[TestCase(WeaverFuncHasOp.GreterThanOrEqualTo, 1.5f, "gte, 1.5")]
		[TestCase(WeaverFuncHasOp.LessThan, 99, "lt, 99")]
		[TestCase(WeaverFuncHasOp.LessThanOrEqualTo, 1.23456789d, "lte, 1.23456789")]
		public void Gremlin(WeaverFuncHasOp pOperation, object pValue, string pExpect) {
			pExpect = "has('ExpectOneNode', Tokens.T."+pExpect+")";

			var f = new WeaverFuncHas<Person>(n => n.ExpectOneNode, pOperation, pValue);

			Assert.AreEqual("ExpectOneNode", f.PropertyName, "Incorrect PropertyName.");
			Assert.AreEqual(pOperation, f.Operation, "Incorrect Operation.");
			Assert.AreEqual(pValue, f.Value, "Incorrect Value.");
			Assert.AreEqual(pExpect, f.Script, "Incorrect GremlinCode.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BadExpression() {
			WeaverTestUtils.CheckThrows<WeaverFuncException>(true, () => {
				var f = new WeaverFuncHas<Person>(n => (n.ExpectOneNode == false),
					WeaverFuncHasOp.EqualTo, null);
				var p = f.PropertyName;
			});
		}

	}

}