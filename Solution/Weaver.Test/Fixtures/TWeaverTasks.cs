﻿using System.Collections.Generic;
using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Interfaces;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.Rels;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverTasks : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		[Test] //TEST: uncomment UpdateNodesAtPath
		public void UpdateNodesAtPath() {
			var person = new Person();
			person.PersonId = 321;
			person.Name = "Zach K";
			person.Age = 27.3f;
			person.IsMale = true;

			var path = new TestPath();
			var end = path.BaseNode
				.OutHasPerson.ToNode
					.Has(p => p.PersonId, WeaverFuncHasOp.EqualTo, 123);

			var updates = new WeaverUpdates<Person>();
			updates.AddUpdate(person, p => p.PersonId);
			updates.AddUpdate(person, p => p.Name);
			updates.AddUpdate(person, p => p.IsMale);
			updates.AddUpdate(person, p => p.Age);

			IWeaverQuery q = WeaverTasks.UpdateNodesAtPath(path, updates);

			string expect = path.Script+
				".each{it.PersonId=321;it.Name=_P0;it.IsMale=true;it.Age=27.3F};";

			var expectParams = new Dictionary<string, string>();
			expectParams.Add("_P0", "'"+person.Name+"'");

			Assert.AreEqual(expect, q.Script, "Incorrect Query.Script.");
			CheckQueryParams(q, expectParams);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BeginPath() {
			var r = new Root();

			IWeaverPath<Root> p = WeaverTasks.BeginPath(r);

			Assert.NotNull(p.Query, "Query should not be null.");
			Assert.AreEqual(r, p.BaseNode, "Incorrect BaseNode.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BeginPathFromIndex() {
			const string name = "Person";
			const int perId = 123;

			IWeaverPath<Person> p =
				WeaverTasks.BeginPath<Person>(name, (per => per.PersonId), perId);

			Assert.NotNull(p.Query, "Query should be filled.");
			Assert.NotNull(p.BaseIndex, "BaseIndex should be filled.");
			Assert.AreEqual(name, p.BaseIndex.IndexName, "Incorrect BaseIndex.IndexName.");
			Assert.AreEqual(perId, p.BaseIndex.Value, "Incorrect BaseIndex.Value.");
			Assert.AreEqual("PersonId", p.BaseIndex.PropertyName, "Incorrect BaseIndex.PropertyName.");
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddNode() {
			var person = new Person();
			person.Id = 98765;
			person.PersonId = 1234;
			person.Name = "Zach K";
			person.Age = 27.1f;
			person.IsMale = true;

			IWeaverQuery q = WeaverTasks.AddNode(person);

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
			Assert.True(pairMap.ContainsKey("PersonId"), "Missing PersonId key.");
			Assert.True(pairMap.ContainsKey("Name"), "Missing Name key.");
			Assert.True(pairMap.ContainsKey("Age"), "Missing Age key.");
			Assert.True(pairMap.ContainsKey("IsMale"), "Missing IsMale key.");

			var expectParams = new Dictionary<string, string>();
			expectParams.Add(pairMap["Name"], "Zach K");
			WeaverTestUtil.CheckQueryParams(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddNodeIndex() {
			const string indexName = "Person";
			IWeaverQuery q = WeaverTasks.AddNodeIndex(indexName);

			const string expect = "g.createManualIndex(_P0,Vertex.class);";
			var expectParams = new Dictionary<string, string>();
			expectParams.Add("_P0", indexName);

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.AreEqual(expect, q.Script, "Incorrect Query.Script.");
			WeaverTestUtil.CheckQueryParams(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddRelIndex() {
			const string indexName = "PersonLikesCandy";
			IWeaverQuery q = WeaverTasks.AddRelIndex(indexName);

			const string expect = "g.createManualIndex(_P0,Edge.class);";
			var expectParams = new Dictionary<string, string>();
			expectParams.Add("_P0", indexName);

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.AreEqual(expect, q.Script, "Incorrect Query.Script.");
			WeaverTestUtil.CheckQueryParams(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddNodeToIndex() {
			const string indexName = "Person";
			const int nodeId = 987;
			const int perId = 123;
			var per = new Person { Id = nodeId, PersonId = perId };

			IWeaverQuery q = WeaverTasks.AddNodeToIndex(indexName, per, p => p.PersonId);

			string expect = "n=g.v(987L);g.idx(_P0).put(_P1,"+perId+",n);";

			var expectParams = new Dictionary<string, string>();
			expectParams.Add("_P0", indexName);
			expectParams.Add("_P1", "PersonId");

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.AreEqual(expect, q.Script, "Incorrect Query.Script.");
			WeaverTestUtil.CheckQueryParams(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddNodeToIndexFail() {
			var per = new Person { Id = -1 };

			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => WeaverTasks.AddNodeToIndex("Test", per, p => p.PersonId)
			);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddRel() {
			var plc = new PersonLikesCandy();
			plc.Enjoyment = 0.84f;
			plc.TimesEaten = 54;
			plc.Notes = "Tastes great!";

			const long perId = 99;
			const long candyId = 1234;

			var person = new Person { Id = perId };
			var candy = new Candy() { Id = candyId };

			IWeaverQuery q = WeaverTasks.AddRel(person, plc, candy);

			int bracketI = q.Script.IndexOf('[');
			int bracketIClose = q.Script.LastIndexOf(']');

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.NotNull(q.Script, "Script should be filled.");
			Assert.AreEqual("f=g.v(99L);t=g.v(1234L);g.addEdge(f,t,_P0,[", 
				q.Script.Substring(0, bracketI+1), "Incorrect starting code.");
			Assert.AreEqual("]);", q.Script.Substring(bracketIClose), "Incorrect ending code.");

			////

			string vals = q.Script.Substring(bracketI+1, bracketIClose-bracketI-1);
			Dictionary<string, string> pairMap = WeaverTestUtil.GetPropListDictionary(vals);

			Assert.AreEqual(3, pairMap.Keys.Count, "Incorrect Key count.");
			Assert.True(pairMap.ContainsKey("Enjoyment"), "Missing Enjoyment key.");
			Assert.True(pairMap.ContainsKey("TimesEaten"), "Missing TimesEaten key.");
			Assert.True(pairMap.ContainsKey("Notes"), "Missing Notes key.");

			Assert.AreEqual("0.84F", pairMap["Enjoyment"], "Incorrect Enjoyment value.");
			Assert.AreEqual("54", pairMap["TimesEaten"], "Incorrect TimesEaten value.");

			var expectParams = new Dictionary<string, string>();
			expectParams.Add("_P0", "PersonLikesCandy");
			expectParams.Add(pairMap["Notes"], plc.Notes);
			WeaverTestUtil.CheckQueryParams(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddRelNoProps() {
			var rhp = new RootHasPerson();
			var root = new Root { Id = 0 };
			var per = new Person { Id = 99 };

			IWeaverQuery q = WeaverTasks.AddRel(root, rhp, per);

			Assert.True(q.IsFinalized, "Incorrect IsFinalized.");
			Assert.NotNull(q.Script, "Script should be filled.");
			Assert.AreEqual(q.Script, "f=g.v(0L);t=g.v(99L);g.addEdge(f,t,_P0);","Incorrect Script.");

			var expectParams = new Dictionary<string, string>();
			expectParams.Add("_P0", "RootHasPerson");
			WeaverTestUtil.CheckQueryParams(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(-1, 0)]
		[TestCase(0, -1)]
		public void AddRelFail(int pPerId, int pCanId) {
			var per = new Person { Id = pPerId };
			var can = new Candy { Id = pCanId };

			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => WeaverTasks.AddRel(per, new PersonLikesCandy(), can)
			);
		}

	}

}