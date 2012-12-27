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
	public class TWeaverFuncHas : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			const WeaverFuncHasOp op = WeaverFuncHasOp.EqualTo;
			const string val = "test";

			var f = new WeaverFuncHas<Person>(n => n.ExpectOneNode, op, val);

			Assert.AreEqual(op, f.Operation, "Incorrect Operation.");
			Assert.AreEqual(val, f.Value, "Incorrect Value.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropertyName() {
			var f = new WeaverFuncHas<Person>(n => n.ExpectOneNode, WeaverFuncHasOp.EqualTo, 1);
			Assert.AreEqual("ExpectOneNode", f.PropertyName, "Incorrect PropertyName.");
			Assert.AreEqual("ExpectOneNode", f.PropertyName, "Incorrect cached PropertyName.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropertyNameInvalid() {
			var f = new WeaverFuncHas<Person>(x => (x.ExpectOneNode == false),
				WeaverFuncHasOp.EqualTo, 1);

			WeaverTestUtils.CheckThrows<WeaverFuncException>(true, () => {
				var p = f.PropertyName;
			});
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(WeaverFuncHasOp.EqualTo, "test", "eq,_P0")]
		[TestCase(WeaverFuncHasOp.NotEqualTo, null, "neq,null")]
		[TestCase(WeaverFuncHasOp.GreaterThan, 123u, "gt,123")]
		[TestCase(WeaverFuncHasOp.GreterThanOrEqualTo, 1.5f, "gte,1.5")]
		[TestCase(WeaverFuncHasOp.LessThan, 99, "lt,99")]
		[TestCase(WeaverFuncHasOp.LessThanOrEqualTo, 1.23456789d, "lte,1.23456789")]
		public void BuildParameterizedString(WeaverFuncHasOp pOperation, object pValue, string pExpect) {
			var val = (pValue is string ? "_P0" : pValue+"");
			if ( pValue == null ) { val = "null"; }

			var mockQuery = new Mock<IWeaverQuery>();
			mockQuery.Setup(x => x.AddParamIfString(It.IsAny<WeaverQueryVal>())).Returns(val);

			var mockPath = new Mock<IWeaverPath>();
			mockPath.SetupGet(x => x.Query).Returns(mockQuery.Object);

			var f = new WeaverFuncHas<Person>(n => n.ExpectOneNode, pOperation, pValue);
			f.Path = mockPath.Object;

			Assert.AreEqual("has('ExpectOneNode',Tokens.T."+pExpect+")",
				f.BuildParameterizedString(), "Incorrect result.");
		}

	}

}