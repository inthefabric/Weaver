using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weaver.Core;
using Weaver.Core.Graph;
using Weaver.Core.Query;
using Weaver.Core.Schema;
using Weaver.Core.Steps;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;
using Weaver.Test.Utils;

namespace Weaver.Test.WeavCore {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverInstance : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			var verts = new List<WeaverVertexSchema>();
			var edges = new List<WeaverEdgeSchema>();
			
			var wi = new WeaverInstance(verts, edges);

			Assert.NotNull(wi.Config, "Config should be filled.");
			Assert.AreEqual(verts, wi.Config.VertexSchemas, "Incorrect Vertex list.");
			Assert.AreEqual(edges, wi.Config.EdgeSchemas, "Incorrect Edge list.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Graph() {
			var wi = new WeaverInstance(new List<WeaverVertexSchema>(), new List<WeaverEdgeSchema>());
			WeaverGraph g = (WeaverGraph)wi.Graph;

			Assert.NotNull(g, "Graph should be filled.");
			Assert.NotNull(g.Path, "Graph.Path should be filled.");
			Assert.NotNull(g.Path.Config, "Graph.Path.Config should be filled.");
			Assert.NotNull(g.Path.Query, "Graph.Path.Query should be filled.");

			Assert.AreEqual(1, g.Path.Length, "Incorrect Path length.");
			Assert.AreEqual(g, g.Path.ItemAtIndex(0), "Incorrect Path item.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void FromVar() {
			const string name = "test";

			var mockVar = new Mock<IWeaverVarAlias<Person>>();
			mockVar.SetupGet(x => x.Name).Returns(name);

			var wi = new WeaverInstance(new List<WeaverVertexSchema>(), new List<WeaverEdgeSchema>());
			Person p = wi.FromVar(mockVar.Object);

			Assert.NotNull(p, "FromVar should be filled.");
			Assert.NotNull(p.Path, "FromVar.Path should be filled.");
			Assert.NotNull(p.Path.Config, "FromVar.Path.Config should be filled.");
			Assert.NotNull(p.Path.Query, "FromVar.Path.Query should be filled.");

			Assert.AreEqual(1, p.Path.Length, "Incorrect Path length.");
			Assert.NotNull((p.Path.ItemAtIndex(0) as WeaverStepCustom), "Incorrect Path item.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		[Category("Integration")]
		public void VertexById() {
			IWeaverQuery q = WeavInst.Graph.V.Id<Person>("123").ToQuery();

			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal("123"));

			Assert.AreEqual("g.v(_P0);", q.Script, "Incorrect Script.");
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		[Category("Integration")]
		public void PathBasedQuery() {
			IWeaverStepAs<Person> personAlias;

			IWeaverQuery q = WeavInst.Graph.V.ExactIndex<Root>(x => x.Id, 0)
				.OutHasPerson.InVertex
					.As(out personAlias)
				.InPersonKnows.OutVertex
					.Has(p => p.PersonId, WeaverStepHasOp.GreaterThan, 5)
					.Has(p => p.Name, WeaverStepHasOp.NotEqualTo, "Hello")
					.Has(p => p.Name, WeaverStepHasOp.NotEqualTo, "Goodbye")
					.Back(personAlias)
				.OutLikesCandy.InVertex
					.Property(c => c.Calories)
				.ToQuery();

			const string expectScript = "g.V('id',_P0)"+
				".outE('"+TestSchema.RootHasPerson+"').inV"+
					".as('step5')"+
				".inE('"+TestSchema.PersonKnowsPerson+"').outV"+
					".has('"+TestSchema.Person_PersonId+"',Tokens.T.gt,_P1)"+
					".has('Name',Tokens.T.neq,_P2)"+
					".has('Name',Tokens.T.neq,_P3)"+
					".back('step5')"+
				".outE('"+TestSchema.PersonLikesCandy+"').inV"+
					".property('"+TestSchema.Candy_Calories+"');";

			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal(0));
			expectParams.Add("_P1", new WeaverQueryVal(5));
			expectParams.Add("_P2", new WeaverQueryVal("Hello"));
			expectParams.Add("_P3", new WeaverQueryVal("Goodbye"));

			Assert.AreEqual(expectScript, q.Script, "Incorrect Script.");
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}

	}

}