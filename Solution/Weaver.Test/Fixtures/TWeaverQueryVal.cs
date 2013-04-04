using NUnit.Framework;

namespace Weaver.Test.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverQueryVal : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(123, false, "123")]
		[TestCase(1234567890L, false, "1234567890")]
		[TestCase(123.456f, false, "123.456")]
		[TestCase(123.456789, false, "123.456789")]
		[TestCase("1234", true, "1234")]
		[TestCase("with 'inner' quote", true, "with 'inner' quote")]
		[TestCase("1234", true, "1234")]
		[TestCase(true, false, "true")]
		[TestCase(null, false, "null")]
		public void New(object pValue, bool pIsString, string pFixedText) {
			var qv = new WeaverQueryVal(pValue);

			Assert.AreEqual(pValue, qv.Original, "Incorrect Original.");

			Assert.AreEqual(pValue+"", qv.RawText, "Incorrect RawText.");
			Assert.AreEqual(pIsString, qv.IsString, "Incorrect IsString.");

			Assert.AreEqual(pFixedText, qv.FixedText, "Incorrect FixedText.");
		}

	}

}