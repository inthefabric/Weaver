using Fabric.Domain.Graph.Items;
using Fabric.Test.Common.Nodes;
using Fabric.Test.Common.RelTypes;

namespace Fabric.Test.Common.Rels {

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