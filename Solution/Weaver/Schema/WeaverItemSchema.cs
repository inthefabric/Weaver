using System.Collections.Generic;

namespace Weaver.Schema {

	/*================================================================================================*/
	public class WeaverItemSchema {

		public string Name { get; private set; }
		public string DbName { get; private set; }
		public List<WeaverPropSchema> Props { get; private set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverItemSchema(string pName, string pDbName=null) {
			Name = pName;
			DbName = pDbName;
			Props = new List<WeaverPropSchema>();
		}

	}

}