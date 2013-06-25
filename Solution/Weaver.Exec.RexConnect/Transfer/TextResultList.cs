using System.Collections.Generic;

namespace Weaver.Exec.RexConnect.Transfer {

	/*================================================================================================*/
	public class TextResultList {

		public virtual IList<string> Values { get; private set; }
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TextResultList(IList<string> pValues) {
			Values = pValues;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual string ToString(int pIndex) {
			return Values[pIndex];
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual byte ToByte(int pIndex) {
			return byte.Parse(ToString(pIndex));
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual short ToShort(int pIndex) {
			return short.Parse(ToString(pIndex));
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual int ToInt(int pIndex) {
			return int.Parse(ToString(pIndex));
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual long ToLong(int pIndex) {
			return long.Parse(ToString(pIndex));
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual float ToFloat(int pIndex) {
			return float.Parse(ToString(pIndex));
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual double ToDouble(int pIndex) {
			return double.Parse(ToString(pIndex));
		}

	}

}