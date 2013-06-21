using Weaver.Core.Elements;
using Weaver.Test.Common.EdgeTypes;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;

namespace Weaver.Test.Common.Edges {

	/*================================================================================================*/
	[WeaverEdge(TestSchema.PersonLikesCandy, typeof(Person), typeof(Candy))]
	public class PersonLikesCandy : WeaverEdge<Person, Likes, Candy> {

		[WeaverProperty(TestSchema.PersonLikesCandy_TimesEaten)]
		public int TimesEaten { get; set; }

		[WeaverProperty(TestSchema.PersonLikesCandy_Enjoyment)]
		public float Enjoyment { get; set; }

		[WeaverProperty(TestSchema.PersonLikesCandy_Notes)]
		public string Notes { get; set; }

	}

}