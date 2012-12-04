using System;
using System.Linq.Expressions;
using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncProp : WeaverTestBase {

		private Expression<Func<Person, object>> vPropExpr;
		private string vPropExprResult;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Gremlin() {
			var f = new WeaverFuncProp<Person>(n => n.PersonId);

			Assert.AreEqual("PersonId", f.PropertyName, "Incorrect PropertyName.");
			Assert.AreEqual("PersonId", f.GremlinCode, "Incorrect GremlinCode.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GremlinBadExpression() {
			WeaverTestUtils.CheckThrows<WeaverFuncException>(true, () => {
				var f = new WeaverFuncProp<Person>(n => (n.ExpectOneNode == false));
				var p = f.PropertyName;
			});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNamePassLong() {
			vPropExpr = (p => p.PersonId);
			TryPropExpr();
			Assert.AreEqual("PersonId", vPropExprResult, "Incorrect property name.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNamePassBool() {
			vPropExpr = (p => p.IsMale);
			TryPropExpr();
			Assert.AreEqual("IsMale", vPropExprResult, "Incorrect property name.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNamePassString() {
			vPropExpr = (p => p.Name);
			TryPropExpr();
			Assert.AreEqual("Name", vPropExprResult, "Incorrect property name.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNamePassFloat() {
			vPropExpr = (p => p.Age);
			TryPropExpr();
			Assert.AreEqual("Age", vPropExprResult, "Incorrect property name.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailGt() {
			vPropExpr = (p => p.PersonId > 123);
			WeaverTestUtils.CheckThrows<WeaverFuncException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailAdd() {
			vPropExpr = (p => p.PersonId + 123);
			WeaverTestUtils.CheckThrows<WeaverFuncException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailSub() {
			vPropExpr = (p => 987 - p.PersonId);
			WeaverTestUtils.CheckThrows<WeaverFuncException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailEq() {
			vPropExpr = (p => "asdf" == p.Name);
			WeaverTestUtils.CheckThrows<WeaverFuncException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailBoolEq() {
			vPropExpr = (p => (p.ExpectOneNode == false));
			WeaverTestUtils.CheckThrows<WeaverFuncException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailConcat() {
			vPropExpr = (p => p.Name+p.Name);
			WeaverTestUtils.CheckThrows<WeaverFuncException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void TryPropExpr() {
			vPropExprResult = WeaverFuncProp.GetPropertyName(new WeaverFuncProp<Person>(null), vPropExpr);
		}

	}

}