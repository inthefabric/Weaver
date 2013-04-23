using System;
using Weaver.Exceptions;
using Weaver.Interfaces;

namespace Weaver.Items {
	
	/*================================================================================================*/
	public enum WeaverRelConn {

		OutToOne = 1,
		OutToOneOrMore,
		OutToZeroOrMore,
		OutToZeroOrOne,

		InFromOne,
		InFromOneOrMore,
		InFromZeroOrMore,
		InFromZeroOrOne

	}

	/*================================================================================================*/
	public abstract class WeaverRel<TFrom, TType, TTo> : WeaverItem, IWeaverRel<TFrom, TTo>
																	where TFrom : IWeaverNode, new()
																	where TType : IWeaverRelType, new()
																	where TTo : IWeaverNode, new() {

		public string Id { get; set; }
		public bool IsFromManyNodes { get; private set; }
		public bool IsToManyNodes { get; private set; }
		public bool IsOutgoing { get; private set; }

		private WeaverRelConn? vConn;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected WeaverRel() {}

		/*--------------------------------------------------------------------------------------------*/
		protected WeaverRel(WeaverRelConn pConnection) : this() {
			Connection = pConnection;
		}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverRelConn Connection {
			get {
				if ( vConn == null ) {
					throw new WeaverRelException(this, "Connection has not been set.");
				}

				return (WeaverRelConn)vConn;
			}
			set {
				if ( vConn != null ) {
					throw new WeaverRelException(this, "Connection has already been set.");
				}

				vConn = value;
				
				IsFromManyNodes = (
					vConn == WeaverRelConn.InFromZeroOrMore || 
					vConn == WeaverRelConn.InFromOneOrMore
				);
				
				IsToManyNodes = (
					vConn == WeaverRelConn.OutToZeroOrMore ||
					vConn == WeaverRelConn.OutToOneOrMore
				);
				
				IsOutgoing = WeaverRel.IsConnOutgoing((WeaverRelConn)vConn);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TFrom FromNode {
			get {
				var n = new TFrom { IsFromNode = true, ExpectOneNode = !IsFromManyNodes };
				Path.AddItem(n);
				return n;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public TTo ToNode {
			get {
				var n = new TTo { IsFromNode = false, ExpectOneNode = !IsToManyNodes };
				Path.AddItem(n);
				return n;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public Type FromNodeType { get { return typeof(TFrom); } }
		public Type ToNodeType { get { return typeof(TTo); } }

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverRelType RelType {
			get {
				return new TType();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual string Label {
			get { return typeof(TFrom).Name+typeof(TType).Name+typeof(TTo).Name; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string ItemIdentifier { get { return Label+"(Id='"+Id+"')"; } }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return (IsOutgoing ? "out" : "in")+"E('"+Path.Config.GetItemDbName(Label)+"')";
		}

	}

	/*================================================================================================*/
	public static class WeaverRel {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static bool IsConnOutgoing(WeaverRelConn pConn) {
			return (
				pConn == WeaverRelConn.OutToOne || 
				pConn == WeaverRelConn.OutToOneOrMore ||
				pConn == WeaverRelConn.OutToZeroOrMore ||
				pConn == WeaverRelConn.OutToZeroOrOne
			);
		}

	}

}