using System.Collections.Generic;
using System.Linq;
using Weaver.Core.Schema;

namespace Weaver.Test.Common.Schema {

	/*================================================================================================*/
	public class TestSchema {

		public const string Person_PersonId = "PerId";
		public const string Person_IsMale = "IsMale";
		public const string Person_Age = "Age";
		public const string Person_Note = "Note";
		public const string Vertex_Name = "Name";
		public const string Candy_CandyId = "CanId";
		public const string Candy_IsChocolate = "IsChoc";
		public const string Candy_Calories = "Calories";

		public const string RootHasPerson = "RHP";
		public const string RootHasCandy = "RHC";
		public const string PersonLikesCandy = "PLC";
		public const string PersonKnowsPerson = "PKP";

		public const string PersonLikesCandy_TimesEaten = "TE";
		public const string PersonLikesCandy_Enjoyment = "Enj";
		public const string PersonLikesCandy_Notes = "No";
		public const string PersonKnowsPerson_MetOnDate = "MOD";
		public const string PersonKnowsPerson_Amount = "Amt";

		public IList<WeaverVertexSchema> Vertices;
		public IList<WeaverEdgeSchema> Edges;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestSchema() {
			Vertices = new List<WeaverVertexSchema>();
			Edges = new List<WeaverEdgeSchema>();

			WeaverPropSchema ps;

			////
			
			var vert = new WeaverVertexSchema("TestVertex", "Tn");
			Vertices.Add(vert);

				ps = new WeaverPropSchema("Id", "Id", typeof(string));
				vert.Props.Add(ps);

				ps = new WeaverPropSchema("Name", Vertex_Name, typeof(string));
				vert.Props.Add(ps);
			
			////
			
			var item = new WeaverVertexSchema("TestItem", "Ti");
			Vertices.Add(item);

				ps = new WeaverPropSchema("Value", "Val", typeof(string));
				item.Props.Add(ps);
			
			////
			
			var root = new WeaverVertexSchema("Root", "Root");
			Vertices.Add(root);

			////

			var per = new WeaverVertexSchema("Person", "Per");
			per.BaseVertex = vert;
			Vertices.Add(per);

				ps = new WeaverPropSchema("PersonId", Person_PersonId, typeof(int));
				per.Props.Add(ps);

				ps = new WeaverPropSchema("IsMale", Person_IsMale, typeof(bool));
				per.Props.Add(ps);

				ps = new WeaverPropSchema("Age", Person_Age, typeof(float));
				per.Props.Add(ps);

				ps = new WeaverPropSchema("Note", Person_Note, typeof(string));
				per.Props.Add(ps);

			////

			var can = new WeaverVertexSchema("Candy", "Can");
			can.BaseVertex = vert;
			Vertices.Add(can);

				ps = new WeaverPropSchema("CandyId", "CanId", typeof(int));
				can.Props.Add(ps);

				ps = new WeaverPropSchema("IsChocolate", Candy_IsChocolate, typeof(bool));
				can.Props.Add(ps);

				ps = new WeaverPropSchema("Calories", Candy_Calories, typeof(int));
				can.Props.Add(ps);

			////

			var rootHasPer = new WeaverEdgeSchema(root, "RootHasPerson", RootHasPerson, "Has", per);
			Edges.Add(rootHasPer);

			////

			var rootHasCan = new WeaverEdgeSchema(root, "RootHasCandy", "RHC", "Has", per);
			Edges.Add(rootHasCan);

			////

			var perKnowsPer = new WeaverEdgeSchema(
				per, "PersonKnowsPerson", PersonKnowsPerson, "Knows", per);
			Edges.Add(perKnowsPer);

				ps = new WeaverPropSchema("MetOnDate", "Met", typeof(string));
				perKnowsPer.Props.Add(ps);

				ps = new WeaverPropSchema("Amount", "Amt", typeof(float));
				perKnowsPer.Props.Add(ps);

			////

			var perLikesCan = new WeaverEdgeSchema(
				root, "PersonLikesCandy", PersonLikesCandy, "Likes", per);
			Edges.Add(perLikesCan);

				ps = new WeaverPropSchema("TimesEaten", PersonLikesCandy_TimesEaten, typeof(int));
				perLikesCan.Props.Add(ps);

				ps = new WeaverPropSchema("Enjoyment", PersonLikesCandy_Enjoyment, typeof(float));
				perLikesCan.Props.Add(ps);

				ps = new WeaverPropSchema("Notes", PersonLikesCandy_Notes, typeof(string));
				perLikesCan.Props.Add(ps);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverVertexSchema GetVertexSchema(string pName) {
			return Vertices.FirstOrDefault(v => v.Name == pName);
		}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverEdgeSchema GetEdgeSchema(string pName) {
			return Edges.FirstOrDefault(v => v.Name == pName);
		}
		
	}

}