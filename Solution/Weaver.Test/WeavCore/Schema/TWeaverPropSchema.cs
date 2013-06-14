using NUnit.Framework;
using Weaver.Core.Schema;

namespace Weaver.Test.WeavCore.Schema {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverPropSchema : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase("Name")]
		[TestCase("ItemId")]
		public void Constructor(string pName) {
			var ps = new WeaverPropSchema(pName, pName+"Db", typeof(string));
			
			Assert.AreEqual(pName, ps.Name, "Incorrect Name.");
			Assert.AreEqual(pName+"Db", ps.DbName, "Incorrect DbName.");
			Assert.AreEqual("String", ps.Type.Name, "Incorrect Type.");

			Assert.Null(ps.IsPrimaryKey, "Incorrect default IsPrimaryKey.");
			Assert.Null(ps.IsUnique, "Incorrect default IsUnique.");
			Assert.Null(ps.IsTimestamp, "Incorrect default IsTimestamp.");
			Assert.Null(ps.IsNullable, "Incorrect default IsNullable.");
			Assert.Null(ps.IsCaseInsensitive, "Incorrect default IsCaseInsensitive.");
			Assert.Null(ps.Len, "Incorrect default Len.");
			Assert.Null(ps.LenMin, "Incorrect default LenMin.");
			Assert.Null(ps.LenMax, "Incorrect default LenMax.");
			Assert.Null(ps.ValidRegex, "Incorrect default ValidRegex.");
		}

	}

}