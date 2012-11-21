using NUnit.Framework;
using Weaver.Functions;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncIndex {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase("PersonId", "123", false, "index.get('PersonId', 123)")]
		[TestCase("Name", "zach", true, "index.get('Name', 'zach')")]
		[TestCase("Age", "27.1", false, "index.get('Age', 27.1)")]
		public void Gremlin(string pName, string pVal, bool pIsString, string pExpect) {
			var q = new WeaverFuncIndex(pName, pVal, pIsString);

			Assert.AreEqual(pName, q.IndexName, "Incorrect IndexName.");
			Assert.AreEqual(pVal, q.Value, "Incorrect Value.");
			Assert.AreEqual(pIsString, q.ValueIsString, "Incorrect ValueIsString.");
			Assert.AreEqual(pExpect, q.GremlinCode, "Incorrect GrelminCode.");
		}

	}

}