namespace Weaver.Core.Schema {

	/*================================================================================================*/
	public class WeaverVertexSchema : WeaverItemSchema {

		public WeaverVertexSchema BaseNode { get; set; }
		public bool IsBaseClass { get; set; }
		public bool IsAbstract { get; set; }
		public bool IsRoot { get; set; }
		public bool IsInternal { get; set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverVertexSchema(string pName, string pDbName=null) : base(pName, pDbName) {}

	}

}