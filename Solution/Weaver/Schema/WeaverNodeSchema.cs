namespace Weaver.Schema {

	/*================================================================================================*/
	public class WeaverNodeSchema : WeaverItemSchema {

		public WeaverNodeSchema BaseNode { get; set; }
		public bool IsBaseClass { get; set; }
		public bool IsAbstract { get; set; }
		public bool IsRoot { get; set; }
		public bool IsInternal { get; set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverNodeSchema(string pName, string pDbName=null) : base(pName, pDbName) {}

	}

}