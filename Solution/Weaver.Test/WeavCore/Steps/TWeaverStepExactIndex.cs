using System;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using Weaver.Core.Exceptions;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;
using Weaver.Test.Utils;

namespace Weaver.Test.WeavCore.Steps {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverStepExactIndex : WeaverTestBase {

		private Mock<IWeaverQuery> vMockQuery;
		private Mock<IWeaverPath> vMockPath;
		private WeaverStepExactIndex<Person> vKeyIndex;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();
			vMockQuery = new Mock<IWeaverQuery>();

			vMockPath = new Mock<IWeaverPath>();
			vMockPath.SetupGet(x => x.Query).Returns(vMockQuery.Object);
			vMockPath.SetupGet(x => x.Config).Returns(WeavInst.Config);

			vKeyIndex = new WeaverStepExactIndex<Person>(x => x.PersonId, 123);
			vKeyIndex.Path = vMockPath.Object;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			Assert.AreEqual(123, vKeyIndex.Value, "Incorrect Value.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void IndexName() {
			Assert.AreEqual(TestSchema.Person_PersonId, vKeyIndex.IndexName,
				"Incorrect IndexName.");
			Assert.AreEqual(TestSchema.Person_PersonId, vKeyIndex.IndexName,
				"Incorrect cached IndexName.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void IndexNameInvalid() {
			vKeyIndex = new WeaverStepExactIndex<Person>(n => (n.PersonId == 99), 1);
			vKeyIndex.Path = vMockPath.Object;

			WeaverTestUtil.CheckThrows<WeaverStepException>(true, () => {
				var p = vKeyIndex.IndexName;
			});
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[TestCase("PersonId", 123, "('"+TestSchema.Person_PersonId+"',123)")]
		[TestCase("Name", "zach", "('Name',_P0)")]
		[TestCase("Age", 27.1f, "('Age',27.1)")]
		[TestCase("Name", null, "('Name',null)")]
		public void BuildParameterizedString(string pPropName, object pValue, string pExpect) {
			Expression<Func<Person, object>> func = null;

			switch ( pPropName ) {
				case "PersonId": func = (p => p.PersonId); break;
				case "Name": func = (p => p.Name); break;
				case "Age": func = (p => p.Age); break;
			}
			
			var val = (pValue is string ? "_P0" : pValue+"");
			if ( pValue == null ) { val = "null"; }

			vMockQuery.Setup(x => x.AddParam(It.IsAny<WeaverQueryVal>())).Returns(val);

			vKeyIndex = new WeaverStepExactIndex<Person>(func, pValue+"");
			vKeyIndex.Path = vMockPath.Object;

			Assert.AreEqual(pExpect, vKeyIndex.BuildParameterizedString(), "Incorrect result.");
		}

	}

}