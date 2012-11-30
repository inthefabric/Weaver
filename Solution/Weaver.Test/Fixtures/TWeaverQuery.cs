﻿using System.Collections.Generic;
using NUnit.Framework;
using Weaver.Functions;
using Weaver.Items;
using Weaver.Test.Common;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.Rels;

namespace Weaver.Test.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverQuery {


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

			WeaverQuery q = WeaverQuery.AddNode(person);

			int bracketI = q.Script.IndexOf('[');
			int bracketIClose = q.Script.LastIndexOf(']');

			Assert.NotNull(q.Script, "Script should be filled.");
			Assert.AreEqual("g.addVertex([",
				q.Script.Substring(0, bracketI+1), "Incorrect starting code.");
			Assert.AreEqual("]);", q.Script.Substring(bracketIClose), "Incorrect ending code.");

			////
			
			string vals = q.Script.Substring(bracketI+1, bracketIClose-bracketI-1);
			string[] valPairs = vals.Split(',');
			var pairMap = new Dictionary<string, string>();

			foreach ( string pair in valPairs ) {
				string[] parts = pair.Split(':');
				pairMap.Add(parts[0], parts[1]);
			}

			Assert.AreEqual(4, pairMap.Keys.Count, "Incorrect Key count.");
			Assert.True(pairMap.ContainsKey("PersonId"), "Missing PersonId key.");
			Assert.True(pairMap.ContainsKey("Name"), "Missing Name key.");
			Assert.True(pairMap.ContainsKey("Age"), "Missing Age key.");
			Assert.True(pairMap.ContainsKey("IsMale"), "Missing IsMale key.");

			var expectParams = new Dictionary<string, string>();
			expectParams.Add(pairMap["PersonId"], "1234");
			expectParams.Add(pairMap["Name"], "'Zach K'");
			expectParams.Add(pairMap["Age"], "27.1");
			expectParams.Add(pairMap["IsMale"], "true");
			CheckQueryParams(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase("Person")]
		[TestCase("Artifact")]
		public void AddNodeIndex(string pIndexName) {
			WeaverQuery q = WeaverQuery.AddNodeIndex(pIndexName);

			const string expect = "g.createManualIndex(_P0,Vertex.class);";
			var expectParams = new Dictionary<string, string>();
			expectParams.Add("_P0", pIndexName);

			Assert.AreEqual(expect, q.Script, "Incorrect Query.Script.");
			CheckQueryParams(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddNodeToIndex() {
			const string indexName = "Person";
			const int nodeId = 987;
			const int perId = 123;
			var per = new Person { Id = nodeId, PersonId = perId };

			WeaverQuery q = WeaverQuery.AddNodeToIndex(indexName, per, p => p.PersonId);

			string expect = "n=g.v(_P0);g.idx(_P1).put(_P2,"+perId+",n);";

			var expectParams = new Dictionary<string, string>();
			expectParams.Add("_P0", nodeId+"");
			expectParams.Add("_P1", indexName);
			expectParams.Add("_P2", "'PersonId'");

			Assert.AreEqual(expect, q.Script, "Incorrect Query.Script.");
			CheckQueryParams(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
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

			WeaverQuery q = WeaverQuery.UpdateNodesAtPath(path, updates);

			string expect = path.GremlinCode+
				".each{it.PersonId=321;it.Name=_P0;it.IsMale=true;it.Age=27.3};";

			var expectParams = new Dictionary<string, string>();
			expectParams.Add("_P0", "'"+person.Name+"'");

			Assert.AreEqual(expect, q.Script, "Incorrect Query.Script.");
			CheckQueryParams(q, expectParams);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		//TODO: test Rel with no properties
		[Test]
		public void AddRel() {
			var plc = new PersonLikesCandy();
			plc.Enjoyment = 0.84f;
			plc.TimesEaten = 54;

			const int perId = 99;
			const int candyId = 1234;

			var person = new Person { Id = perId };
			var candy = new Candy() { Id = candyId };

			WeaverQuery q = WeaverQuery.AddRel(person, plc, candy);

			int bracketI = q.Script.IndexOf('[');
			int bracketIClose = q.Script.LastIndexOf(']');

			Assert.NotNull(q.Script, "Script should be filled.");
			Assert.AreEqual("f=g.v(_P0);t=g.v(_P1);g.addEdge(f,t,_P2,[", 
				q.Script.Substring(0, bracketI+1), "Incorrect starting code.");
			Assert.AreEqual("]);", q.Script.Substring(bracketIClose), "Incorrect ending code.");

			////

			string vals = q.Script.Substring(bracketI+1, bracketIClose-bracketI-1);
			string[] valPairs = vals.Split(',');
			var pairMap = new Dictionary<string, string>();

			foreach ( string pair in valPairs ) {
				string[] parts = pair.Split(':');
				pairMap.Add(parts[0], parts[1]);
			}

			Assert.AreEqual(2, pairMap.Keys.Count, "Incorrect Key count.");
			Assert.True(pairMap.ContainsKey("Enjoyment"), "Missing Enjoyment key.");
			Assert.True(pairMap.ContainsKey("TimesEaten"), "Missing PersonId key.");

			var expectParams = new Dictionary<string, string>();
			expectParams.Add("_P0", perId+"");
			expectParams.Add("_P1", candyId+"");
			expectParams.Add("_P2", "PersonLikesCandy");
			expectParams.Add(pairMap["Enjoyment"], "0.84");
			expectParams.Add(pairMap["TimesEaten"], "54");
			CheckQueryParams(q, expectParams);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void CheckQueryParams(WeaverQuery pQuery, Dictionary<string, string> pExpectParams) {
			Assert.NotNull(pQuery.Params, "Query.Params should not be null.");
			Assert.AreEqual(pExpectParams.Keys.Count, pQuery.Params.Keys.Count,
				"Incorrect Query.Params count.");

			foreach ( string key in pExpectParams.Keys ) {
				Assert.True(pQuery.Params.ContainsKey(key), "Missing Query.Params["+key+"].");
				Assert.AreEqual(pExpectParams[key], pQuery.Params[key],
					"Incorrect value for Query.Params["+key+"].");
				//Console.WriteLine(key+": "+pQuery.Params[key]);
			}
		}

	}

}