using System;
using NUnit.Framework;
using Weaver.Titan.Elements;

namespace Weaver.Test.WeavTitan.Elements {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverTitanPropertyAttribute : WeaverTestBase {


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