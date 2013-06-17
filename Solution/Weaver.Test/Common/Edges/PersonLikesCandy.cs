using Weaver.Core.Elements;
using Weaver.Test.Common.EdgeTypes;
using Weaver.Test.Common.Vertices;

namespace Weaver.Test.Common.Edges {

	/*================================================================================================*/
	public class PersonLikesCandy : WeaverEdge<PersonLikesCandy, Person, Likes, Candy> {

		[WeaverItemProperty]
		public int TimesEaten { get; set; }

		[WeaverItemProperty]
		public float Enjoyment { get; set; }

		[WeaverItemProperty]
		public string Notes { get; set; }

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Person BuildOutVertex () {
			return new Person();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected override Candy BuildInVertex () {
			return new Candy();
		}
		
	}

}