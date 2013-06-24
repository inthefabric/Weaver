using System;
using NUnit.Framework;
using Weaver.Test.WeavTitan.Common;
using Weaver.Titan.Elements;

namespace Weaver.Test.WeavTitan.Elements {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverTitanPropertyAttribute : WeaverTestBase {



		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void HasTitanVertexCentricIndex() {
			var prop = new WeaverTitanPropertyAttribute("test");
			prop.EdgesForVertexCentricIndexing = new [] { typeof(One), typeof(Two) };

			Assert.True(prop.HasTitanVertexCentricIndex(typeof(One)), "Incorrect result: One.");
			Assert.True(prop.HasTitanVertexCentricIndex(typeof(Two)), "Incorrect result: Two.");
			Assert.False(prop.HasTitanVertexCentricIndex(typeof(Empty)), "Incorrect result: Empty.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void HasTitanVertexCentricIndexNull() {
			var prop = new WeaverTitanPropertyAttribute("test");
			prop.EdgesForVertexCentricIndexing = null;

			Assert.False(prop.HasTitanVertexCentricIndex(typeof(Empty)), "Incorrect result: Empty.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(typeof(bool), "Boolean")]
		[TestCase(typeof(Boolean), "Boolean")]
		[TestCase(typeof(byte), "Byte")]
		[TestCase(typeof(Byte), "Byte")]
		[TestCase(typeof(int), "Integer")]
		[TestCase(typeof(Int32), "Integer")]
		[TestCase(typeof(long), "Long")]
		[TestCase(typeof(Int64), "Long")]
		[TestCase(typeof(DateTime), "Long")]
		[TestCase(typeof(Single), "Float")]
		[TestCase(typeof(float), "Float")]
		[TestCase(typeof(Double), "Double")]
		[TestCase(typeof(double), "Double")]
		[TestCase(typeof(string), "String")]
		[TestCase(typeof(String), "String")]
		public void GetTitanTypeNameStatic(Type pType, string pExpect) {
			Assert.AreEqual(pExpect, WeaverTitanPropertyAttribute.GetTitanTypeName(pType),"Incorrect result.");
		}

	}

}