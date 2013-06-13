using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weaver.Core;
using Weaver.Core.Exceptions;
using Weaver.Core.Schema;
using Weaver.Core.Steps;
using Weaver.Test.Core.Common;
using Weaver.Test.Core.Common.Schema;
using Weaver.Test.Core.Common.Vertices;
using Weaver.Test.Core.Utils;

namespace Weaver.Test.Core.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverConfig : WeaverTestBase {

		private WeaverConfig vConfig;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();

			vConfig = new WeaverConfig(Schema.Vertices, Schema.Edges);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			var verts = new List<WeaverVertexSchema>();
			var edges = new List<WeaverEdgeSchema>();
			
			var wc = new WeaverConfig(verts, edges);

			Assert.AreEqual(verts, wc.VertexSchemas, "Incorrect Vertex list.");
			Assert.AreEqual(edges, wc.EdgeSchemas, "Incorrect Edge list.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewItemFail() {
			var verts = new List<WeaverVertexSchema>();
			var edges = new List<WeaverEdgeSchema>();

			var per = new WeaverVertexSchema("Person", "Per");
			verts.Add(per);
			verts.Add(per);

			var ex = WeaverTestUtil.CheckThrows<WeaverException>(
				true, () => new WeaverConfig(verts, edges)
			);

			Assert.AreNotEqual(-1, ex.Message.IndexOf("item with"), "Incorrect exception.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewPropertyFail() {
			var verts = new List<WeaverVertexSchema>();
			var edges = new List<WeaverEdgeSchema>();

			var per = new WeaverVertexSchema("Person", "Per");
			verts.Add(per);

			var ps = new WeaverPropSchema("PersonId", "PerId", typeof(int));
			per.Props.Add(ps);
			per.Props.Add(ps);

			var ex = WeaverTestUtil.CheckThrows<WeaverException>(
				true, () => new WeaverConfig(verts, edges)
			);

			Assert.AreNotEqual(-1, ex.Message.IndexOf("item property"), "Incorrect exception.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetItemDbNameItem() {
			string name = vConfig.GetItemDbName(new Person());
			Assert.AreEqual(TestSchema.Person_Vertex, name, "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetItemDbNameT() {
			string name = vConfig.GetItemDbName<Person>();
			Assert.AreEqual(TestSchema.Person_Vertex, name, "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetItemDbNameName() {
			string name = vConfig.GetItemDbName("Person");
			Assert.AreEqual(TestSchema.Person_Vertex, name, "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetItemDbNameFail() {
			WeaverTestUtil.CheckThrows<KeyNotFoundException>(
				true, () => vConfig.GetItemDbName("x"));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetPropertyDbNameStep() {
			var mockStep = new Mock<IWeaverStep>();
			string name = vConfig.GetPropertyDbName<Person>(mockStep.Object, x => x.PersonId);
			Assert.AreEqual(TestSchema.Person_PersonId, name, "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetPropertyDbNameEx() {
			string name = vConfig.GetPropertyDbName<Person>(x => x.PersonId);
			Assert.AreEqual(TestSchema.Person_PersonId, name, "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetPropertyDbNameName() {
			string name = vConfig.GetPropertyDbName<Person>("PersonId");
			Assert.AreEqual(TestSchema.Person_PersonId, name, "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetPropertyDbNameFail() {
			var ex = WeaverTestUtil.CheckThrows<WeaverException>(
				true, () => vConfig.GetPropertyDbName<Person>("x"));
			Console.WriteLine(ex+"");
			Assert.AreNotEqual(-1, ex.Message.IndexOf("Unknown item"), "Incorrect exception.");
		}

	}

}