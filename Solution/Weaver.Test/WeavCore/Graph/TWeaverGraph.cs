using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weaver.Core;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Graph;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Test.Common;
using Weaver.Test.Common.Edges;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;
using Weaver.Test.Utils;

namespace Weaver.Test.WeavCore.Graph {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverGraph : WeaverTestBase {

		private WeaverGraph vGraph;
		private Mock<IWeaverPath> vMockPath;
		private IList<IWeaverPathItem> vPathItems;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();

			var schema = new TestSchema();
			var config = new WeaverConfig(ConfigHelper.VertexTypes, ConfigHelper.EdgeTypes);
			var query = new WeaverQuery(); //should mock this, but it would be a pain

			vGraph = new WeaverGraph();
			vPathItems = new List<IWeaverPathItem>();

			vMockPath = new Mock<IWeaverPath>();
			vMockPath.SetupGet(x => x.Config).Returns(config);
			vMockPath.SetupGet(x => x.Query).Returns(query);

			vMockPath
				.Setup(x => x.AddItem(It.IsAny<IWeaverPathItem>()))
				.Callback((IWeaverPathItem x) => vPathItems.Add(x));

			vGraph.Path = vMockPath.Object;
		}
				

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void V() {
			IWeaverAllVertices av = vGraph.V;

			Assert.NotNull(av, "Result should be filled.");
			Assert.AreEqual(1, vPathItems.Count, "Incorrect PathItems.Count.");
			Assert.AreEqual(av, vPathItems[0], "Incorrect Path item.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void E() {
			IWeaverAllEdges ae = vGraph.E;

			Assert.NotNull(ae, "Result should be filled.");
			Assert.AreEqual(1, vPathItems.Count, "Incorrect PathItems.Count.");
			Assert.AreEqual(ae, vPathItems[0], "Incorrect Path item.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddVertex() {
			var person = new Person();
			person.Id = "ABC98-765";
			person.PersonId = 1234;
			person.Name = "Zach K";
			person.Age = 27.1f;
			person.IsMale = true;
			person.Note = null;

			IWeaverQuery q = vGraph.AddVertex(person);

			int bracketI = q.Script.IndexOf('[');
			int bracketIClose = q.Script.LastIndexOf(']');

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.NotNull(q.Script, "Script should be filled.");
			Assert.AreEqual("g.addVertex([",
				q.Script.Substring(0, bracketI+1), "Incorrect starting code.");
			Assert.AreEqual("]);", q.Script.Substring(bracketIClose), "Incorrect ending code.");

			////

			string vals = q.Script.Substring(bracketI+1, bracketIClose-bracketI-1);
			Dictionary<string, string> pairMap = WeaverTestUtil.GetPropListDictionary(vals);

			Assert.AreEqual(4, pairMap.Keys.Count, "Incorrect Key count.");
			Assert.False(pairMap.ContainsKey("Id"), "Should not include Id key.");
			Assert.True(pairMap.ContainsKey(TestSchema.Person_PersonId), "Missing PersonId key.");
			Assert.True(pairMap.ContainsKey(TestSchema.Vertex_Name), "Missing Name key.");
			Assert.True(pairMap.ContainsKey(TestSchema.Person_Age), "Missing Age key.");
			Assert.True(pairMap.ContainsKey(TestSchema.Person_IsMale), "Missing IsMale key.");
			Assert.False(pairMap.ContainsKey(TestSchema.Person_Note), "Incorrect Note key.");

			int i = -1;
			Assert.AreEqual("_P"+(++i), pairMap[TestSchema.Person_PersonId],
				"Incorrect PersonId value.");
			Assert.AreEqual("_P"+(++i), pairMap[TestSchema.Person_IsMale], "Incorrect IsMale value.");
			Assert.AreEqual("_P"+(++i), pairMap[TestSchema.Person_Age], "Incorrect Age value.");
			Assert.AreEqual("_P"+(++i), pairMap[TestSchema.Vertex_Name], "Incorrect Name value.");

			i = -1;
			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P"+(++i), new WeaverQueryVal(person.PersonId));
			expectParams.Add("_P"+(++i), new WeaverQueryVal(person.IsMale));
			expectParams.Add("_P"+(++i), new WeaverQueryVal(person.Age));
			expectParams.Add("_P"+(++i), new WeaverQueryVal(person.Name));

			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddEdge() {
			var plc = new PersonLikesCandy();
			plc.Enjoyment = 0.84f;
			plc.TimesEaten = 54;
			plc.Notes = "Tastes great!";

			const string perId = "a99";
			const string candyId = "x1234";

			var person = new Person { Id = perId };
			var candy = new Candy() { Id = candyId };

			IWeaverQuery q = vGraph.AddEdge(person, plc, candy);

			int bracketI = q.Script.IndexOf('[');
			int bracketIClose = q.Script.LastIndexOf(']');

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.NotNull(q.Script, "Script should be filled.");
			Assert.AreEqual("g.addEdge(g.v(_P0),g.v(_P1),_P2,[", 
				q.Script.Substring(0, bracketI+1), "Incorrect starting code.");
			Assert.AreEqual("]);", q.Script.Substring(bracketIClose), "Incorrect ending code.");

			////

			string vals = q.Script.Substring(bracketI+1, bracketIClose-bracketI-1);
			Dictionary<string, string> pairMap = WeaverTestUtil.GetPropListDictionary(vals);

			Assert.AreEqual(3, pairMap.Keys.Count, "Incorrect Key count.");

			Assert.True(pairMap.ContainsKey(TestSchema.PersonLikesCandy_TimesEaten),
				"Missing TimesEaten key.");
			Assert.True(pairMap.ContainsKey(TestSchema.PersonLikesCandy_Enjoyment),
				"Missing Enjoyment key.");
			Assert.True(pairMap.ContainsKey(TestSchema.PersonLikesCandy_Notes),
				"Missing Notes key.");

			Assert.AreEqual("_P3", pairMap[TestSchema.PersonLikesCandy_TimesEaten],
				"Incorrect TimesEaten value.");
			Assert.AreEqual("_P4", pairMap[TestSchema.PersonLikesCandy_Enjoyment],
				"Incorrect Enjoyment value.");
			Assert.AreEqual("_P5", pairMap[TestSchema.PersonLikesCandy_Notes],
				"Incorrect Notes value.");

			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal(person.Id));
			expectParams.Add("_P1", new WeaverQueryVal(candy.Id));
			expectParams.Add("_P2", new WeaverQueryVal(TestSchema.PersonLikesCandy));
			expectParams.Add("_P3", new WeaverQueryVal(plc.TimesEaten));
			expectParams.Add("_P4", new WeaverQueryVal(plc.Enjoyment));
			expectParams.Add("_P5", new WeaverQueryVal(plc.Notes));
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddEdgeVarNoProps() {
			var rhp = new RootHasPerson();
			const string outVarName = "_var0";
			const string inVarName = "_var1";

			var mockOutVar = new Mock<IWeaverVarAlias>();
			mockOutVar.SetupGet(x => x.Name).Returns(outVarName);
			mockOutVar.SetupGet(x => x.VarType).Returns(typeof(Root));

			var mockInVar = new Mock<IWeaverVarAlias>();
			mockInVar.SetupGet(x => x.Name).Returns(inVarName);
			mockInVar.SetupGet(x => x.VarType).Returns(typeof(Person));

			IWeaverQuery q = vGraph.AddEdge(mockOutVar.Object, rhp, mockInVar.Object);

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.NotNull(q.Script, "Script should be filled.");
			Assert.AreEqual(q.Script, "g.addEdge("+outVarName+","+inVarName+",_P0);",
				"Incorrect Script.");

			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal(TestSchema.RootHasPerson));
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true)]
		[TestCase(false)]
		public void AddEdgeVarInvalidVarType(bool pBadOut) {
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
				vGraph.AddEdge(mockOutVar.Object, mockEdge.Object, mockInVar.Object)
			);

			Assert.AreNotEqual(-1, ex.Message.IndexOf(pBadOut ? " Out " : " In "),
				"Incorrect exception.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddEdgeNoProps() {
			var rhp = new RootHasPerson();
			var root = new Root { Id = "V0" };
			var per = new Person { Id = "eee99" };

			IWeaverQuery q = vGraph.AddEdge(root, rhp, per);

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.NotNull(q.Script, "Script should be filled.");
			Assert.AreEqual("g.addEdge(g.v(_P0),g.v(_P1),_P2);", q.Script, "Incorrect Script.");

			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal(root.Id));
			expectParams.Add("_P1", new WeaverQueryVal(per.Id));
			expectParams.Add("_P2", new WeaverQueryVal(TestSchema.RootHasPerson));
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(null, "x0")]
		[TestCase("x0", null)]
		public void AddEdgeFail(string pPerId, string pCanId) {
			var per = new Person { Id = pPerId };
			var can = new Candy { Id = pCanId };

			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => vGraph.AddEdge(per, new PersonLikesCandy(), can)
			);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString() {
			var g = new WeaverGraph();
			Assert.AreEqual("g", g.BuildParameterizedString(), "Incorrect result.");
		}

	}

}