using Weaver.Items;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.RelTypes;

namespace Weaver.Test.Common.Rels {

	/*================================================================================================*/
	public class PersonLikesCandy : WeaverRel<Person, Likes, Candy> {

		[WeaverItemProperty]
		public int TimesEaten { get; set; }

		[WeaverItemProperty]
		public float Enjoyment { get; set; }

		[WeaverItemProperty]
		public string Notes { get; set; }

	}

}