using Moq;
using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Interfaces;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures {

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
		public void AddParam() {
			const string key = "ParamA";
			const string val = "ValueA";

			var mockVal = new Mock<IWeaverQueryVal>();
			mockVal.Setup(x => x.GetQuoted()).Returns(val);

			var q = new WeaverQuery();
			q.AddParam(key, mockVal.Object);

			Assert.AreEqual(1, q.Params.Keys.Count, "Incorrect Params count.");
			Assert.True(q.Params.ContainsKey(key), "Params should contain this key.");
			Assert.AreEqual(val, q.Params[key], "Incorrect Params value for this key.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddParamUnnamed() {
			const string key = "_P0";
			const string val = "ValueA";

			var mockVal = new Mock<IWeaverQueryVal>();
			mockVal.Setup(x => x.GetQuoted()).Returns(val);

			var q = new WeaverQuery();
			string result = q.AddParam(mockVal.Object);

			Assert.AreEqual(key, result, "Incorrect result.");
			Assert.AreEqual(1, q.Params.Keys.Count, "Incorrect Params count.");
			Assert.True(q.Params.ContainsKey(key), "Params should contain this key.");
			Assert.AreEqual(val, q.Params[key], "Incorrect Params value for this key.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true)]
		[TestCase(false)]
		public void AddParamIfString(bool pIsStr) {
			const string key = "_P0";
			const string val = "ValueA";
			const string fix = "FixedA";

			var mockVal = new Mock<IWeaverQueryVal>();
			mockVal.Setup(x => x.GetQuoted()).Returns(val);
			mockVal.SetupGet(x => x.FixedText).Returns(fix);
			mockVal.SetupGet(x => x.IsString).Returns(pIsStr);

			var q = new WeaverQuery();
			string result = q.AddParamIfString(mockVal.Object);

			if ( pIsStr ) {
				Assert.AreEqual(key, result, "Incorrect result.");
				Assert.AreEqual(1, q.Params.Keys.Count, "Incorrect Params count.");
				Assert.True(q.Params.ContainsKey(key), "Params should contain this key.");
				Assert.AreEqual(val, q.Params[key], "Incorrect Params value for this key.");
			}
			else {
				Assert.AreEqual(fix, result, "Incorrect result.");
				Assert.AreEqual(0, q.Params.Keys.Count, "Incorrect Params count.");
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NextParamName() {
			var q = new WeaverQuery();

			var mockVal = new Mock<IWeaverQueryVal>();
			mockVal.Setup(x => x.GetQuoted()).Returns("fake");

			for ( int i = 0 ; i <= 101 ; ++i ) {
				Assert.AreEqual("_P"+i, q.NextParamName, "Incorrect result at count "+i+".");
				q.AddParam(mockVal.Object);
			}
		}

	}

}