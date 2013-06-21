using Weaver.Core.Elements;
using Weaver.Test.Common.EdgeTypes;
using Weaver.Test.Common.Schema;
using Weaver.Titan.Elements;

namespace Weaver.Test.WeavTitan.Common {

	/*================================================================================================*/
	[WeaverTitanEdge(TestSchema.PersonKnowsPerson,
		WeaverEdgeConn.OutOne, typeof(TitanPerson),
		WeaverEdgeConn.InZeroOrMore, typeof(TitanPerson)
	)]
	public class TitanPersonKnowsTitanPerson : WeaverEdge<TitanPerson, Knows, TitanPerson> {

		[WeaverTitanProperty(TestSchema.PersonKnowsPerson_MetOnDate)]
		public string MetOnDate { get; set; }

		[WeaverTitanProperty(TestSchema.PersonKnowsPerson_Amount,
			TitanIndex=true, TitanElasticIndex=true)]
		public float Amount { get; set; }

	}

}