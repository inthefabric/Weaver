using System;
using Fabric.Domain.Graph.Functions;
using Fabric.Test.Common.Nodes;
using NUnit.Framework;

namespace Fabric.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncHas {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(WeaverFuncHasOp.EqualTo, "test", "eq, 'test'")]
		[TestCase(WeaverFuncHasOp.NotEqualTo, null, "neq, null")]
		[TestCase(WeaverFuncHasOp.GreaterThan, 123u, "gt, 123")]
		[TestCase(WeaverFuncHasOp.GreterThanOrEqualTo, 1.5f, "gte, 1.5")]
		[TestCase(WeaverFuncHasOp.LessThan, 99, "lt, 99")]
		[TestCase(WeaverFuncHasOp.LessThanOrEqualTo, 1.23456789d, "lte, 1.23456789")]
		public void Gremlin(WeaverFuncHasOp pOperation, object pValue, string pExpect) {
			pExpect = "has('ExpectOneNode', T."+pExpect+")";

			var f = new WeaverFuncHas<Person>(
				new Person(), (n => n.ExpectOneNode), pOperation, pValue);

			Assert.AreEqual("ExpectOneNode", f.PropertyName, "Incorrect PropertyName.");
			Assert.AreEqual(pOperation, f.Operation, "Incorrect Operation.");
			Assert.AreEqual(pValue, f.Value, "Incorrect Value.");
			Assert.AreEqual(pExpect, f.GremlinCode, "Incorrect GremlinCode.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GremlinBadExpression() {
			try {
				var f = new WeaverFuncHas<Person>(
					new Person(), (n => (n.ExpectOneNode == false)),
					WeaverFuncHasOp.EqualTo, null);
				Assert.Fail("Expected an Exception: "+f);
			}
			catch ( Exception e ) {
				Assert.NotNull(e);
			}
		}

	}

}