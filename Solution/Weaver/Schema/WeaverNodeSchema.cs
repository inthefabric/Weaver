using System.Collections.Generic;

namespace Fabric.Domain.Graph.Schema {

	/*================================================================================================*/
	public class WeaverNodeSchema {

		public string Name { get; private set; }
		public string Short { get; private set; }
		public WeaverNodeSchema BaseNode { get; set; }
		public bool IsAbstract { get; set; }
		public bool IsRoot { get; set; }
		public List<WeaverPropSchema> Props { get; private set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverNodeSchema(string pName, string pShort) {
			Name = pName;
			Short = pShort;
			Props = new List<WeaverPropSchema>();
		}

	}

}