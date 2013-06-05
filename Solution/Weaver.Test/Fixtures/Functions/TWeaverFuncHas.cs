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

			vFuncHas = new WeaverFuncHas<Person>(n => n.PersonId, WeaverFuncHasMode.Has, 
				WeaverFuncHasOp.EqualTo, 1);
			vFuncHas.Path = vMockPath.Object;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(WeaverFuncHasMode.Has)]
		[TestCase(WeaverFuncHasMode.HasNot)]
		public void New(WeaverFuncHasMode pMode) {
			const WeaverFuncHasOp op = WeaverFuncHasOp.EqualTo;
			const string val = "test";
			var f = new WeaverFuncHas<Person>(n => n.PersonId, pMode, op, val);

			Assert.AreEqual(pMode, f.Mode, "Incorrect Mode.");
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
			vFuncHas = new WeaverFuncHas<Person>(n => (n.PersonId == 99), WeaverFuncHasMode.Has, 
				WeaverFuncHasOp.EqualTo, 1);
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
			var modes = new[] { WeaverFuncHasMode.Has, WeaverFuncHasMode.HasNot };
			var funcs = new[] { "has", "hasNot" };

			for ( int i = 0 ; i < modes.Length ; ++i ) {
				vFuncHas = new WeaverFuncHas<Person>(n => n.PersonId, modes[i], pOperation, pValue);
				vFuncHas.Path = vMockPath.Object;

				Assert.AreEqual(funcs[i]+"('"+TestSchema.Person_PersonId+"',Tokens.T."+pExpect+")",
					vFuncHas.BuildParameterizedString(), "Incorrect result.");
			}
		}

	}

}