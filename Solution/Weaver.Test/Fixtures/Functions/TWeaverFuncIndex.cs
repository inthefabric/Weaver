using System;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncIndex : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			var q = new WeaverFuncIndex<Person>("Test", x => x.PersonId, 123);
			Assert.AreEqual("Test", q.IndexName, "Incorrect IndexName.");
			Assert.AreEqual(123, q.Value, "Incorrect Value.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropertyName() {
			var q = new WeaverFuncIndex<Person>("Test", x => x.PersonId, 123);
			Assert.AreEqual("PersonId", q.PropertyName, "Incorrect PropertyName.");
			Assert.AreEqual("PersonId", q.PropertyName, "Incorrect cached PropertyName.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropertyNameInvalid() {
			var q = new WeaverFuncIndex<Person>("Test", x => (x.ExpectOneNode == false), 123);

			WeaverTestUtil.CheckThrows<WeaverFuncException>(true, () => {
				var p = q.PropertyName;
			});
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[TestCase("PersonId", 123, "g.idx(_P0).get('PersonId',123)")]
		[TestCase("Name", "zach", "g.idx(_P0).get('Name',_P1)")]
		[TestCase("Age", 27.1f, "g.idx(_P0).get('Age',27.1)")]
		[TestCase("Name", null, "g.idx(_P0).get('Name',null)")]
		public void BuildParameterizedString(string pPropName, object pValue, string pExpect) {
			const string indexName = "Person";
			Expression<Func<Person, object>> func = null;

			switch ( pPropName ) {
				case "PersonId": func = (p => p.PersonId); break;
				case "Name": func = (p => p.Name); break;
				case "Age": func = (p => p.Age); break;
			}
			
			var val = (pValue is string ? "_P1" : pValue+"");
			if ( pValue == null ) { val = "null"; }

			var mockQuery = new Mock<IWeaverQuery>();
			mockQuery.Setup(x => x.AddParam(It.IsAny<WeaverQueryVal>())).Returns("_P0");
			mockQuery.Setup(x => x.AddParamIfString(It.IsAny<WeaverQueryVal>())).Returns(val);

			var mockPath = new Mock<IWeaverPath>();
			mockPath.SetupGet(x => x.Query).Returns(mockQuery.Object);

			var f = new WeaverFuncIndex<Person>(indexName, func, pValue);
			f.Path = mockPath.Object;

			Assert.AreEqual(pExpect, f.BuildParameterizedString(), "Incorrect result.");
		}

	}

}