using System;
using NUnit.Framework;
using Weaver.Core;
using Weaver.Core.Graph;
using Weaver.Core.Pipe;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Test.Utils;
using Weaver.Examples.Core;
using System.Collections.Generic;

namespace Weaver.Test.WeavExamples {

	/*================================================================================================*/
	[TestFixture]
	public class TTraversals : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetSaturn() {
			IWeaverVarAlias<Weaver.Examples.Core.Vertices.Titan> v;
			IWeaverQuery q = Traversals.GetSaturn(out v);
			
			string expectScript = "saturn=g.V('name',_P0).next();";
			Assert.AreEqual(expectScript, q.Script, "Incorrect script.");
			
			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal("saturn"));
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}

	}

}