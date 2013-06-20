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
using Weaver.Test.Utils;
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
			const string dbName = "Te";
			var vert = new WeaverVertexSchema("Test", dbName);

			IWeaverQuery q = vGraph.TypeGroupOf(vert, id);

			Assert.NotNull(q, "Result should be filled.");
			Assert.AreEqual("com.thinkaurelius.titan.core.TypeGroup.of(_P0,_P1);", q.Script,
				"Incorrect Query.Script.");
			
			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal(id));
			expectParams.Add("_P1", new WeaverQueryVal(dbName));
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void TypeGroupOfFail() {
			var vert = new WeaverVertexSchema("Test", "Te");
			WeaverTestUtil.CheckThrows<WeaverException>(true, () => vGraph.TypeGroupOf(vert, 1));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void MakePropertyKeyMax() {
			const string propDbName = "Pr";
			const string groupName = "_MyGroup";

			var vert = new WeaverVertexSchema(null);
			var prop = new WeaverTitanPropSchema("Prop", propDbName, typeof(int));
			prop.TitanIndex = true;
			prop.TitanElasticIndex = true;
			
			var mockGroup = new Mock<IWeaverVarAlias>();
			mockGroup.SetupGet(x => x.Name).Returns(groupName);

			IWeaverPathPipeEnd end = vGraph.MakePropertyKey(vert, prop, mockGroup.Object);
			
			var expect = new[] {
				"group("+groupName+")",
				"indexed(Vertex.class)",
				"indexed('search',Vertex.class)"
			};

			CheckMakeProperty(end, expect, propDbName, "Integer");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void MakePropertyKeyMin() {
			const string propDbName = "Pr";
			var vert = new WeaverVertexSchema("Test", "Te");
			var prop = new WeaverTitanPropSchema("Prop", propDbName, typeof(string));

			IWeaverPathPipeEnd end = vGraph.MakePropertyKey(vert, prop);
			CheckMakeProperty(end, new string[0], propDbName, "String");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void MakePropertyKeyEdge() {
			const string propDbName = "Pr";
			var edge = new WeaverEdgeSchema(null, null, null, null, null);
			var prop = new WeaverTitanPropSchema("Prop", propDbName, typeof(long));
			prop.TitanIndex = true;
			prop.TitanElasticIndex = true;

			IWeaverPathPipeEnd end = vGraph.MakePropertyKey(edge, prop);

			var expect = new[] {
				"indexed(Edge.class)",
				"indexed('search',Edge.class)"
			};

			CheckMakeProperty(end, expect, propDbName, "Long");
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
			const string edgeDbName = "FUT";
			var vertF = new WeaverVertexSchema("FromVert", "Fr");
			var vertT = new WeaverVertexSchema("ToVert", "To");
			var edge = new WeaverEdgeSchema(vertF, "FromUsesTo", edgeDbName, "Uses", vertT);

			IWeaverPathPipeEnd end = vGraph.BuildEdgeLabel(edge, p => null);
			CheckBuildEdge(end, new string[0], edgeDbName);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildEdgeLabelMax() {
			const string edgeDbName = "FUT";
			const string fv1 = "_FV1";
			const string fv2 = "_FV2";
			const string fv3 = "_FV3";
			const string fv4 = "_FV4";
			const string fv5 = "_FV5";
			const string tv1 = "_TV1";
			const string tv2 = "_TV2";
			const string tv3 = "_TV3";
			const string tv4 = "_TV4";
			const string tv5 = "_TV5";

			var vertF = new WeaverVertexSchema("FromVert", "Fr");
			var vertT = new WeaverVertexSchema("ToVert", "To");
			var edge = new WeaverEdgeSchema(vertF, "FromUsesTo", edgeDbName, "Uses", vertT);
			edge.OutVertexConn = WeaverEdgeConn.OutToZeroOrOne;
			edge.InVertexConn = WeaverEdgeConn.InFromOne;

			////
			
			var prop = new WeaverTitanPropSchema(null, fv1, null);
			prop.AddTitanVertexCentricIndex(edge);
			vertF.Props.Add(prop);

			prop = new WeaverTitanPropSchema(null, fv2, null);
			vertF.Props.Add(prop);

			prop = new WeaverTitanPropSchema(null, fv3, null);
			vertF.Props.Add(prop);

			prop = new WeaverTitanPropSchema(null, fv4, null);
			prop.AddTitanVertexCentricIndex(edge);
			vertF.Props.Add(prop);

			prop = new WeaverTitanPropSchema(null, fv5, null);
			vertF.Props.Add(prop);

			////

			prop = new WeaverTitanPropSchema(null, tv1, null);
			vertT.Props.Add(prop);

			prop = new WeaverTitanPropSchema(null, tv2, null);
			prop.AddTitanVertexCentricIndex(edge);
			vertT.Props.Add(prop);

			prop = new WeaverTitanPropSchema(null, tv3, null);
			prop.AddTitanVertexCentricIndex(edge);
			vertT.Props.Add(prop);

			prop = new WeaverTitanPropSchema(null, tv4, null);
			prop.AddTitanVertexCentricIndex(edge);
			vertT.Props.Add(prop);

			prop = new WeaverTitanPropSchema(null, tv5, null);
			vertT.Props.Add(prop);

			////

			IWeaverPathPipeEnd end = vGraph.BuildEdgeLabel(edge, p => new WeaverVarAlias(p.DbName));

			var expect = new[] {
				"primaryKey("+fv1+","+fv4+","+tv2+","+tv3+","+tv4+")",
				"signature("+fv2+","+fv3+","+fv5+","+tv1+","+tv5+")",
				"unique(IN)",
				"unique(OUT)"
			};
			
			CheckBuildEdge(end, expect, edgeDbName);
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