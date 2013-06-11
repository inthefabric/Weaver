using Moq;
using NUnit.Framework;
using Weaver.Core.Exceptions;
using Weaver.Core.Query;
using Weaver.Test.Core.Utils;

namespace Weaver.Test.Core.Fixtures.Query {

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

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void StoreResultAsVar() {
			const string name = "_var0";
			const string script = "this.is.the.query";
			const string varScript = name+"="+script;

			var mockVar = new Mock<IWeaverVarAlias>();
			mockVar.SetupGet(x => x.Name).Returns(name);

			var q = new WeaverQuery();
			q.FinalizeQuery(script);
			q.StoreResultAsVar(mockVar.Object);

			Assert.AreEqual(mockVar.Object, q.ResultVar, "Incorrect ResultVar.");
			Assert.AreEqual(varScript+";", q.Script, "Incorrect Script.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void StoreResultAsVarNotFinalized() {
			var q = new WeaverQuery();
			WeaverTestUtil.CheckThrows<WeaverException>(true, () => q.StoreResultAsVar(null));
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void StoreResultAsVarTwice() {
			var mockVar = new Mock<IWeaverVarAlias>();
			mockVar.SetupGet(x => x.Name).Returns("x");

			var q = new WeaverQuery();
			q.FinalizeQuery("script");
			q.StoreResultAsVar(mockVar.Object);

			WeaverTestUtil.CheckThrows<WeaverException>(true, () => q.StoreResultAsVar(null));
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

	}

}