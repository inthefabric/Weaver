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
	public class TWeaverFuncHas : WeaverTestBase {

		private Mock<IWeaverQuery> vMockQuery;
		private Mock<IWeaverPath> vMockPath;
		private WeaverFuncHas<Person> vFuncHas;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();
			vMockQuery = new Mock<IWeaverQuery>();

			vMockPath = new Mock<IWeaverPath>();
			vMockPath.SetupGet(x => x.Query).Returns(vMockQuery.Object);
			vMockPath.SetupGet(x => x.Config).Returns(WeavInst.Config);

			vFuncHas = new WeaverFuncHas<Person>(n => n.PersonId, WeaverFuncHasOp.EqualTo, 1);
			vFuncHas.Path = vMockPath.Object;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			const WeaverFuncHasOp op = WeaverFuncHasOp.EqualTo;
			const string val = "test";
			var f = new WeaverFuncHas<Person>(n => n.PersonId, op, val);

			Assert.AreEqual(op, f.Operation, "Incorrect Operation.");
			Assert.AreEqual(val, f.Value, "Incorrect Value.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropertyName() {
			Assert.AreEqual(TestSchema.Person_PersonId, vFuncHas.PropertyName,
				"Incorrect PropertyName.");
			Assert.AreEqual(TestSchema.Person_PersonId, vFuncHas.PropertyName,
				"Incorrect cached PropertyName.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropertyNameInvalid() {
			vFuncHas = new WeaverFuncHas<Person>(n => (n.PersonId == 99), WeaverFuncHasOp.EqualTo,1);
			vFuncHas.Path = vMockPath.Object;

			WeaverTestUtil.CheckThrows<WeaverFuncException>(true, () => {
				var p = vFuncHas.PropertyName;
			});
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(WeaverFuncHasOp.EqualTo, "test", "eq,_P0")]
		[TestCase(WeaverFuncHasOp.NotEqualTo, null, "neq,null")]
		[TestCase(WeaverFuncHasOp.GreaterThan, 123u, "gt,123")]
		[TestCase(WeaverFuncHasOp.GreterThanOrEqualTo, 1.5f, "gte,1.5")]
		[TestCase(WeaverFuncHasOp.LessThan, 99, "lt,99")]
		[TestCase(WeaverFuncHasOp.LessThanOrEqualTo, 1.23456789d, "lte,1.23456789")]
		public void BuildParameterizedString(WeaverFuncHasOp pOperation, object pValue, string pExpect){
			var val = (pValue is string ? "_P0" : pValue+"");
			if ( pValue == null ) { val = "null"; }

			vMockQuery.Setup(x => x.AddParam(It.IsAny<WeaverQueryVal>())).Returns(val);

			vFuncHas = new WeaverFuncHas<Person>(n => n.PersonId, pOperation, pValue);
			vFuncHas.Path = vMockPath.Object;

			Assert.AreEqual("has('"+TestSchema.Person_PersonId+"',Tokens.T."+pExpect+")",
				vFuncHas.BuildParameterizedString(), "Incorrect result.");
		}

	}

}