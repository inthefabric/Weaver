using System.Collections.Generic;
using NUnit.Framework;
using Weaver.Core.Schema;

namespace Weaver.Test.Fixtures.Core {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverInstance : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Basic() {
			var verts = new List<WeaverVertexSchema>();
			var edges = new List<WeaverEdgeSchema>();
			var wi = new Weaver.Core.WeaverInstance(verts, edges);
			var x = wi.Graph.V;
		}

	}

}