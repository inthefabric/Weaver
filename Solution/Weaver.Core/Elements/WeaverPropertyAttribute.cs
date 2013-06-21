using System;

namespace Weaver.Core.Elements {

	/*================================================================================================*/
	[AttributeUsage(AttributeTargets.Property)]
	public class WeaverPropertyAttribute : Attribute {

		public string DbName { get; private set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverPropertyAttribute(string DbName) {
			this.DbName = DbName;
		}

	}

}