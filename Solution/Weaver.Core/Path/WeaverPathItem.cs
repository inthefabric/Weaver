﻿using Weaver.Core.Exceptions;

namespace Weaver.Core.Path {

	/*================================================================================================*/
	public abstract class WeaverPathItem : IWeaverPathItem {
		

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
		public virtual string ItemIdentifier { get { return GetType().Name+""; } }

		/*--------------------------------------------------------------------------------------------*/
		public bool SkipDotPrefix { get; protected set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract string BuildParameterizedString();

	}

}