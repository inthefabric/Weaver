using Weaver.Items;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.RelTypes;

namespace Weaver.Test.Common.Rels {

	/*================================================================================================*/
	public class PersonLikesCandy : WeaverRel<Person, Likes, Candy> {

		public int TimesEaten { get; set; }
		public float Enjoyment { get; set; }

	}

}