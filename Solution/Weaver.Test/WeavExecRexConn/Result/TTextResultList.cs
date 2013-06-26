using System.Collections.Generic;
using NUnit.Framework;
using Weaver.Exec.RexConnect.Result;

namespace Weaver.Test.WeavExecRexConn.Result {

	/*================================================================================================*/
	[TestFixture]
	public class TTextResultList : WeaverTestBase {

		private IList<string> vValues;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void  SetUp() {
			base.SetUp();

			vValues = new List<string>();
			vValues.Add("12");
			vValues.Add("1234");
			vValues.Add("987.654");
			vValues.Add("testing");
			vValues.Add("true");
			vValues.Add("TRUE");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			var trl = new TextResultList(vValues);
			Assert.AreEqual(vValues, trl.Values, "Incorrect Values.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(2, "987.654")]
		[TestCase(3, "testing")]
		public void ToString(int pIndex, string pExpect) {
			var trl = new TextResultList(vValues);
			Assert.AreEqual(pExpect, trl.ToString(pIndex), "Incorrect Value["+pIndex+"].");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(4, true)]
		[TestCase(5, true)]
		public void ToBool(int pIndex, bool pExpect) {
			var trl = new TextResultList(vValues);
			Assert.AreEqual(pExpect, trl.ToBool(pIndex), "Incorrect Value["+pIndex+"].");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(0, 12)]
		public void ToByte(int pIndex, byte pExpect) {
			var trl = new TextResultList(vValues);
			Assert.AreEqual(pExpect, trl.ToByte(pIndex), "Incorrect Value["+pIndex+"].");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(0, 12)]
		public void ToShort(int pIndex, short pExpect) {
			var trl = new TextResultList(vValues);
			Assert.AreEqual(pExpect, trl.ToShort(pIndex), "Incorrect Value["+pIndex+"].");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(0, 12)]
		[TestCase(1, 1234)]
		public void ToInt(int pIndex, int pExpect) {
			var trl = new TextResultList(vValues);
			Assert.AreEqual(pExpect, trl.ToInt(pIndex), "Incorrect Value["+pIndex+"].");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(0, 12)]
		[TestCase(1, 1234)]
		public void ToLong(int pIndex, long pExpect) {
			var trl = new TextResultList(vValues);
			Assert.AreEqual(pExpect, trl.ToLong(pIndex), "Incorrect Value["+pIndex+"].");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(0, 12)]
		[TestCase(1, 1234)]
		[TestCase(2, 987.654f)]
		public void ToFloat(int pIndex, float pExpect) {
			var trl = new TextResultList(vValues);
			Assert.AreEqual(pExpect, trl.ToFloat(pIndex), "Incorrect Value["+pIndex+"].");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(0, 12)]
		[TestCase(1, 1234)]
		[TestCase(2, 987.654)]
		public void ToDouble(int pIndex, double pExpect) {
			var trl = new TextResultList(vValues);
			Assert.AreEqual(pExpect, trl.ToDouble(pIndex), "Incorrect Value["+pIndex+"].");
		}

	}

}