using Weaver.Core.Elements;
using Weaver.Test.Common.EdgeTypes;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;

namespace Weaver.Test.Common.Edges {

	/*================================================================================================*/
	[WeaverEdge(TestSchema.RootHasPerson, typeof(Root), typeof(Person))]
	public class RootHasPerson : WeaverEdge<Root, Has, Person> {

	}

}