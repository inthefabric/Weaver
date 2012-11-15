using Weaver.Items;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.RelTypes;

namespace Weaver.Test.Common.Rels {

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