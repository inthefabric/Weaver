using System;

namespace Fabric.Domain.Graph.Schema {

	/*================================================================================================*/
	public class WeaverPropSchema {

		public string Name { get; private set; }
		public Type Type { get; private set; }

		public bool? IsPrimaryKey { get; set; }
		public bool? IsUnique { get; set; }
		public bool? IsTimestamp { get; set; }
		public bool? IsNullable { get; set; }
		public bool? IsCaseInsensitive { get; set; }
		public int? Len { get; set; }
		public int? LenMin { get; set; }
		public int? LenMax { get; set; }
		public string ValidRegex { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverPropSchema(string pName, Type pType) {
			Name = pName;
			Type = pType;
			IsPrimaryKey = IsUnique = (pName.Substring(pName.Length-2) == "Id");
		}

	}

}