using Weaver.Core.Elements;
using Weaver.Test.Common.EdgeTypes;
using Weaver.Test.Common.Vertices;

namespace Weaver.Test.Common.Edges {

	/*================================================================================================*/
	public class PersonKnowsPerson : WeaverEdge<Person, Knows, Person> {

		[WeaverItemProperty]
		public string MetOnDate { get; set; }

		[WeaverItemProperty]
		public float Amount { get; set; }

	}

}