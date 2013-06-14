using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weaver.Core.Exceptions;
using Weaver.Core.Query;
using Weaver.Test.Common.Vertices;
using Weaver.Test.Utils;

namespace Weaver.Test.WeavCore.Query {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverQuery : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			var q = new WeaverQuery();

			Assert.False(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.Null(q.Script, "Script should be null.");
			Assert.NotNull(q.Params, "Params should not be null.");
			Assert.Null(q.ResultVar, "ResultVar should be null.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void FinalizeQuery() {
			const string script = "finalize.this.query.please";

			var q = new WeaverQuery();
			q.FinalizeQuery(script);

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.AreEqual(script+";", q.Script, "Incorrect Script.");
			Assert.NotNull(q.Params, "Params should not be null.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void FinalizeQueryTwiceFail() {
			var q = new WeaverQuery();
			q.FinalizeQuery("1");
			WeaverTestUtil.CheckThrows<WeaverException>(true, () => q.FinalizeQuery("2"));
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddStringParam() {
			const string key = "_P0";
			const string val = "ValueA";

			var q = new WeaverQuery();
			string result = q.AddStringParam(val);

			Assert.AreEqual(key, result, "Incorrect result.");
			Assert.AreEqual(1, q.Params.Keys.Count, "Incorrect Params count.");
			Assert.True(q.Params.ContainsKey(key), "Params should contain this key.");
			Assert.AreEqual(val, q.Params[key].Original, "Incorrect Params.Original.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddParamIntVal() {
			const string key = "_P0";
			const int val = 235632;

			var mockVal = new Mock<IWeaverQueryVal>();
			mockVal.SetupGet(x => x.Original).Returns(val);

			var q = new WeaverQuery();
			string result = q.AddParam(mockVal.Object);

			Assert.AreEqual(key, result, "Incorrect result.");
			Assert.AreEqual(1, q.Params.Keys.Count, "Incorrect Params count.");
			Assert.True(q.Params.ContainsKey(key), "Params should contain this key.");
			Assert.AreEqual(val, q.Params[key].Original, "Incorrect Params value for this key.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddParamName() {
			var q = new WeaverQuery();

			for ( int i = 0 ; i <= 101 ; ++i ) {
				string name = q.AddStringParam("fake");
				Assert.AreEqual("_P"+i, name, "Incorrect result at count "+i+".");
				
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true)]
		[TestCase(false)]
		public void StoreResultAsVar(bool pGeneric) {
			const string name = "xyz";
			const string script = "this.is.the.query";
			const string varScript = name+"="+script;

			var q = new WeaverQuery();
			q.AddParam(new WeaverQueryVal("test"));
			q.FinalizeQuery(script);

			IWeaverQuery q2;

			if ( pGeneric ) {
				IWeaverVarAlias<Person> alias;
				q2 = WeaverQuery.StoreResultAsVar(name, q, out alias);
				Assert.AreEqual(alias, q2.ResultVar, "Incorrect ResultVar.");
			}
			else {
				IWeaverVarAlias alias;
				q2 = WeaverQuery.StoreResultAsVar(name, q, out alias);
				Assert.AreEqual(alias, q2.ResultVar, "Incorrect ResultVar.");
			}

			Assert.AreEqual(varScript+";", q2.Script, "Incorrect Script.");
			Assert.AreEqual(q.Params, q2.Params, "Incorrect Params.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void StoreResultAsVarT() {
			const string name = "xyz";
			const string script = "this.is.the.query";
			const string varScript = name+"="+script;

			var q = new WeaverQuery();
			q.AddParam(new WeaverQueryVal("test"));
			q.FinalizeQuery(script);

			IWeaverVarAlias<Person> alias;
			IWeaverQuery q2 = WeaverQuery.StoreResultAsVar(name, q, out alias);

			Assert.AreEqual(alias, q2.ResultVar, "Incorrect ResultVar.");
			Assert.AreEqual(varScript+";", q2.Script, "Incorrect Script.");
			Assert.AreEqual(q.Params, q2.Params, "Incorrect Params.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void StoreResultAsVarNotFinalized() {
			var q = new WeaverQuery();
			IWeaverVarAlias alias;

			WeaverTestUtil.CheckThrows<WeaverException>(
				true, () => WeaverQuery.StoreResultAsVar("x", q, out alias)
			);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void StoreResultAsVarTwice() {
			var q = new WeaverQuery();
			q.FinalizeQuery("script");

			IWeaverVarAlias alias;
			IWeaverQuery q2 = WeaverQuery.StoreResultAsVar("x", q, out alias);

			WeaverTestUtil.CheckThrows<WeaverException>(
				true, () => WeaverQuery.StoreResultAsVar("x", q2, out alias)
			);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(null, "")]
		[TestCase(0, "")]
		[TestCase(1, "x0")]
		[TestCase(2, "x0,x1")]
		[TestCase(10, "x0,x1,x2,x3,x4,x5,x6,x7,x8,x9")]
		public void InitListVar(int? pItems, string pExpectList) {
			const string name = "_var0";
			IWeaverVarAlias setList;
			IWeaverQuery q;

			if ( pItems == null ) {
				q = WeaverQuery.InitListVar(name, out setList);
			}
			else {
				var list = new List<IWeaverVarAlias>();

				for ( int i = 0 ; i < pItems ; ++i ) {
					var mockVar = new Mock<IWeaverVarAlias>();
					mockVar.SetupGet(x => x.Name).Returns("x"+i);
					list.Add(mockVar.Object);
				}

				q = WeaverQuery.InitListVar(name, list, out setList);
			}

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.AreEqual(name+"=["+pExpectList+"];", q.Script, "Incorrect Script.");
			Assert.NotNull(setList, "The out WeaverListVar should not be null.");
		}

	}

}