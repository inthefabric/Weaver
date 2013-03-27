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
	public class TWeaverFuncKeyIndex : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			var q = new WeaverFuncKeyIndex<Person>(x => x.PersonId, 123, false);
			Assert.AreEqual(123, q.Value, "Incorrect Value.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void IndexName() {
			var q = new WeaverFuncKeyIndex<Person>(x => x.PersonId, 123, false);
			Assert.AreEqual("PersonId", q.IndexName, "Incorrect IndexName.");
			Assert.AreEqual("PersonId", q.IndexName, "Incorrect cached IndexName.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void IndexNameInvalid() {
			var q = new WeaverFuncKeyIndex<Person>(x => (x.ExpectOneNode == false), 123, false);

			WeaverTestUtil.CheckThrows<WeaverFuncException>(true, () => {
				var p = q.IndexName;
			});
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[TestCase("PersonId", 123, true, "V('PersonId',123)[0]")]
		[TestCase("Name", "zach", false, "V('Name',_P0)")]
		[TestCase("Age", 27.1f, true, "V('Age',27.1)[0]")]
		[TestCase("Name", null, false, "V('Name',null)")]
		public void BuildParameterizedString(string pPropName, object pValue, bool pSingle, 
																					string pExpect) {
			Expression<Func<Person, object>> func = null;

			switch ( pPropName ) {
				case "PersonId": func = (p => p.PersonId); break;
				case "Name": func = (p => p.Name); break;
				case "Age": func = (p => p.Age); break;
			}
			
			var val = (pValue is string ? "_P0" : pValue+"");
			if ( pValue == null ) { val = "null"; }

			var mockQuery = new Mock<IWeaverQuery>();
			mockQuery.Setup(x => x.AddParam(It.IsAny<WeaverQueryVal>())).Returns(val);

			var mockPath = new Mock<IWeaverPath>();
			mockPath.SetupGet(x => x.Query).Returns(mockQuery.Object);

			var f = new WeaverFuncKeyIndex<Person>(func, pValue, pSingle);
			f.Path = mockPath.Object;

			Assert.AreEqual(pExpect, f.BuildParameterizedString(), "Incorrect result.");
		}

	}

}