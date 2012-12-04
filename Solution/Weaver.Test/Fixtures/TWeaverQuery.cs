using System.Collections.Generic;
using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Items;
using Weaver.Test.Common;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.Rels;
using Weaver.Test.Utils;

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
			Dictionary<string, string> pairMap = GetPropListDictionary(vals);

			Assert.AreEqual(4, pairMap.Keys.Count, "Incorrect Key count.");
			Assert.True(pairMap.ContainsKey("PersonId"), "Missing PersonId key.");
			Assert.True(pairMap.ContainsKey("Name"), "Missing Name key.");
			Assert.True(pairMap.ContainsKey("Age"), "Missing Age key.");
			Assert.True(pairMap.ContainsKey("IsMale"), "Missing IsMale key.");

			var expectParams = new Dictionary<string, string>();
			expectParams.Add(pairMap["Name"], "Zach K");
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

			string expect = "n=g.v(987L);g.idx(_P0).put(_P1,"+perId+",n);";

			var expectParams = new Dictionary<string, string>();
			expectParams.Add("_P0", indexName);
			expectParams.Add("_P1", "'PersonId'");

			Assert.AreEqual(expect, q.Script, "Incorrect Query.Script.");
			CheckQueryParams(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddNodeToIndexFail() {
			var per = new Person { Id = -1 };

			WeaverTestUtils.CheckThrows<WeaverException>(true,
				() => WeaverQuery.AddNodeToIndex("Test", per, p => p.PersonId)
			);
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
				".each{it.PersonId=321;it.Name=_P0;it.IsMale=true;it.Age=27.3F};";

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
			plc.Notes = "Tastes great!";

			const long perId = 99;
			const long candyId = 1234;

			var person = new Person { Id = perId };
			var candy = new Candy() { Id = candyId };

			WeaverQuery q = WeaverQuery.AddRel(person, plc, candy);

			int bracketI = q.Script.IndexOf('[');
			int bracketIClose = q.Script.LastIndexOf(']');

			Assert.NotNull(q.Script, "Script should be filled.");
			Assert.AreEqual("f=g.v(99L);t=g.v(1234L);g.addEdge(f,t,_P0,[", 
				q.Script.Substring(0, bracketI+1), "Incorrect starting code.");
			Assert.AreEqual("]);", q.Script.Substring(bracketIClose), "Incorrect ending code.");

			////

			string vals = q.Script.Substring(bracketI+1, bracketIClose-bracketI-1);
			Dictionary<string, string> pairMap = GetPropListDictionary(vals);

			Assert.AreEqual(3, pairMap.Keys.Count, "Incorrect Key count.");
			Assert.True(pairMap.ContainsKey("Enjoyment"), "Missing Enjoyment key.");
			Assert.True(pairMap.ContainsKey("TimesEaten"), "Missing TimesEaten key.");
			Assert.True(pairMap.ContainsKey("Notes"), "Missing Notes key.");

			Assert.AreEqual("0.84F", pairMap["Enjoyment"], "Incorrect Enjoyment value.");
			Assert.AreEqual("54", pairMap["TimesEaten"], "Incorrect TimesEaten value.");

			var expectParams = new Dictionary<string, string>();
			expectParams.Add("_P0", "PersonLikesCandy");
			expectParams.Add(pairMap["Notes"], plc.Notes);
			CheckQueryParams(q, expectParams);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(-1, 0)]
		[TestCase(0, -1)]
		public void AddRelFail(int pPerId, int pCanId) {
			var per = new Person { Id = pPerId };
			var can = new Candy { Id = pCanId };

			WeaverTestUtils.CheckThrows<WeaverException>(true,
				() => WeaverQuery.AddRel(per, new PersonLikesCandy(), can)
			);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true, "Zach")]
		[TestCase(false, "Zach")]
		[TestCase(true, null)]
		public void BuildPropListPerson(bool pIncludeId, string pName) {
			var p = new Person();
			p.Id = 123456789123;
			p.PersonId = 3456789;
			p.Name = pName;
			p.Age = 27.3f;
			p.IsMale = true;

			var q = new WeaverQuery();
			string propList = WeaverQuery.BuildPropList(q, p, pIncludeId);
			Dictionary<string, string> pairMap = GetPropListDictionary(propList);

			int expectCount = 3 + (pIncludeId ? 1 : 0) + (pName != null ? 1 : 0);
			Assert.AreEqual(expectCount, pairMap.Keys.Count, "Incorrect Key count.");

			Assert.True(pairMap.ContainsKey("PersonId"), "Missing PersonId key.");
			Assert.True(pairMap.ContainsKey("Age"), "Missing Age key.");
			Assert.True(pairMap.ContainsKey("IsMale"), "Missing IsMale key.");
			Assert.AreEqual(pIncludeId, pairMap.ContainsKey("Id"), "Incorrect Id key.");
			Assert.AreEqual((pName != null), pairMap.ContainsKey("Name"), "Incorrect Name key.");

			Assert.AreEqual("3456789", pairMap["PersonId"], "Incorrect PersonId value.");
			Assert.AreEqual("27.3F", pairMap["Age"], "Incorrect Age value.");
			Assert.AreEqual("true", pairMap["IsMale"], "Incorrect IsMale value.");

			if ( pIncludeId ) {
				Assert.AreEqual("123456789123L", pairMap["Id"], "Incorrect Id value.");
			}

			if ( pName != null ) {
				Assert.AreEqual("_P0", pairMap["Name"], "Incorrect Name value.");

				var expectParams = new Dictionary<string, string>();
				expectParams.Add("_P0", pName);
				CheckQueryParams(q, expectParams);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Dictionary<string, string> GetPropListDictionary(string pPropList) {
			string[] valPairs = pPropList.Split(',');
			var map = new Dictionary<string, string>();

			foreach ( string pair in valPairs ) {
				string[] parts = pair.Split(':');
				map.Add(parts[0], parts[1]);
			}

			return map;
		}
		
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