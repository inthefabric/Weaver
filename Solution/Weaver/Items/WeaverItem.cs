using System.Collections.Generic;
using Weaver.Exceptions;
using Weaver.Interfaces;

namespace Weaver.Items {

	/*================================================================================================*/
	public abstract class WeaverItem : IWeaverItem {
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual IWeaverPath Path { get; set; }

		/*--------------------------------------------------------------------------------------------*/
		public virtual int PathIndex {
			get {
				if ( Path == null ) {
					throw new WeaverException("Path is null for "+this+".");
				}

				return Path.IndexOfItem(this);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual IWeaverItem PrevPathItem {
			get { return Path.ItemAtIndex(PathIndex-1); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual IWeaverItem NextPathItem {
			get { return Path.ItemAtIndex(PathIndex+1); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual IList<IWeaverItem> PathToThisItem {
			get { return Path.PathToIndex(PathIndex); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual IList<IWeaverItem> PathFromThisItem {
			get { return Path.PathFromIndex(PathIndex); }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual string ItemIdentifier { get { return GetType().Name+""; } }

		/*--------------------------------------------------------------------------------------------*/
		public abstract string BuildParameterizedString();

	}

}