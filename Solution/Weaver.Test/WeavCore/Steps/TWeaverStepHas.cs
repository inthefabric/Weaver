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
	public class TWeaverStepHas : WeaverTestBase {

		private Mock<IWeaverQuery> vMockQuery;
		private Mock<IWeaverPath> vMockPath;
		private WeaverStepHas<Person> vFuncHas;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();
			vMockQuery = new Mock<IWeaverQuery>();

			vMockPath = new Mock<IWeaverPath>();
			vMockPath.SetupGet(x => x.Query).Returns(vMockQuery.Object);
			vMockPath.SetupGet(x => x.Config).Returns(WeavInst.Config);

			vFuncHas = new WeaverStepHas<Person>(n => n.PersonId, WeaverStepHasMode.Has, 
				WeaverStepHasOp.EqualTo, 1);
			vFuncHas.Path = vMockPath.Object;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(WeaverStepHasMode.Has, WeaverStepHasOp.EqualTo)]
		[TestCase(WeaverStepHasMode.HasNot, WeaverStepHasOp.EqualTo)]
		[TestCase(WeaverStepHasMode.Has, null)]
		[TestCase(WeaverStepHasMode.HasNot, null)]
		public void New(WeaverStepHasMode pMode, WeaverStepHasOp pOp) {
			const string val = "test";
			var f = new WeaverStepHas<Person>(n => n.PersonId, pMode, pOp, val);

			Assert.AreEqual(pMode, f.Mode, "Incorrect Mode.");
			Assert.AreEqual(pOp, f.Operation, "Incorrect Operation.");
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
			vFuncHas = new WeaverStepHas<Person>(n => (n.PersonId == 99), WeaverStepHasMode.Has, 
				WeaverStepHasOp.EqualTo, 1);
			vFuncHas.Path = vMockPath.Object;

			WeaverTestUtil.CheckThrows<WeaverStepException>(true, () => {
				var p = vFuncHas.PropertyName;
			});
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(WeaverStepHasOp.EqualTo, "test", "eq,_P0")]
		[TestCase(WeaverStepHasOp.NotEqualTo, null, "neq,null")]
		[TestCase(WeaverStepHasOp.GreaterThan, 123u, "gt,123")]
		[TestCase(WeaverStepHasOp.GreaterThanOrEqualTo, 1.5f, "gte,1.5")]
		[TestCase(WeaverStepHasOp.LessThan, 99, "lt,99")]
		[TestCase(WeaverStepHasOp.LessThanOrEqualTo, 1.23456789d, "lte,1.23456789")]
		[TestCase(null, null, null)]
		[TestCase(null, "test", null)]
		[TestCase(null, 1234, null)]
		public void BuildParameterizedString(WeaverStepHasOp? pOper, object pValue, string pExpect) {
			var val = (pValue is string ? "_P0" : pValue+"");
			if ( pValue == null ) { val = "null"; }

			vMockQuery.Setup(x => x.AddParam(It.IsAny<WeaverQueryVal>())).Returns(val);
			var modes = new[] { WeaverStepHasMode.Has, WeaverStepHasMode.HasNot };
			var funcs = new[] { "has", "hasNot" };

			for ( int i = 0 ; i < modes.Length ; ++i ) {
				vFuncHas = new WeaverStepHas<Person>(n => n.PersonId, modes[i], pOper, pValue);
				vFuncHas.Path = vMockPath.Object;

				if ( pOper == null ) {
					Assert.AreEqual(funcs[i]+"('"+TestSchema.Person_PersonId+"')",
						vFuncHas.BuildParameterizedString(), "Incorrect result.");
				}
				else {
					Assert.AreEqual(funcs[i]+"('"+TestSchema.Person_PersonId+"',Tokens.T."+pExpect+")",
						vFuncHas.BuildParameterizedString(), "Incorrect result.");
				}
			}
		}

	}

}