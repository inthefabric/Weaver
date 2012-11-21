﻿using System;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {

	/*================================================================================================*/
	public class WeaverFuncAs<TItem> : WeaverFunc where TItem : IWeaverItem {

		public Type ItemType { get; private set; }

		private readonly string vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncAs(IWeaverPath pPath) {
			ItemType = typeof(TItem);
			vLabel = "step"+(pPath.Length-1);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string Label {
			get { return vLabel+""; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string GremlinCode {
			get { return "as('"+vLabel+"')"; }
		}

	}

}