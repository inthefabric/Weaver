using Weaver.Items;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.RelTypes;

namespace Weaver.Test.Common.Rels {

	/*================================================================================================*/
	public class PersonKnowsPerson : WeaverRel<Person, Knows, Person> {

		public string MetOnDate { get; set; }
		public float Amount { get; set; }

	}

}