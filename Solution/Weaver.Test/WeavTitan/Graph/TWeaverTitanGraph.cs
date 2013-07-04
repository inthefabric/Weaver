using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Path;
using Weaver.Core.Pipe;
using Weaver.Core.Query;
using Weaver.Test.Common.Edges;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;
using Weaver.Test.Utils;
using Weaver.Test.WeavTitan.Common;
using Weaver.Titan.Graph;

namespace Weaver.Test.WeavTitan.Graph {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverTitanGraph : WeaverTestBase {

		private const string TryEachScript = "_TRY.each{k,v->if((z=v.getProperty(k))){p.put(k,z)}};";

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
		public void AddEdgeVci() {
			vMockPath.SetupGet(x => x.Query).Returns(new WeaverQuery()); //should mock this

			var okt = new OneKnowsTwo();
			okt.FirstProp = "test";
			okt.SecondProp = 998877;

			const string perId = "a99";
			const string twoId = "x1234";

			var one = new One { Id = perId };
			var two = new Two { Id = twoId };

			IWeaverQuery q = vGraph.AddEdgeVci(one, okt, two);
			Console.WriteLine(q.Script);

			const string expect = 
				"_A=g.v(_P0);"+
				"_B=g.v(_P1);"+
				"_PROP=[First:_P3,Second:_P4];"+
				"_TRY=[OA:_A,OD:_A,TB:_B,TC:_B,TD:_B];"+
				TryEachScript+
				"g.addEdge(_A,_B,_P2,_PROP);";

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.NotNull(q.Script, "Script should be filled.");
			Assert.AreEqual(expect, q.Script, "Incorrect starting code.");

			////

			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal(one.Id));
			expectParams.Add("_P1", new WeaverQueryVal(two.Id));
			expectParams.Add("_P2", new WeaverQueryVal("OKT"));
			expectParams.Add("_P3", new WeaverQueryVal(okt.FirstProp));
			expectParams.Add("_P4", new WeaverQueryVal(okt.SecondProp));
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddEdgeVciVarNoProps() {
			vMockPath.SetupGet(x => x.Query).Returns(new WeaverQuery()); //should mock this

			var ehe = new EmptyHasEmpty();
			const string outVarName = "_var0";
			const string inVarName = "_var1";

			var mockOutVar = new Mock<IWeaverVarAlias>();
			mockOutVar.SetupGet(x => x.Name).Returns(outVarName);
			mockOutVar.SetupGet(x => x.VarType).Returns(typeof(Empty));

			var mockInVar = new Mock<IWeaverVarAlias>();
			mockInVar.SetupGet(x => x.Name).Returns(inVarName);
			mockInVar.SetupGet(x => x.VarType).Returns(typeof(Empty));

			IWeaverQuery q = vGraph.AddEdgeVci(mockOutVar.Object, ehe, mockInVar.Object);

			const string expect = 
				"_PROP=[:];"+
				"g.addEdge("+outVarName+","+inVarName+",_P0,_PROP);";

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.NotNull(q.Script, "Script should be filled.");
			Assert.AreEqual(expect, q.Script, "Incorrect Script.");

			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal("EHE"));
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true)]
		[TestCase(false)]
		public void AddEdgeVciVarInvalidVarType(bool pBadOut) {
			var mockOutVar = new Mock<IWeaverVarAlias>();
			mockOutVar.SetupGet(x => x.VarType).Returns(typeof(Person));

			var mockInVar = new Mock<IWeaverVarAlias>();
			mockInVar.SetupGet(x => x.VarType).Returns(typeof(Person));

			var mockEdge = new Mock<IWeaverEdge>();
			mockEdge.SetupGet(x => x.OutVertexType).Returns(typeof(Person));
			mockEdge.SetupGet(x => x.InVertexType).Returns(typeof(Person));
			mockEdge.Setup(x => x.IsValidOutVertexType(It.IsAny<Type>())).Returns(!pBadOut);
			mockEdge.Setup(x => x.IsValidInVertexType(It.IsAny<Type>())).Returns(pBadOut);

			var ex = WeaverTestUtil.CheckThrows<WeaverException>(true, () =>
				vGraph.AddEdgeVci(mockOutVar.Object, mockEdge.Object, mockInVar.Object)
			);

			Assert.AreNotEqual(-1, ex.Message.IndexOf(pBadOut ? " Out " : " In "),
				"Incorrect exception.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddEdgeVciNoProps() {
			vMockPath.SetupGet(x => x.Query).Returns(new WeaverQuery()); //should mock this

			var ehe = new EmptyHasEmpty();
			var e1 = new Empty { Id = "V0" };
			var e2 = new Empty { Id = "eee99" };

			IWeaverQuery q = vGraph.AddEdgeVci(e1, ehe, e2);

			const string expect = 
				"_A=g.v(_P0);"+
				"_B=g.v(_P1);"+
				"_PROP=[:];"+
				"g.addEdge(_A,_B,_P2,_PROP);";

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.NotNull(q.Script, "Script should be filled.");
			Assert.AreEqual(expect, q.Script, "Incorrect Script.");

			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal(e1.Id));
			expectParams.Add("_P1", new WeaverQueryVal(e2.Id));
			expectParams.Add("_P2", new WeaverQueryVal("EHE"));
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(null, "x0")]
		[TestCase("x0", null)]
		public void AddEdgeVciFail(string pPerId, string pCanId) {
			var per = new Person { Id = pPerId };
			var can = new Candy { Id = pCanId };

			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => vGraph.AddEdgeVci(per, new PersonLikesCandy(), can)
			);
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
		public void TypeGroupOfErrId() {
			Exception e = WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => vGraph.TypeGroupOf<TestVertex>(1));
			Assert.True(e.Message.Contains("greater than"), "Incorrect exception.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void TypeGroupOfErrType() {
			Exception e = WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => vGraph.TypeGroupOf<TestVertex>(2));
			Assert.AreEqual(0, e.Message.IndexOf("Type"), "Incorrect exception.");
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
		[Test]
		public void MakeVertexPropertyErrType() {
			Exception e = WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => vGraph.MakeVertexPropertyKey<NullableProp>(x => x.NonTitanAttribute));
			Assert.AreEqual(0, e.Message.IndexOf("Type"), "Incorrect exception.");
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
				"signature(OB,OC,OE,TiNa,TA,TE)",
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