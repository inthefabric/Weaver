using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Query;
using Weaver.Core.Util;
using Weaver.Test.Common.Edges;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;
using Weaver.Test.Utils;

namespace Weaver.Test.WeavCore.Util {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverUtil : WeaverTestBase {

		private Expression<Func<Person, object>> vPropExpr;
		private string vPropExprResult;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(typeof(Person), true)]
		[TestCase(typeof(WeaverUtil), false)]
		public void GetElementAttribute(Type pType, bool pFound) {
			var result = WeaverUtil.GetElementAttribute<WeaverVertexAttribute>(pType);

			if ( pFound ) {
				Assert.NotNull(result, "Result should be filled.");
			}
			else {
				Assert.Null(result, "Result should be null.");
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(typeof(Person), 6)] //4+name+id
		[TestCase(typeof(Root), 2)] //0+name+id
		[TestCase(typeof(WeaverUtil), 0)]
		public void GetElementPropertyAttributes(Type pType, int pCount) {
			IList<WeaverPropPair> result = WeaverUtil.GetElementPropertyAttributes(pType);

			Assert.NotNull(result, "Result should be filled.");
			Assert.AreEqual(pCount, result.Count, "Incorrect result count.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetPropertyAttribute() {
			WeaverPropPair result = WeaverUtil.GetPropertyAttribute<Person>(x => x.Age);

			Assert.NotNull(result, "Result should be filled.");
			Assert.AreEqual("Age", result.Info.Name, "Incorrect result PropertyInfo.Name.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true, "Zach")]
		[TestCase(false, "Zach")]
		[TestCase(true, null)]
		public void BuildPropListPerson(bool pIncludeId, string pName) {
			var p = new Person();
			p.Id = "123456789123ABC";
			p.PersonId = 3456789;
			p.Name = pName;
			p.Age = 27.3f;
			p.IsMale = true;

			var q = new WeaverQuery();
			string propList = WeaverUtil.BuildPropList(WeavInst.Config, q, p, pIncludeId);
			Dictionary<string, string> pairMap = WeaverTestUtil.GetPropListDictionary(propList);

			int expectCount = 3 + (pIncludeId ? 1 : 0) + (pName != null ? 1 : 0);
			Assert.AreEqual(expectCount, pairMap.Keys.Count, "Incorrect Key count.");

			Assert.True(pairMap.ContainsKey(TestSchema.Person_PersonId), "Missing PersonId key.");
			Assert.True(pairMap.ContainsKey(TestSchema.Person_Age), "Missing Age key.");
			Assert.True(pairMap.ContainsKey(TestSchema.Person_IsMale), "Missing IsMale key.");
			Assert.AreEqual(pIncludeId, pairMap.ContainsKey("id"), "Incorrect Id key.");
			Assert.AreEqual((pName != null), pairMap.ContainsKey("Name"), "Incorrect Name key.");

			Assert.AreEqual("_P0", pairMap[TestSchema.Person_PersonId], "Incorrect PersonId value.");
			Assert.AreEqual("_P1", pairMap[TestSchema.Person_IsMale], "Incorrect IsMale value.");
			Assert.AreEqual("_P2", pairMap[TestSchema.Person_Age], "Incorrect Age value.");

			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal(p.PersonId));
			expectParams.Add("_P1", new WeaverQueryVal(p.IsMale));
			expectParams.Add("_P2", new WeaverQueryVal(p.Age));

			int pi = 3;

			if ( pName != null ) {
				Assert.AreEqual("_P3", pairMap["Name"], "Incorrect Name value.");
				expectParams.Add("_P3", new WeaverQueryVal(pName));
				pi++;
			}

			if ( pIncludeId ) {
				Assert.AreEqual("_P"+pi, pairMap["id"], "Incorrect Id value.");
				expectParams.Add("_P"+pi, new WeaverQueryVal(p.Id));
			}

			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNamePassLong() {
			vPropExpr = (p => p.PersonId);
			TryPropExpr();
			Assert.AreEqual(TestSchema.Person_PersonId, vPropExprResult, "Incorrect property name.");
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
		public void PropNamePassLowerId() {
			vPropExpr = (p => p.Id);
			TryPropExpr();
			Assert.AreEqual("id", vPropExprResult, "Incorrect property name.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNamePassLowerLabel() {
			Expression<Func<PersonLikesCandy, object>> expr = (p => p.Label);
			vPropExprResult = WeaverUtil.GetPropertyDbName(expr);

			Assert.AreEqual("label", vPropExprResult, "Incorrect property name.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailGt() {
			vPropExpr = (p => p.PersonId > 123);
			WeaverTestUtil.CheckThrows<WeaverException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailAdd() {
			vPropExpr = (p => p.PersonId + 123);
			WeaverTestUtil.CheckThrows<WeaverException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailSub() {
			vPropExpr = (p => 987 - p.PersonId);
			WeaverTestUtil.CheckThrows<WeaverException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailEq() {
			vPropExpr = (p => "asdf" == p.Name);
			WeaverTestUtil.CheckThrows<WeaverException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailBoolEq() {
			vPropExpr = (p => (p.ExpectOneVertex == false));
			WeaverTestUtil.CheckThrows<WeaverException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailConcat() {
			vPropExpr = (p => p.Name+p.Name);
			WeaverTestUtil.CheckThrows<WeaverException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropNameFailUnknown() {
			vPropExpr = (p => p.InPersonKnows);
			WeaverTestUtil.CheckThrows<WeaverException>(true, TryPropExpr);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void TryPropExpr() {
			vPropExprResult = WeaverUtil.GetPropertyDbName(vPropExpr);
		}

	}

}