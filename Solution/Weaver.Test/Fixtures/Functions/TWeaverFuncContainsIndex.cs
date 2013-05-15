using System;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.Schema;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncContainsIndex : WeaverTestBase {

		private Mock<IWeaverQuery> vMockQuery;
		private Mock<IWeaverPath> vMockPath;
		private WeaverFuncContainsIndex<Person> vContainsIndex;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();
			vMockQuery = new Mock<IWeaverQuery>();

			vMockPath = new Mock<IWeaverPath>();
			vMockPath.SetupGet(x => x.Query).Returns(vMockQuery.Object);
			vMockPath.SetupGet(x => x.Config).Returns(WeavInst.Config);

			vContainsIndex = new WeaverFuncContainsIndex<Person>(x => x.PersonId, "abc");
			vContainsIndex.Path = vMockPath.Object;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			Assert.AreEqual("abc", vContainsIndex.Value, "Incorrect Value.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void IndexName() {
			Assert.AreEqual(TestSchema.Person_PersonId, vContainsIndex.IndexName,
				"Incorrect IndexName.");
			Assert.AreEqual(TestSchema.Person_PersonId, vContainsIndex.IndexName,
				"Incorrect cached IndexName.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void IndexNameInvalid() {
			vContainsIndex = new WeaverFuncContainsIndex<Person>(n => (n.PersonId == 99), "1");
			vContainsIndex.Path = vMockPath.Object;

			WeaverTestUtil.CheckThrows<WeaverFuncException>(true, () => {
				var p = vContainsIndex.IndexName;
			});
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[TestCase("abc", "query().has('"+TestSchema.Node_Name+"',"+
			"com.thinkaurelius.titan.core.attribute.Text.CONTAINS,_P0)")]
		public void BuildParameterizedString(string pValue, string pExpect) {
			vMockQuery.Setup(x => x.AddParam(It.IsAny<WeaverQueryVal>())).Returns("_P0");

			vContainsIndex = new WeaverFuncContainsIndex<Person>(p => p.Name, pValue);
			vContainsIndex.Path = vMockPath.Object;

			Assert.AreEqual(pExpect, vContainsIndex.BuildParameterizedString(), "Incorrect result.");
		}

	}

}