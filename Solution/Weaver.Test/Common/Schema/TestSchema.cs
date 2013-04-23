using System.Collections.Generic;
using Weaver.Schema;

namespace Weaver.Test.Common.Schema {

	/*================================================================================================*/
	public class TestSchema {

		public const string Person_PersonId = "PerId";
		public const string Person_IsMale = "IsMale";
		public const string Person_Age = "Age";
		public const string Person_Note = "Note";
		public const string Plc_TimesEaten = "Te";
		public const string Plc_Enjoyment = "Enj";
		public const string Plc_Notes = "Notes";
		public const string Node_Name = "Name";
		public const string Candy_IsChocolate = "IsChoc";
		public const string Candy_Calories = "Calories";

		public const string RootHasPerson = "RHP";
		public const string PersonLikesCandy = "PLC";
		public const string PersonKnowsPerson = "PKP";

		public IList<WeaverNodeSchema> Nodes;
		public IList<WeaverRelSchema> Rels;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestSchema() {
			Nodes = new List<WeaverNodeSchema>();
			Rels = new List<WeaverRelSchema>();

			WeaverPropSchema ps;

			////
			
			var node = new WeaverNodeSchema("TestNode", "Tn");
			Nodes.Add(node);

				ps = new WeaverPropSchema("Id", "Id", typeof(string));
				node.Props.Add(ps);

				ps = new WeaverPropSchema("Name", Node_Name, typeof(string));
				node.Props.Add(ps);
			
			////
			
			var item = new WeaverNodeSchema("TestItem", "Ti");
			Nodes.Add(item);

				ps = new WeaverPropSchema("Value", "Val", typeof(string));
				item.Props.Add(ps);
			
			////
			
			var root = new WeaverNodeSchema("Root", "Root");
			Nodes.Add(root);

			////

			var per = new WeaverNodeSchema("Person", "Per");
			per.BaseNode = node;
			Nodes.Add(per);

				ps = new WeaverPropSchema("PersonId", Person_PersonId, typeof(int));
				per.Props.Add(ps);

				ps = new WeaverPropSchema("IsMale", Person_IsMale, typeof(bool));
				per.Props.Add(ps);

				ps = new WeaverPropSchema("Age", Person_Age, typeof(float));
				per.Props.Add(ps);

				ps = new WeaverPropSchema("Note", Person_Note, typeof(string));
				per.Props.Add(ps);

			////

			var can = new WeaverNodeSchema("Candy", "Can");
			can.BaseNode = node;
			Nodes.Add(can);

				ps = new WeaverPropSchema("CandyId", "CanId", typeof(int));
				can.Props.Add(ps);

				ps = new WeaverPropSchema("IsChocolate", Candy_IsChocolate, typeof(bool));
				can.Props.Add(ps);

				ps = new WeaverPropSchema("Calories", Candy_Calories, typeof(int));
				can.Props.Add(ps);

			////

			var rootHasPer = new WeaverRelSchema(root, "RootHasPerson", RootHasPerson, "Has", per);
			Rels.Add(rootHasPer);

			////

			var rootHasCan = new WeaverRelSchema(root, "RootHasCandy", "RHC", "Has", per);
			Rels.Add(rootHasCan);

			////

			var perKnowsPer = new WeaverRelSchema(
				per, "PersonKnowsPerson", PersonKnowsPerson, "Knows", per);
			Rels.Add(perKnowsPer);

				ps = new WeaverPropSchema("MetOnDate", "Met", typeof(string));
				perKnowsPer.Props.Add(ps);

				ps = new WeaverPropSchema("Amount", "Amt", typeof(float));
				perKnowsPer.Props.Add(ps);

			////

			var perLikesCan = new WeaverRelSchema(
				root, "PersonLikesCandy", PersonLikesCandy, "Likes", per);
			Rels.Add(perLikesCan);

				ps = new WeaverPropSchema("TimesEaten", Plc_TimesEaten, typeof(int));
				perLikesCan.Props.Add(ps);

				ps = new WeaverPropSchema("Enjoyment", Plc_Enjoyment, typeof(float));
				perLikesCan.Props.Add(ps);

				ps = new WeaverPropSchema("Notes", Plc_Notes, typeof(string));
				perLikesCan.Props.Add(ps);
		}
		
	}

}