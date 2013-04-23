using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.Rels;
using Weaver.Test.Common.Schema;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverTasks : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BeginPath() {
			var r = new Root();

			IWeaverPath<Root> p = WeavInst.BeginPath(r);

			Assert.NotNull(p.Query, "Query should not be null.");
			Assert.AreEqual(r, p.BaseNode, "Incorrect BaseNode.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true)]
		[TestCase(false)]
		public void BeginPathNodeVarAlias(bool pCopyItem) {
			const string varName = "_V0";
			var mockVar = new Mock<IWeaverVarAlias<Person>>();
			mockVar.SetupGet(x => x.Name).Returns(varName);

			IWeaverPathFromVarAlias<Person> p = WeavInst.BeginPath(mockVar.Object, pCopyItem);

			Assert.NotNull(p.Query, "Query should not be null.");
			Assert.AreEqual(mockVar.Object, p.BaseVar, "Incorrect BaseNode.");
			Assert.AreEqual(pCopyItem, p.CopyItemIntoVar, "Incorrect CopyItem.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BeginPathFromKeyIndex() {
			const int perId = 123;

			IWeaverPathFromKeyIndex<Person> p =
				WeavInst.BeginPath<Person>(x => x.PersonId, perId);

			Assert.NotNull(p.Query, "Query should be filled.");
			Assert.NotNull(p.BaseIndex, "BaseIndex should be filled.");
			Assert.AreEqual(TestSchema.Person_PersonId, p.BaseIndex.IndexName,
				"Incorrect BaseIndex.IndexName.");
			Assert.AreEqual(perId, p.BaseIndex.Value, "Incorrect BaseIndex.Value.");
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true)]
		[TestCase(false)]
		public void AddNode(bool pIncludeNulls) {
			var person = new Person();
			person.Id = "ABC98-765";
			person.PersonId = 1234;
			person.Name = "Zach K";
			person.Age = 27.1f;
			person.IsMale = true;
			person.Note = null;

			IWeaverQuery q = WeavInst.AddNode(person, pIncludeNulls);

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

			Assert.AreEqual((pIncludeNulls ? 5 : 4), pairMap.Keys.Count, "Incorrect Key count.");
			Assert.False(pairMap.ContainsKey("Id"), "Should not include Id key.");
			Assert.True(pairMap.ContainsKey(TestSchema.Person_PersonId), "Missing PersonId key.");
			Assert.True(pairMap.ContainsKey(TestSchema.Node_Name), "Missing Name key.");
			Assert.True(pairMap.ContainsKey(TestSchema.Person_Age), "Missing Age key.");
			Assert.True(pairMap.ContainsKey(TestSchema.Person_IsMale), "Missing IsMale key.");
			Assert.AreEqual(pIncludeNulls, pairMap.ContainsKey(TestSchema.Person_Note),
				"Incorrect Note key.");

			int i = -1;
			Assert.AreEqual("_P"+(++i), pairMap[TestSchema.Person_PersonId],
				"Incorrect PersonId value.");
			Assert.AreEqual("_P"+(++i), pairMap[TestSchema.Person_IsMale], "Incorrect IsMale value.");
			Assert.AreEqual("_P"+(++i), pairMap[TestSchema.Person_Age], "Incorrect Age value.");

			if ( pIncludeNulls ) {
				Assert.AreEqual("_P"+(++i), pairMap[TestSchema.Person_Note], "Incorrect Note value.");
			}

			Assert.AreEqual("_P"+(++i), pairMap[TestSchema.Node_Name], "Incorrect Name value.");

			i = -1;
			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P"+(++i), new WeaverQueryVal(person.PersonId));
			expectParams.Add("_P"+(++i), new WeaverQueryVal(person.IsMale));
			expectParams.Add("_P"+(++i), new WeaverQueryVal(person.Age));

			if ( pIncludeNulls ) {
				expectParams.Add("_P"+(++i), new WeaverQueryVal(person.Note));
			}

			expectParams.Add("_P"+(++i), new WeaverQueryVal(person.Name));

			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------* /
		[TestCase(WeavInst.ItemType.Node, "Vertex")]
		[TestCase(WeavInst.ItemType.Rel, "Edge")]
		public void CreateKeyIndex(WeavInst.ItemType pType, string pClass) {
			IWeaverQuery q = WeavInst.CreateKeyIndex<Person>(x => x.PersonId, pType);

			string expect = "g.createKeyIndex(_P0,"+pClass+".class);";
			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal("PersonId"));

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.AreEqual(expect, q.Script, "Incorrect Query.Script.");
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddRel() {
			var plc = new PersonLikesCandy();
			plc.Enjoyment = 0.84f;
			plc.TimesEaten = 54;
			plc.Notes = "Tastes great!";

			const string perId = "a99";
			const string candyId = "x1234";

			var person = new Person { Id = perId };
			var candy = new Candy() { Id = candyId };

			IWeaverQuery q = WeavInst.AddRel(person, plc, candy);

			int bracketI = q.Script.IndexOf('[');
			int bracketIClose = q.Script.LastIndexOf(']');

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.NotNull(q.Script, "Script should be filled.");
			Assert.AreEqual("f=g.v(_P0);t=g.v(_P1);g.addEdge(f,t,_P2,[", 
				q.Script.Substring(0, bracketI+1), "Incorrect starting code.");
			Assert.AreEqual("]);", q.Script.Substring(bracketIClose), "Incorrect ending code.");

			////

			string vals = q.Script.Substring(bracketI+1, bracketIClose-bracketI-1);
			Dictionary<string, string> pairMap = WeaverTestUtil.GetPropListDictionary(vals);

			Assert.AreEqual(3, pairMap.Keys.Count, "Incorrect Key count.");
			Assert.True(pairMap.ContainsKey(TestSchema.Plc_Enjoyment), "Missing Enjoyment key.");
			Assert.True(pairMap.ContainsKey(TestSchema.Plc_TimesEaten), "Missing TimesEaten key.");
			Assert.True(pairMap.ContainsKey(TestSchema.Plc_Notes), "Missing Notes key.");

			Assert.AreEqual("_P3", pairMap[TestSchema.Plc_TimesEaten], "Incorrect TimesEaten value.");
			Assert.AreEqual("_P4", pairMap[TestSchema.Plc_Enjoyment], "Incorrect Enjoyment value.");
			Assert.AreEqual("_P5", pairMap[TestSchema.Plc_Notes], "Incorrect Notes value.");

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
		public void AddRelVarNoProps() {
			var rhp = new RootHasPerson();
			const string fromVarName = "_var0";
			const string toVarName = "_var1";

			var mockFromVar = new Mock<IWeaverVarAlias>();
			mockFromVar.SetupGet(x => x.Name).Returns(fromVarName);
			mockFromVar.SetupGet(x => x.VarType).Returns(typeof(Root));

			var mockToVar = new Mock<IWeaverVarAlias>();
			mockToVar.SetupGet(x => x.Name).Returns(toVarName);
			mockToVar.SetupGet(x => x.VarType).Returns(typeof(Person));

			IWeaverQuery q = WeavInst.AddRel(mockFromVar.Object, rhp, mockToVar.Object);

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.NotNull(q.Script, "Script should be filled.");
			Assert.AreEqual(q.Script, "g.addEdge("+fromVarName+","+toVarName+",_P0);",
				"Incorrect Script.");

			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal(TestSchema.RootHasPerson));
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true)]
		[TestCase(false)]
		public void AddRelVarInvalidVarType(bool pBadFrom) {
			var mockFromVar = new Mock<IWeaverVarAlias>();
			mockFromVar.SetupGet(x => x.VarType).Returns(pBadFrom ? typeof(Root) : typeof(Person));
			
			var mockToVar = new Mock<IWeaverVarAlias>();
			mockFromVar.SetupGet(x => x.VarType).Returns(pBadFrom ? typeof(Candy) : typeof(Root));
			
			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => WeavInst.AddRel(mockFromVar.Object, new PersonLikesCandy(), mockToVar.Object)
			);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddRelNoProps() {
			var rhp = new RootHasPerson();
			var root = new Root { Id = "V0" };
			var per = new Person { Id = "eee99" };

			IWeaverQuery q = WeavInst.AddRel(root, rhp, per);

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.NotNull(q.Script, "Script should be filled.");
			Assert.AreEqual("f=g.v(_P0);t=g.v(_P1);g.addEdge(f,t,_P2);", q.Script, "Incorrect Script.");

			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal(root.Id));
			expectParams.Add("_P1", new WeaverQueryVal(per.Id));
			expectParams.Add("_P2", new WeaverQueryVal(TestSchema.RootHasPerson));
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(null, "x0")]
		[TestCase("x0", null)]
		public void AddRelFail(string pPerId, string pCanId) {
			var per = new Person { Id = pPerId };
			var can = new Candy { Id = pCanId };

			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => WeavInst.AddRel(per, new PersonLikesCandy(), can)
			);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(null, "")]
		[TestCase(0, "")]
		[TestCase(1, "x0")]
		[TestCase(2, "x0,x1")]
		[TestCase(10, "x0,x1,x2,x3,x4,x5,x6,x7,x8,x9")]
		public void InitListVar(int? pItems, string pExpectList) {
			const string name = "_var0";
			IWeaverVarAlias setList;
			
			var mockTx = new Mock<IWeaverTransaction>();
			mockTx.Setup(x => x.GetNextVarName()).Returns(name);

			IWeaverQuery q;

			if ( pItems == null ) {
				q = WeavInst.InitListVar(mockTx.Object, out setList);
			}
			else {
				var list = new List<IWeaverVarAlias>();

				for ( int i = 0 ; i < pItems ; ++i ) {
					var mockVar = new Mock<IWeaverVarAlias>();
					mockVar.SetupGet(x => x.Name).Returns("x"+i);
					list.Add(mockVar.Object);
				}

				q = WeavInst.InitListVar(mockTx.Object, list, out setList);
			}
			
			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.AreEqual(name+"=["+pExpectList+"];", q.Script, "Incorrect Script.");
			Assert.NotNull(setList, "The out WeaverListVar should not be null.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void StoreQueryResultAsVar() {
			const string name = "_var0";
			IWeaverVarAlias varAlias;

			var mockTx = new Mock<IWeaverTransaction>();
			mockTx.Setup(x => x.GetNextVarName()).Returns(name);

			var mockQuery = new Mock<IWeaverQuery>();
			mockQuery.Setup(x => x.StoreResultAsVar(It.Is<IWeaverVarAlias>(a => a.Name == name)));

			IWeaverQuery result = WeavInst.StoreQueryResultAsVar(
				mockTx.Object, mockQuery.Object, out varAlias);

			Assert.AreEqual(mockQuery.Object, result, "Incorrect result.");
			Assert.AreEqual(name, varAlias.Name, "Incorrect VarAlias.Name.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		[Category("Integration")]
		public void PathBasedQuery() {
			IWeaverFuncAs<Person> personAlias;

			IWeaverQuery q = WeavInst.BeginPath(new Root())
				.BaseNode
				.OutHasPerson.ToNode
					.As(out personAlias)
				.InPersonKnows.FromNode
					.Has(p => p.PersonId, WeaverFuncHasOp.GreaterThan, 5)
					.Has(p => p.Name, WeaverFuncHasOp.NotEqualTo, "Hello")
					.Has(p => p.Name, WeaverFuncHasOp.NotEqualTo, "Goodbye")
					.Back(personAlias)
				.OutLikesCandy.ToNode
					.Prop(c => c.Calories)
				.End();

			const string expectScript = "g.v(0)"+
				".outE('"+TestSchema.RootHasPerson+"').inV"+
					".as('step3')"+
				".inE('"+TestSchema.PersonKnowsPerson+"').outV"+
					".has('"+TestSchema.Person_PersonId+"',Tokens.T.gt,_P0)"+
					".has('Name',Tokens.T.neq,_P1)"+
					".has('Name',Tokens.T.neq,_P2)"+
					".back('step3')"+
				".outE('"+TestSchema.PersonLikesCandy+"').inV"+
					".property('"+TestSchema.Candy_Calories+"');";

			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal(5));
			expectParams.Add("_P1", new WeaverQueryVal("Hello"));
			expectParams.Add("_P2", new WeaverQueryVal("Goodbye"));

			Assert.AreEqual(expectScript, q.Script);
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}

	}

}