using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Path;
using Weaver.Core.Pipe;
using Weaver.Core.Query;
using Weaver.Core.Schema;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;
using Weaver.Test.Utils;
using Weaver.Test.WeavTitan.Common;
using Weaver.Titan.Graph;
using Weaver.Titan.Schema;

namespace Weaver.Test.WeavTitan.Graph {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverTitanGraph : WeaverTestBase {

		private WeaverTitanGraph vGraph;
		private Mock<IWeaverPath> vMockPath;
		private IList<IWeaverPathItem> vPathItems;
		private IList<IWeaverQueryVal> vQueryVals;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();

			vPathItems = new List<IWeaverPathItem>();
			vQueryVals = new List<IWeaverQueryVal>();

			var mockQuery = new Mock<IWeaverQuery>();
			mockQuery
				.Setup(x => x.AddParam(It.IsAny<IWeaverQueryVal>()))
				.Returns(() => "_P"+vQueryVals.Count)
				.Callback((IWeaverQueryVal v) => vQueryVals.Add(v));

			vMockPath = new Mock<IWeaverPath>();
			vMockPath.SetupGet(x => x.Query).Returns(mockQuery.Object);
			vMockPath
				.Setup(x => x.AddItem(It.IsAny<IWeaverPathItem>()))
				.Callback((IWeaverPathItem x) => vPathItems.Add(x));

			vGraph = new WeaverTitanGraph();
			vGraph.Path = vMockPath.Object;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void QueryV() {
			IWeaverTitanGraphQuery qv = vGraph.QueryV();

			Assert.NotNull(qv, "Result should be filled.");
			Assert.AreEqual(1, vPathItems.Count, "Incorrect PathItems.Count.");
			Assert.AreEqual(qv, vPathItems[0], "Incorrect Path item.");

			WeaverTitanGraphQuery q = (WeaverTitanGraphQuery)qv;
			Assert.True(q.VertMode, "Incorrect Query.VertMode.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void QueryE() {
			IWeaverTitanGraphQuery qe = vGraph.QueryE();

			Assert.NotNull(qe, "Result should be filled.");
			Assert.AreEqual(1, vPathItems.Count, "Incorrect PathItems.Count.");
			Assert.AreEqual(qe, vPathItems[0], "Incorrect Path item.");

			WeaverTitanGraphQuery q = (WeaverTitanGraphQuery)qe;
			Assert.False(q.VertMode, "Incorrect Query.VertMode.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void TypeGroupOf() {
			const int id = 2;

			IWeaverQuery q = vGraph.TypeGroupOf<TitanPerson>(id);

			Assert.NotNull(q, "Result should be filled.");
			Assert.AreEqual("com.thinkaurelius.titan.core.TypeGroup.of(_P0,'group');", q.Script,
				"Incorrect Query.Script.");
			
			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal(id));
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void TypeGroupOfFail() {
			WeaverTestUtil.CheckThrows<WeaverException>(true, () => vGraph.TypeGroupOf<TestVertex>(1));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void MakeVertexPropertyKeyMax() {
			const string groupName = "_MyGroup";

			var mockGroup = new Mock<IWeaverVarAlias>();
			mockGroup.SetupGet(x => x.Name).Returns(groupName);

			IWeaverPathPipeEnd end = 
				vGraph.MakeVertexPropertyKey<TitanPerson>(x => x.Age, mockGroup.Object);
			
			var expect = new[] {
				"group("+groupName+")",
				"indexed(Vertex.class)",
				"indexed('search',Vertex.class)"
			};

			CheckMakeProperty(end, expect, TestSchema.Person_Age, "Float");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void MakeVertexPropertyKeyMin() {
			IWeaverPathPipeEnd end = vGraph.MakeVertexPropertyKey<TitanPerson>(x => x.IsMale);
			CheckMakeProperty(end, new string[0], TestSchema.Person_IsMale, "Boolean");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void MakeVertexPropertyKeyNullable() {
			IWeaverPathPipeEnd end = vGraph.MakeVertexPropertyKey<NullableProp>(x => x.A);
			CheckMakeProperty(end, new string[0], "A", "Integer");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void MakeEdgePropertyKey() {
			IWeaverPathPipeEnd end = 
				vGraph.MakeEdgePropertyKey<TitanPersonKnowsTitanPerson>(x => x.Amount);

			var expect = new[] {
				"indexed(Edge.class)",
				"indexed('search',Edge.class)"
			};

			CheckMakeProperty(end, expect, TestSchema.PersonKnowsPerson_Amount, "Float");
		}

		/*--------------------------------------------------------------------------------------------*/
		private void CheckMakeProperty(IWeaverPathPipeEnd pResult, IEnumerable<string> pOptional,
																	string pPropDbName, string pType) {
			Assert.NotNull(pResult, "Result should be filled.");

			var expect = new List<string>(new[] {
				"makeType()",
				"dataType("+pType+".class)",
				"name(_P0)",
				"unique(OUT)"
			});

			expect.AddRange(pOptional);
			expect.Add("makePropertyKey()");

			Assert.AreEqual(expect.Count, vPathItems.Count, "Incorrect PathItems.Count.");

			for ( int i = 0 ; i < expect.Count ; ++i ) {
				string script = vPathItems[i].BuildParameterizedString();
				Console.WriteLine("Script "+i+": "+script);
				Assert.AreEqual(expect[i], script, "Incorrect PathItem script at index "+i+".");
			}

			Assert.AreEqual(1, vQueryVals.Count, "Incorrect QueryVals.Count.");
			Assert.AreEqual(pPropDbName, vQueryVals[0].Original, "Incorrect QueryVals[0] value.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildEdgeLabelMin() {
			IWeaverPathPipeEnd end = vGraph.BuildEdgeLabel<EmptyHasEmpty>(p => null);
			CheckBuildEdge(end, new string[0], "EHE");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildEdgeLabelMax() {
			IWeaverPathPipeEnd end = vGraph.BuildEdgeLabel<OneKnowsTwo>(n => new WeaverVarAlias(n));

			var expect = new[] {
				"primaryKey(OA,OD,TB,TC,TD)",
				"signature(OB,OC,OE,TA,TE)",
				"unique(IN)",
				"unique(OUT)"
			};
			
			CheckBuildEdge(end, expect, "OKT");
		}

		/*--------------------------------------------------------------------------------------------*/
		private void CheckBuildEdge(IWeaverPathPipeEnd pResult, IEnumerable<string> pOptional,
																					string pDbName) {
			Assert.NotNull(pResult, "Result should be filled.");

			var expect = new List<string>(new[] {
				"makeType()",
				"name(_P0)",
			});

			expect.AddRange(pOptional);
			expect.Add("makeEdgeLabel()");

			Assert.AreEqual(expect.Count, vPathItems.Count, "Incorrect PathItems.Count.");

			for ( int i = 0 ; i < expect.Count ; ++i ) {
				string script = vPathItems[i].BuildParameterizedString();
				Console.WriteLine("Script "+i+": "+script);
				Assert.AreEqual(expect[i], script, "Incorrect PathItem script at index "+i+".");
			}

			Assert.AreEqual(1, vQueryVals.Count, "Incorrect QueryVals.Count.");
			Assert.AreEqual(pDbName, vQueryVals[0].Original, "Incorrect QueryVals[0] value.");
		}

	}

}