using System.Collections.Generic;
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
	public class TWeaverFuncBack : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			const int itemI = 99;

			var mockAlias = new Mock<IWeaverFuncAs<Person>>();
			mockAlias.SetupGet(x => x.PathIndex).Returns(itemI);

			var f = new WeaverFuncBack<Person>(mockAlias.Object);

			Assert.AreEqual("step"+itemI, f.Label, "Incorrect Label.");
			Assert.AreEqual(mockAlias.Object, f.BackToItem, "Incorrect BackToItem.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString() {
			const int itemI = 99;

			var mockAlias = new Mock<IWeaverFuncAs<Person>>();
			mockAlias.SetupGet(x => x.PathIndex).Returns(itemI);

			var f = new WeaverFuncBack<Person>(mockAlias.Object);

			Assert.AreEqual("back('step"+itemI+"')", f.BuildParameterizedString(), "Incorrect result.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(-1)]
		[TestCase(-22)]
		public void NotInPath(int pItemIndex) {
			var mockAlias = new Mock<IWeaverFuncAs<Person>>();
			mockAlias.SetupGet(x => x.PathIndex).Returns(pItemIndex);
			
			WeaverTestUtil.CheckThrows<WeaverFuncException>(true, () => {
				var f = new WeaverFuncBack<Person>(mockAlias.Object);
			});
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		[Category("Integration")]
		public void ComplexPath() {
			IWeaverFuncAs<Person> personAlias;

			IWeaverQuery q = WeavInst.BeginPath(new Root())
				.BaseNode
				.OutHasPerson.ToNode
					.Has(x => x.PersonId, WeaverFuncHasOp.EqualTo, 22)
					.Has(x => x.Name, WeaverFuncHasOp.EqualTo, "test")
					.As(out personAlias)
				.OutLikesCandy.ToNode
				.Back(personAlias)
				.End();
			
			const string expect = "g.v(0)"+
				".outE('"+TestSchema.RootHasPerson+"').inV"+
					".has('"+TestSchema.Person_PersonId+"',Tokens.T.eq,_P0)"+
					".has('"+TestSchema.Node_Name+"',Tokens.T.eq,_P1)"+
					".as('step5')"+
				".outE('"+TestSchema.PersonLikesCandy+"').inV"+
				".back('step5');";
			
			Assert.AreEqual(expect, q.Script, "Incorrect Query.Script.");
			
			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal(22));
			expectParams.Add("_P1", new WeaverQueryVal("test"));
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}

	}

}