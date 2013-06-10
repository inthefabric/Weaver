using Weaver.Core.Elements;
using Weaver.Test.Core.Common.EdgeTypes;
using Weaver.Test.Core.Common.Vertices;

namespace Weaver.Test.Core.Common.Edges {

	/*================================================================================================*/
	public class PersonLikesCandy : WeaverEdge<PersonLikesCandy, Person, Likes, Candy> {

		[WeaverItemProperty]
		public int TimesEaten { get; set; }

		[WeaverItemProperty]
		public float Enjoyment { get; set; }

		[WeaverItemProperty]
		public string Notes { get; set; }

	}

}