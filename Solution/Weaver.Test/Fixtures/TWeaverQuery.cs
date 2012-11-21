using System.Collections.Generic;
using NUnit.Framework;
using Weaver.Test.Common.Nodes;

namespace Weaver.Test.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverQuery {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddNodeGremlin() {
			var person = new Person();
			person.Id = 98765;
			person.PersonId = 1234;
			person.Name = "Zach K";
			person.Age = 27.1f;
			person.IsMale = true;

			WeaverQuery q = WeaverQuery.AddNodeGremlin(person);

			Assert.NotNull(q.Script, "Script should be filled.");
			Assert.AreEqual(0, q.Script.IndexOf("g.addVertex(["), "Incorrect starting code.");
			Assert.AreEqual(q.Script.Length-3, q.Script.LastIndexOf("]);"), "Incorrect ending code.");

			int startI = q.Script.IndexOf('[')+1;
			int endI = q.Script.IndexOf(']');
			string vals = q.Script.Substring(startI, endI-startI);
			string[] valPairs = vals.Split(',');
			var pairMap = new Dictionary<string, string>();

			foreach ( string pair in valPairs ) {
				string[] parts = pair.Split(':');
				pairMap.Add(parts[0], parts[1]);
			}

			Dictionary<string, string> qp = q.Params;
			Assert.NotNull(qp, "Query.Params should not be null.");
			Assert.AreEqual(5, qp.Keys.Count, "Incorrect Query.Params count.");
			Assert.AreEqual(5, pairMap.Keys.Count, "Incorrect Key count.");

			Assert.True(pairMap.ContainsKey("Id"), "Missing Id key.");
			Assert.True(pairMap.ContainsKey("PersonId"), "Missing PersonId key.");
			Assert.True(pairMap.ContainsKey("Name"), "Missing Name key.");
			Assert.True(pairMap.ContainsKey("Age"), "Missing Age key.");
			Assert.True(pairMap.ContainsKey("IsMale"), "Missing IsMale key.");

			Assert.AreEqual("98765", qp[pairMap["Id"]], "Incorrect Id value.");
			Assert.AreEqual("1234", qp[pairMap["PersonId"]], "Incorrect PersonId value.");
			Assert.AreEqual("'Zach K'", qp[pairMap["Name"]], "Incorrect Name value.");
			Assert.AreEqual("27.1", qp[pairMap["Age"]], "Incorrect Age value.");
			Assert.AreEqual("True", qp[pairMap["IsMale"]], "Incorrect IsMale value.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase("Person")]
		[TestCase("Artifact")]
		public void AddNodeIndex(string pIndexName) {
			WeaverQuery q = WeaverQuery.AddNodeIndex(pIndexName);

			const string expect = "g.createManualIndex(P0, Vertex.class);";
			var expectParams = new Dictionary<string, string>();
			expectParams.Add("P0", pIndexName);

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

			const string expect = "n = g.v(P0);g.idx(P1).put(P2,P3,n);";

			var expectParams = new Dictionary<string, string>();
			expectParams.Add("P0", nodeId+"");
			expectParams.Add("P1", "'"+indexName+"'");
			expectParams.Add("P2", "'PersonId'");
			expectParams.Add("P3", perId+"");

			Assert.AreEqual(expect, q.Script, "Incorrect Query.Script.");
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