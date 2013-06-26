using System;
using System.Collections.Generic;
using NUnit.Framework;
using Weaver.Core;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Test.Common;
using Weaver.Test.Common.EdgeTypes;
using Weaver.Test.Common.Edges;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;
using Weaver.Test.Utils;

namespace Weaver.Test.WeavExecRexConn.Transfer {

	/*================================================================================================*/
	[TestFixture]
	public class TGraphElement : WeaverTestBase {

		private WeaverConfig vConfig;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();
			vConfig = new WeaverConfig(ConfigHelper.VertexTypes, ConfigHelper.EdgeTypes);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			var verts = new List<Type>();
			var edges = new List<Type>();
			
			var wc = new WeaverConfig(verts, edges);

			Assert.AreEqual(verts, wc.VertexTypes, "Incorrect Vertex list.");
			Assert.AreEqual(edges, wc.EdgeTypes, "Incorrect Edge list.");
		}

	}

}