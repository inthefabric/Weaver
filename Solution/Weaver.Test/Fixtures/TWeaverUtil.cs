using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverUtil : WeaverTestBase {

		private Expression<Func<Person, object>> vPropExpr;
		private string vPropExprResult;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true, "Zach")]
		[TestCase(false, "Zach")]
		[TestCase(true, null)]
		public void BuildPropListPerson(bool pIncludeId, string pName) {
			var p = new Person();
			p.Id = 123456789123;
			p.PersonId = 3456789;
			p.Name = pName;
			p.Age = 27.3f;
			p.IsMale = true;

			var q = new WeaverQuery();
			string propList = WeaverUtil.BuildPropList(q, p, pIncludeId);
			Dictionary<string, string> pairMap = WeaverTestUtil.GetPropListDictionary(propList);

			int expectCount = 3 + (pIncludeId ? 1 : 0) + (pName != null ? 1 : 0);
			Assert.AreEqual(expectCount, pairMap.Keys.Count, "Incorrect Key count.");

			Assert.True(pairMap.ContainsKey("PersonId"), "Missing PersonId key.");
			Assert.True(pairMap.ContainsKey("Age"), "Missing Age key.");
			Assert.True(pairMap.ContainsKey("IsMale"), "Missing IsMale key.");
			Assert.AreEqual(pIncludeId, pairMap.ContainsKey("Id"), "Incorrect Id key.");
			Assert.AreEqual((pName != null), pairMap.ContainsKey("Name"), "Incorrect Name key.");

			Assert.AreEqual("3456789", pairMap["PersonId"], "Incorrect PersonId value.");
			Assert.AreEqual("27.3F", pairMap["Age"], "Incorrect Age value.");
			Assert.AreEqual("true", pairMap["IsMale"], "Incorrect IsMale value.");

			if ( pIncludeId ) {
				Assert.AreEqual("123456789123L", pairMap["Id"], "Incorrect Id value.");
			}

			if ( pName != null ) {
				Assert.AreEqual("_P0", pairMap["Name"], "Incorrect Name value.");

				var expectParams = new Dictionary<string, string>();
				expectParams.Add("_P0", pName);
				WeaverTestUtil.CheckQueryParams(q, expectParams);
			}
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
			WeaverTestUtil.CheckThrows<WeaverFuncException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailAdd() {
			vPropExpr = (p => p.PersonId + 123);
			WeaverTestUtil.CheckThrows<WeaverFuncException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailSub() {
			vPropExpr = (p => 987 - p.PersonId);
			WeaverTestUtil.CheckThrows<WeaverFuncException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailEq() {
			vPropExpr = (p => "asdf" == p.Name);
			WeaverTestUtil.CheckThrows<WeaverFuncException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailBoolEq() {
			vPropExpr = (p => (p.ExpectOneNode == false));
			WeaverTestUtil.CheckThrows<WeaverFuncException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailConcat() {
			vPropExpr = (p => p.Name+p.Name);
			WeaverTestUtil.CheckThrows<WeaverFuncException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void TryPropExpr() {
			vPropExprResult = WeaverUtil.GetPropertyName(new WeaverFuncProp<Person>(null), vPropExpr);
		}

	}

}