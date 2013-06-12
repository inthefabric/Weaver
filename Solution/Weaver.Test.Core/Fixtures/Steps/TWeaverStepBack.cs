using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weaver.Core.Exceptions;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Test.Core.Common.Schema;
using Weaver.Test.Core.Common.Vertices;
using Weaver.Test.Core.Utils;

namespace Weaver.Test.Core.Fixtures.Steps {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverStepBack : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			const int itemI = 99;

			var mockAlias = new Mock<IWeaverStepAs<Person>>();
			mockAlias.SetupGet(x => x.PathIndex).Returns(itemI);

			var f = new WeaverStepBack<Person>(mockAlias.Object);

			Assert.AreEqual("step"+itemI, f.Label, "Incorrect Label.");
			Assert.AreEqual(mockAlias.Object, f.BackToItem, "Incorrect BackToItem.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString() {
			const int itemI = 99;

			var mockAlias = new Mock<IWeaverStepAs<Person>>();
			mockAlias.SetupGet(x => x.PathIndex).Returns(itemI);

			var f = new WeaverStepBack<Person>(mockAlias.Object);

			Assert.AreEqual("back('step"+itemI+"')", f.BuildParameterizedString(), "Incorrect result.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(-1)]
		[TestCase(-22)]
		public void NotInPath(int pItemIndex) {
			var mockAlias = new Mock<IWeaverStepAs<Person>>();
			mockAlias.SetupGet(x => x.PathIndex).Returns(pItemIndex);
			
			WeaverTestUtil.CheckThrows<WeaverStepException>(true, () => {
				var f = new WeaverStepBack<Person>(mockAlias.Object);
			});
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		[Category("Integration")]
		public void ComplexPath() {
			IWeaverStepAs<Person> personAlias;

			IWeaverQuery q = WeavInst.Graph.V.ExactIndex<Root>(x => x.Id, 0)
				.OutHasPerson.ToVertex
					.Has(x => x.PersonId, WeaverStepHasOp.EqualTo, 22)
					.Has(x => x.Name, WeaverStepHasOp.EqualTo, "test")
					.As(out personAlias)
				.OutLikesCandy.ToVertex
				.Back(personAlias)
				.ToQuery();
			
			const string expect = "g.V('id',_P0)"+
				".outE('"+TestSchema.RootHasPerson+"').inV"+
					".has('"+TestSchema.Person_PersonId+"',Tokens.T.eq,_P1)"+
					".has('"+TestSchema.Vertex_Name+"',Tokens.T.eq,_P2)"+
					".as('step7')"+
				".outE('"+TestSchema.PersonLikesCandy+"').inV"+
				".back('step7');";
			
			Assert.AreEqual(expect, q.Script, "Incorrect Query.Script.");
			
			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal(0));
			expectParams.Add("_P1", new WeaverQueryVal(22));
			expectParams.Add("_P2", new WeaverQueryVal("test"));
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}

	}

}