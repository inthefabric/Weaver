using NUnit.Framework;

namespace Weaver.Test.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverQueryVal : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(123, false, "123")]
		[TestCase(1234567890L, false, "1234567890L")]
		[TestCase(123.456f, false, "123.456F")]
		[TestCase(123.456789, false, "123.456789D")]
		[TestCase("1234", true, "1234")]
		[TestCase(true, false, "true")]
		public void Constructor(object pValue, bool pIsString, string pFixedText) {
			var qv = new WeaverQueryVal(pValue);

			Assert.AreEqual(pValue, qv.Original, "Incorrect Original.");
			Assert.AreEqual(true, qv.AllowQuote, "Incorrect AllowQuote.");

			Assert.AreEqual(pValue+"", qv.RawText, "Incorrect RawText.");
			Assert.AreEqual(pIsString, qv.IsString, "Incorrect IsString.");

			Assert.AreEqual(pFixedText, qv.FixedText, "Incorrect FixedText.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(123, true, "123")]
		[TestCase(123, false, "123")]
		[TestCase("test", true, "'test'")]
		[TestCase("test", false, "test")]
		public void GetQuoted(object pValue, bool pAllowString, string pExpect) {
			var qv = new WeaverQueryVal(pValue, pAllowString);
			Assert.AreEqual(pExpect, qv.GetQuoted(), "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(123, true, "'123'")]
		[TestCase(123, false, "'123'")]
		[TestCase("test", true, "'test'")]
		[TestCase("test", false, "'test'")]
		public void GetQuotedForce(object pValue, bool pAllowString, string pExpect) {
			var qv = new WeaverQueryVal(pValue, pAllowString);
			Assert.AreEqual(pExpect, qv.GetQuotedForce(), "Incorrect result.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(123, "123")]
		[TestCase("test", "'test'")]
		[TestCase(false, "false")]
		public void QuoteIfString(object pValue, string pExpect) {
			Assert.AreEqual(pExpect, WeaverQueryVal.QuoteIfString(pValue), "Incorrect result.");
		}

	}

}