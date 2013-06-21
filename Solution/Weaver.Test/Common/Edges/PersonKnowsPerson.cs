using Weaver.Core.Elements;
using Weaver.Test.Common.EdgeTypes;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;

namespace Weaver.Test.Common.Edges {

	/*================================================================================================*/
	[WeaverEdge(TestSchema.PersonKnowsPerson, typeof(Person), typeof(Person))]
	public class PersonKnowsPerson : WeaverEdge<Person, Knows, Person> {

		[WeaverProperty(TestSchema.PersonKnowsPerson_MetOnDate)]
		public string MetOnDate { get; set; }

		[WeaverProperty(TestSchema.PersonKnowsPerson_Amount)]
		public float Amount { get; set; }

	}

}