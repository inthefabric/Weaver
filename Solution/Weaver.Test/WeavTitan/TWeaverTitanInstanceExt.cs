using System.Collections.Generic;
using NUnit.Framework;
using Weaver.Core;
using Weaver.Core.Schema;
using Weaver.Titan;
using Weaver.Titan.Graph;

namespace Weaver.Test.WeavTitan {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverTitanInstanceExt : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void TitanGraph() {
			var wi = new WeaverInstance(new List<WeaverVertexSchema>(), new List<WeaverEdgeSchema>());
			WeaverTitanGraph g = (WeaverTitanGraph)wi.TitanGraph();

			Assert.NotNull(g, "Graph should be filled.");
			Assert.NotNull(g.Path, "Graph.Path should be filled.");
			Assert.NotNull(g.Path.Config, "Graph.Path.Config should be filled.");
			Assert.NotNull(g.Path.Query, "Graph.Path.Query should be filled.");

			Assert.AreEqual(1, g.Path.Length, "Incorrect Path length.");
			Assert.AreEqual(g, g.Path.ItemAtIndex(0), "Incorrect Path item.");
		}

	}

}