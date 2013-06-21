using System;
using System.Collections.Generic;
using Weaver.Test.Common.Edges;
using Weaver.Test.Common.Vertices;

namespace Weaver.Test.Common {

	/*================================================================================================*/
	public static class ConfigHelper {

		public static IList<Type> VertexTypes = new List<Type>(new [] {
			typeof(Candy),
			typeof(Root),
			typeof(Person)
		});

		public static IList<Type> EdgeTypes = new List<Type>(new[] {
			typeof(PersonKnowsPerson),
			typeof(PersonLikesCandy),
			typeof(RootHasCandy),
			typeof(RootHasPerson)
		});

	}

}