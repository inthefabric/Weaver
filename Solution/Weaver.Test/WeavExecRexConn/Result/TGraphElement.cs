using System;
using NUnit.Framework;
using ServiceStack.Text;
using Weaver.Exec.RexConnect;
using Weaver.Exec.RexConnect.Result;
using Weaver.Test.Utils;

namespace Weaver.Test.WeavExecRexConn.Result {

	/*================================================================================================*/
	[TestFixture]
	public class TGraphElement : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildVertex() {
			var jo = new JsonObject();
			jo.Add("_id", "1234");
			jo.Add("_type", "vertex");

			var ge = GraphElement.Build(jo);

			Assert.AreEqual("1234", ge.Id, "Incorrect Id.");
			Assert.AreEqual(RexConn.GraphElementType.Vertex, ge.Type, "Incorrect Type.");
			Assert.Null(ge.Label, "Label should be null.");
			Assert.Null(ge.OutVertexId, "OutVertexId should be null.");
			Assert.Null(ge.InVertexId, "InVertexId should be null.");
			Assert.Null(ge.Properties, "Properties should be null.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildEdge() {
			var jo = new JsonObject();
			jo.Add("_id", "4321");
			jo.Add("_type", "edge");
			jo.Add("_label", "Contains");
			jo.Add("_outV", "22");
			jo.Add("_inV", "11");

			var ge = GraphElement.Build(jo);

			Assert.AreEqual("4321", ge.Id, "Incorrect Id.");
			Assert.AreEqual(RexConn.GraphElementType.Edge, ge.Type, "Incorrect Type.");
			Assert.AreEqual("Contains", ge.Label, "Incorrect Id.");
			Assert.AreEqual("22", ge.OutVertexId, "Incorrect OutVertexId.");
			Assert.AreEqual("11", ge.InVertexId, "Incorrect InVertexId.");
			Assert.Null(ge.Properties, "Properties should be null.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildNoId() {
			var jo = new JsonObject();
			jo.Add("_type", "vertex");

			var ge = GraphElement.Build(jo);

			Assert.Null(ge.Id, "Id should be null.");
			Assert.AreEqual(RexConn.GraphElementType.Vertex, ge.Type, "Incorrect Type.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildError() {
			var jo = new JsonObject();
			WeaverTestUtil.CheckThrows<Exception>(true, () => GraphElement.Build(jo));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildProps() {
			const string json =
				@"{
					'_id'=1234,
					'_type'='vertex',
					'_properties'={
						'test'='it worked!',
						'TheNumber'=9999
					}
				}";

			JsonObject jo = JsonObject.Parse(json.Replace("'", "\""));

			var ge = GraphElement.Build(jo);

			Assert.AreEqual("1234", ge.Id, "Incorrect Id.");
			Assert.AreEqual(RexConn.GraphElementType.Vertex, ge.Type, "Incorrect Type.");

			Assert.NotNull(ge.Properties, "Properties should be filled.");
			Assert.AreEqual(2, ge.Properties.Keys.Count, "Incorrect Properties.Keys.Count.");
			Assert.AreEqual("it worked!", ge.Properties["test"], "Incorrect 'test' Property.");
			Assert.AreEqual("9999", ge.Properties["TheNumber"], "Incorrect 'TheNumber' Property.");
		}

	}

}