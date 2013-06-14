using System;
using System.Linq.Expressions;
using NUnit.Framework;
using Weaver.Test.Common.Vertices;
using Weaver.Titan.Steps.Parameters;

namespace Weaver.Test.WeavTitan.Steps.Parameters {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverParamElastic : WeaverTestBase {



		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			Expression<Func<Person, object>> prop = (x => x.PersonId);
			const WeaverParamElasticOp op = WeaverParamElasticOp.EqualTo;
			object val = 5;

			var pe = new WeaverParamElastic<Person>(prop, op, val);
			
			Assert.AreEqual(prop, pe.Property, "Incorrect Property.");
			Assert.AreEqual(op, pe.Operation, "Incorrect Operation.");
			Assert.AreEqual(val, pe.Value, "Incorrect Value.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetOperationScript() {
			var pe = new WeaverParamElastic<Person>(x => x.PersonId, WeaverParamElasticOp.EqualTo, 5);

			Assert.AreEqual(WeaverParamElastic.EqualToScript, pe.GetOperationScript(),
				"Incorrect result.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase]
		public void GetOperationScriptStatic() {
			string script = WeaverParamElastic.GetOperationScript(WeaverParamElasticOp.Contains);
			Assert.AreEqual(WeaverParamElastic.ContainsScript, script, "Incorrect result.");
		}

	}

}