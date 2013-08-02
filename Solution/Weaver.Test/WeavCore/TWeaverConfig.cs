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

namespace Weaver.Test.WeavCore {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverConfig : WeaverTestBase {

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

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewWithPropFromGenericBase() {
			var verts = new List<Type>();
			var edges = new List<Type>();

			edges.Add(typeof(EdgeB));
			edges.Add(typeof(EdgeC));

			var wc = new WeaverConfig(verts, edges);

			Assert.AreEqual(verts, wc.VertexTypes, "Incorrect Vertex list.");
			Assert.AreEqual(edges, wc.EdgeTypes, "Incorrect Edge list.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewInvalidType() {
			var verts = new List<Type>();
			var edges = new List<Type>();

			verts.Add(typeof(WeaverConfig));

			var ex = WeaverTestUtil.CheckThrows<WeaverException>(
				true, () => new WeaverConfig(verts, edges)
			);

			Assert.AreEqual(0, ex.Message.IndexOf("Type '"), "Incorrect exception.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewDuplicatePropertyDb() {
			var verts = new List<Type>();
			var edges = new List<Type>();

			verts.Add(typeof(VertexA));
			verts.Add(typeof(VertexB));

			var ex = WeaverTestUtil.CheckThrows<WeaverException>(
				true, () => new WeaverConfig(verts, edges)
			);

			Assert.AreEqual(0, ex.Message.IndexOf("Duplicate property"), "Incorrect exception.");
			Assert.AreNotEqual(-1, ex.Message.IndexOf("'test'"), "Incorrect DbName in exception.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetEdgeDbNameItem() {
			string name = vConfig.GetEdgeDbName(new PersonLikesCandy());
			Assert.AreEqual(TestSchema.PersonLikesCandy, name, "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetEdgeDbNameT() {
			string name = vConfig.GetEdgeDbName<PersonLikesCandy>();
			Assert.AreEqual(TestSchema.PersonLikesCandy, name, "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetEdgeDbNameFail() {
			WeaverTestUtil.CheckThrows<KeyNotFoundException>(
				true, () => vConfig.GetEdgeDbName<EdgeA>());
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetPropertyDbName() {
			string name = vConfig.GetPropertyDbName<Person>(x => x.PersonId);
			Assert.AreEqual(TestSchema.Person_PersonId, name, "Incorrect result.");
		}

	}

	
	/*================================================================================================*/
	[WeaverVertex]
	public class VertexA : TestVertex {

		[WeaverProperty("test")]
		public string Test { get; set; }

	}


	/*================================================================================================*/
	[WeaverVertex]
	public class VertexB : TestVertex {

		[WeaverProperty("test")]
		public string Test { get; set; }

	}


	/*================================================================================================*/
	[WeaverEdge("EA", typeof(Person), typeof(Candy))]
	public class EdgeA : WeaverEdge<Person, Knows, Candy> {

	}


	/*================================================================================================*/
	public abstract class EdgeBase<T> : WeaverEdge<Person, Knows, T> where T : IWeaverVertex, new() {

		[WeaverProperty("shared")]
		public string Shared { get; set; }

	}


	/*================================================================================================*/
	[WeaverEdge("EB", typeof(Person), typeof(Person))]
	public class EdgeB : EdgeBase<Person> {

	}


	/*================================================================================================*/
	[WeaverEdge("EC", typeof(Person), typeof(Candy))]
	public class EdgeC : EdgeBase<Candy> {

	}

}