using System;
using System.Linq.Expressions;
using NUnit.Framework;
using Weaver.Functions;
using Weaver.Test.Common.Nodes;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncIndex : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewIndex() {
			var q = new WeaverFuncIndex<Person>("Test", x => x.PersonId, 123);

			Assert.AreEqual("Test", q.IndexName, "Incorrect IndexName.");
			Assert.AreEqual("PersonId", q.PropertyName, "Incorrect PropertyName.");
			Assert.AreEqual("PersonId", q.PropertyName, "Incorrect cached PropertyName.");
			Assert.AreEqual(123, q.Value, "Incorrect Value.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[TestCase("PersonId", 123, "g.idx(Person).get('PersonId', 123)")]
		[TestCase("Name", "zach", "g.idx(Person).get('Name', 'zach')")]
		[TestCase("Age", 27.1f, "g.idx(Person).get('Age', 27.1F)")]
		public void Gremlin(string pPropName, object pValue, string pExpect) {
			const string indexName = "Person";
			Expression<Func<Person, object>> func = null;

			switch ( pPropName ) {
				case "PersonId": func = (p => p.PersonId); break;
				case "Name": func = (p => p.Name); break;
				case "Age": func = (p => p.Age); break;
			}

			var q = new WeaverFuncIndex<Person>(indexName, func, pValue);

			Assert.AreEqual(indexName, q.IndexName, "Incorrect IndexName.");
			Assert.AreEqual(pValue, q.Value, "Incorrect Value.");
			Assert.AreEqual(pExpect, q.Script, "Incorrect GrelminCode.");
		}

	}

}