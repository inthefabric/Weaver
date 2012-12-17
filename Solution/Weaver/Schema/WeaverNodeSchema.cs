using System.Collections.Generic;

namespace Weaver.Schema {

	/*================================================================================================*/
	public class WeaverNodeSchema {

		public string Name { get; private set; }
		public string Short { get; private set; }
		public WeaverNodeSchema BaseNode { get; set; }
		public bool IsAbstract { get; set; }
		public bool IsRoot { get; set; }
		public bool IsInternal { get; set; }
		public List<WeaverPropSchema> Props { get; private set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverNodeSchema(string pName, string pShort=null) {
			Name = pName;
			Short = pShort;
			Props = new List<WeaverPropSchema>();
		}

	}

}