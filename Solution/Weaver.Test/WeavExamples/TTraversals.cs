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
using Weaver.Examples.Core.Vertices;

namespace Weaver.Test.WeavExamples {

	/*================================================================================================*/
	[TestFixture]
	public class TTraversals : WeaverTestBase {
	
		//Demonstrating some traversals shown here:
		//https://github.com/thinkaurelius/titan/wiki/Getting-Started


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetSaturn() {
			//saturn = g.V('name','saturn').next()
			
			IWeaverVarAlias<Examples.Core.Vertices.Titan> v;
			
			IWeaverQuery q = Traversals.GetCharacterByName<Examples.Core.Vertices.Titan>(
				"saturn", "saturn", out v);
			
			string expectScript = "saturn=g.V('name',_P0).next();";
			Assert.AreEqual(expectScript, q.Script, "Incorrect script.");
			Assert.AreEqual("saturn", v.Name, "Incorrect Alias.Name.");
			
			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal("saturn"));
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetNameOfGrandchildOfSaturn() {
			//saturn.in('father').in('father').name
			
			var v = new WeaverVarAlias<Character>("saturn");
			
			IWeaverQuery q = Traversals.GetGrandchildOfCharacter(v)
				.Property(x => x.Name)
				.ToQuery();
			
			string expectScript = "saturn.inE('father').outV.inE('father').outV.property('name');";
			Assert.AreEqual(expectScript, q.Script, "Incorrect script.");
		}

	}

}