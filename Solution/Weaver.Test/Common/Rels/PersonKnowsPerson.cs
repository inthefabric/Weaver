using Fabric.Domain.Graph.Items;
using Fabric.Test.Common.Nodes;
using Fabric.Test.Common.RelTypes;

namespace Fabric.Test.Common.Rels {

	/*================================================================================================*/
	public class PersonKnowsPerson :
					WeaverRel<IQueryPerson, Person, Knows, IQueryPerson, Person>,
																		IQueryPersonKnowsPerson {

		public string MetOnDate { get; set; }
		public float Amount { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public PersonKnowsPerson(WeaverRelConn pConn) : base(pConn) {}

		/*--------------------------------------------------------------------------------------------*/
		//public override string Label { get { return "PersonKnowsPerson"; } }

	}

}