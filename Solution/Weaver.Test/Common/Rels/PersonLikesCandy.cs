using Weaver.Items;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.RelTypes;

namespace Weaver.Test.Common.Rels {

	/*================================================================================================*/
	public class PersonLikesCandy :
						WeaverRel<IQueryPerson, Person, Likes, IQueryCandy, Candy>,
																		IQueryPersonLikesCandy {

		public int TimesEaten { get; set; }
		public float Enjoyment { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public PersonLikesCandy(WeaverRelConn pConn) : base(pConn) {}

		/*--------------------------------------------------------------------------------------------*/
		//public override string Label { get { return "PersonLikesCandy"; } }

	}

}