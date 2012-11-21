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

			string gremlin = WeaverQuery.AddNodeGremlin(person);

			Assert.AreEqual(0, gremlin.IndexOf("g.addVertex(["), "Incorrect starting code.");
			Assert.AreEqual(gremlin.Length-3, gremlin.LastIndexOf("]);"), "Incorrect ending code.");

			int startI = gremlin.IndexOf('[')+1;
			int endI = gremlin.IndexOf(']');
			string vals = gremlin.Substring(startI, endI-startI);
			string[] valPairs = vals.Split(',');
			var pairMap = new Dictionary<string, string>();

			foreach ( string pair in valPairs ) {
				string[] parts = pair.Split(':');
				pairMap.Add(parts[0], parts[1]);
			}

			Assert.AreEqual(5, pairMap.Keys.Count, "Incorrect Key count.");

			Assert.True(pairMap.ContainsKey("Id"), "Missing Id key.");
			Assert.True(pairMap.ContainsKey("PersonId"), "Missing PersonId key.");
			Assert.True(pairMap.ContainsKey("Name"), "Missing Name key.");
			Assert.True(pairMap.ContainsKey("Age"), "Missing Age key.");
			Assert.True(pairMap.ContainsKey("IsMale"), "Missing IsMale key.");

			Assert.AreEqual("98765", pairMap["Id"], "Incorrect Id value.");
			Assert.AreEqual("1234", pairMap["PersonId"], "Incorrect PersonId value.");
			Assert.AreEqual("'Zach K'", pairMap["Name"], "Incorrect Name value.");
			Assert.AreEqual("27.1", pairMap["Age"], "Incorrect Age value.");
			Assert.AreEqual("True", pairMap["IsMale"], "Incorrect IsMale value.");
		}

	}

}