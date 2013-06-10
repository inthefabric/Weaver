using Weaver.Core.Elements;
using Weaver.Test.Core.Common.EdgeTypes;
using Weaver.Test.Core.Common.Vertices;

namespace Weaver.Test.Core.Common.Edges {

	/*================================================================================================*/
	public class PersonKnowsPerson : WeaverEdge<Person, Knows, Person> {

		[WeaverItemProperty]
		public string MetOnDate { get; set; }

		[WeaverItemProperty]
		public float Amount { get; set; }

	}

}