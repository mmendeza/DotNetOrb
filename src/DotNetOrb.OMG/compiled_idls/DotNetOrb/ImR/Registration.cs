/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:37
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace DotNetOrb.ImR
{
	public abstract partial class Registration : CORBA.Object, DotNetOrb.ImR.IRegistration
	{
		[IdlName("IllegalHostName")]
		[RepositoryID("IDL:DotNetOrb/ImR/Registration/IllegalHostName:1.0")]
		[Helper(typeof(IllegalHostNameHelper))]
		public partial class IllegalHostName: CORBA.UserException, CORBA.IIDLEntity, IEquatable<DotNetOrb.ImR.Registration.IllegalHostName>
		{
			public IllegalHostName()
			{
			}

			public IllegalHostName(string _msg): base(_msg)
			{
			}

			public IllegalHostName(IllegalHostName other)
			{
				Name = other.Name;
			}

			public IllegalHostName(string name, string _msg = ""): base(_msg)
			{
				this.Name = name;
			}

			public bool Equals(DotNetOrb.ImR.Registration.IllegalHostName? other)
			{
				if (other == null) return false;
				if (!Name.Equals(other.Name)) return false;
				return true;
			}
			[IdlName("name")]
			[WideChar(false)]
			public string Name { get; set; }
		}

		public static class IllegalHostNameHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(IllegalHostNameHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateExceptionTc(DotNetOrb.ImR.Registration.IllegalHostNameHelper.Id(), "IllegalHostName", new CORBA.StructMember[] {new CORBA.StructMember("name", CORBA.ORB.Init().CreateStringTc(0), null), });
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, DotNetOrb.ImR.Registration.IllegalHostName e)
			{
				any.Type = Type();
				Write(any.CreateOutputStream(), e);
			}

			public static DotNetOrb.ImR.Registration.IllegalHostName Extract(CORBA.Any any)
			{
				var inputStream = any.CreateInputStream();
				try
				{
					return Read(inputStream);
				}
				finally
				{
					inputStream.Close();
				}
			}

			public static string Id()
			{
				return "IDL:DotNetOrb/ImR/Registration/IllegalHostName:1.0";
			}

			public static DotNetOrb.ImR.Registration.IllegalHostName Read(CORBA.IInputStream inputStream)
			{
				var id = inputStream.ReadString();
				if (!id.Equals(Id()))
				{
					throw new CORBA.Marshal("Wrong id: " + id);
				}
				var result = new DotNetOrb.ImR.Registration.IllegalHostName();
				result.Name = inputStream.ReadString();
				return result;
			}

			public static void Write(CORBA.IOutputStream outputStream, DotNetOrb.ImR.Registration.IllegalHostName e)
			{
				outputStream.WriteString(Id());
				outputStream.WriteString(e.Name);
			}

		}
		[IdlName("InvalidSSDRef")]
		[RepositoryID("IDL:DotNetOrb/ImR/Registration/InvalidSSDRef:1.0")]
		[Helper(typeof(InvalidSSDRefHelper))]
		public partial class InvalidSSDRef: CORBA.UserException, CORBA.IIDLEntity, IEquatable<DotNetOrb.ImR.Registration.InvalidSSDRef>
		{
			public InvalidSSDRef()
			{
			}

			public InvalidSSDRef(string _msg): base(_msg)
			{
			}

			public InvalidSSDRef(InvalidSSDRef other)
			{
			}

			public bool Equals(DotNetOrb.ImR.Registration.InvalidSSDRef? other)
			{
				if (other == null) return false;
				return true;
			}
		}

		public static class InvalidSSDRefHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(InvalidSSDRefHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateExceptionTc(DotNetOrb.ImR.Registration.InvalidSSDRefHelper.Id(), "InvalidSSDRef", new CORBA.StructMember[] {});
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, DotNetOrb.ImR.Registration.InvalidSSDRef e)
			{
				any.Type = Type();
				Write(any.CreateOutputStream(), e);
			}

			public static DotNetOrb.ImR.Registration.InvalidSSDRef Extract(CORBA.Any any)
			{
				var inputStream = any.CreateInputStream();
				try
				{
					return Read(inputStream);
				}
				finally
				{
					inputStream.Close();
				}
			}

			public static string Id()
			{
				return "IDL:DotNetOrb/ImR/Registration/InvalidSSDRef:1.0";
			}

			public static DotNetOrb.ImR.Registration.InvalidSSDRef Read(CORBA.IInputStream inputStream)
			{
				var id = inputStream.ReadString();
				if (!id.Equals(Id()))
				{
					throw new CORBA.Marshal("Wrong id: " + id);
				}
				var result = new DotNetOrb.ImR.Registration.InvalidSSDRef();
				return result;
			}

			public static void Write(CORBA.IOutputStream outputStream, DotNetOrb.ImR.Registration.InvalidSSDRef e)
			{
				outputStream.WriteString(Id());
			}

		}
		[IdlName("IllegalPOAName")]
		[RepositoryID("IDL:DotNetOrb/ImR/Registration/IllegalPOAName:1.0")]
		[Helper(typeof(IllegalPOANameHelper))]
		public partial class IllegalPOAName: CORBA.UserException, CORBA.IIDLEntity, IEquatable<DotNetOrb.ImR.Registration.IllegalPOAName>
		{
			public IllegalPOAName()
			{
			}

			public IllegalPOAName(string _msg): base(_msg)
			{
			}

			public IllegalPOAName(IllegalPOAName other)
			{
				Name = other.Name;
			}

			public IllegalPOAName(string name, string _msg = ""): base(_msg)
			{
				this.Name = name;
			}

			public bool Equals(DotNetOrb.ImR.Registration.IllegalPOAName? other)
			{
				if (other == null) return false;
				if (!Name.Equals(other.Name)) return false;
				return true;
			}
			[IdlName("name")]
			[WideChar(false)]
			public string Name { get; set; }
		}

		public static class IllegalPOANameHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(IllegalPOANameHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateExceptionTc(DotNetOrb.ImR.Registration.IllegalPOANameHelper.Id(), "IllegalPOAName", new CORBA.StructMember[] {new CORBA.StructMember("name", CORBA.ORB.Init().CreateStringTc(0), null), });
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, DotNetOrb.ImR.Registration.IllegalPOAName e)
			{
				any.Type = Type();
				Write(any.CreateOutputStream(), e);
			}

			public static DotNetOrb.ImR.Registration.IllegalPOAName Extract(CORBA.Any any)
			{
				var inputStream = any.CreateInputStream();
				try
				{
					return Read(inputStream);
				}
				finally
				{
					inputStream.Close();
				}
			}

			public static string Id()
			{
				return "IDL:DotNetOrb/ImR/Registration/IllegalPOAName:1.0";
			}

			public static DotNetOrb.ImR.Registration.IllegalPOAName Read(CORBA.IInputStream inputStream)
			{
				var id = inputStream.ReadString();
				if (!id.Equals(Id()))
				{
					throw new CORBA.Marshal("Wrong id: " + id);
				}
				var result = new DotNetOrb.ImR.Registration.IllegalPOAName();
				result.Name = inputStream.ReadString();
				return result;
			}

			public static void Write(CORBA.IOutputStream outputStream, DotNetOrb.ImR.Registration.IllegalPOAName e)
			{
				outputStream.WriteString(Id());
				outputStream.WriteString(e.Name);
			}

		}
		[IdlName("DuplicatePOAName")]
		[RepositoryID("IDL:DotNetOrb/ImR/Registration/DuplicatePOAName:1.0")]
		[Helper(typeof(DuplicatePOANameHelper))]
		public partial class DuplicatePOAName: CORBA.UserException, CORBA.IIDLEntity, IEquatable<DotNetOrb.ImR.Registration.DuplicatePOAName>
		{
			public DuplicatePOAName()
			{
			}

			public DuplicatePOAName(string _msg): base(_msg)
			{
			}

			public DuplicatePOAName(DuplicatePOAName other)
			{
				Name = other.Name;
			}

			public DuplicatePOAName(string name, string _msg = ""): base(_msg)
			{
				this.Name = name;
			}

			public bool Equals(DotNetOrb.ImR.Registration.DuplicatePOAName? other)
			{
				if (other == null) return false;
				if (!Name.Equals(other.Name)) return false;
				return true;
			}
			[IdlName("name")]
			[WideChar(false)]
			public string Name { get; set; }
		}

		public static class DuplicatePOANameHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(DuplicatePOANameHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateExceptionTc(DotNetOrb.ImR.Registration.DuplicatePOANameHelper.Id(), "DuplicatePOAName", new CORBA.StructMember[] {new CORBA.StructMember("name", CORBA.ORB.Init().CreateStringTc(0), null), });
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, DotNetOrb.ImR.Registration.DuplicatePOAName e)
			{
				any.Type = Type();
				Write(any.CreateOutputStream(), e);
			}

			public static DotNetOrb.ImR.Registration.DuplicatePOAName Extract(CORBA.Any any)
			{
				var inputStream = any.CreateInputStream();
				try
				{
					return Read(inputStream);
				}
				finally
				{
					inputStream.Close();
				}
			}

			public static string Id()
			{
				return "IDL:DotNetOrb/ImR/Registration/DuplicatePOAName:1.0";
			}

			public static DotNetOrb.ImR.Registration.DuplicatePOAName Read(CORBA.IInputStream inputStream)
			{
				var id = inputStream.ReadString();
				if (!id.Equals(Id()))
				{
					throw new CORBA.Marshal("Wrong id: " + id);
				}
				var result = new DotNetOrb.ImR.Registration.DuplicatePOAName();
				result.Name = inputStream.ReadString();
				return result;
			}

			public static void Write(CORBA.IOutputStream outputStream, DotNetOrb.ImR.Registration.DuplicatePOAName e)
			{
				outputStream.WriteString(Id());
				outputStream.WriteString(e.Name);
			}

		}
		[IdlName("register_host")]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.Registration.IllegalHostName))]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.Registration.InvalidSSDRef))]
		public abstract void RegisterHost(DotNetOrb.ImR.HostInfo info);
		[IdlName("set_server_down")]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.UnknownServerName))]
		public abstract void SetServerDown([WideChar(false)] string name);
		[IdlName("register_poa")]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.Registration.IllegalPOAName))]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.Registration.DuplicatePOAName))]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.UnknownServerName))]
		public abstract void RegisterPoa([WideChar(false)] string name, [WideChar(false)] string server, [WideChar(false)] string host, uint port);
		[IdlName("get_imr_info")]
		public abstract DotNetOrb.ImR.ImRInfo GetImrInfo();
	}
}
