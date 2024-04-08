/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:27
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace CORBA
{

	public class ContainedPOATie: ContainedPOA
	{
		public IContainedOperations _OperationsDelegate { get; set; }
		private PortableServer.IPOA _poa;

		public ContainedPOATie(IContainedOperations d)
		{
			_OperationsDelegate = d;
		}

		public ContainedPOATie(IContainedOperations d, PortableServer.POA poa)
		{
			_OperationsDelegate = d;
			_poa = poa;
		}

		public override PortableServer.IPOA _DefaultPOA()
		{
			if (_poa != null)
			{
				return _poa;
			}
			return base._DefaultPOA();
		}

		public override CORBA.IContained _This()
		{
			return CORBA.ContainedHelper.Narrow(_ThisObject());
		}

		public override CORBA.IContained _This(CORBA.ORB orb)
		{
			return CORBA.ContainedHelper.Narrow(_ThisObject(orb));
		}

		public override CORBA.DefinitionKind DefKind 
		{
			get
			{
				return _OperationsDelegate.DefKind;
			}
		}
		[IdlName("destroy")]
		public override void Destroy()
		{
			_OperationsDelegate.Destroy();
		}
		[WideChar(false)]
		public override string Id 
		{
			get
			{
				return _OperationsDelegate.Id;
			}

			set
			{
				_OperationsDelegate.Id = value;
			}
		}
		[WideChar(false)]
		public override string Name 
		{
			get
			{
				return _OperationsDelegate.Name;
			}

			set
			{
				_OperationsDelegate.Name = value;
			}
		}
		[WideChar(false)]
		public override string Version 
		{
			get
			{
				return _OperationsDelegate.Version;
			}

			set
			{
				_OperationsDelegate.Version = value;
			}
		}
		public override CORBA.IContainer DefinedIn 
		{
			get
			{
				return _OperationsDelegate.DefinedIn;
			}
		}
		[WideChar(false)]
		public override string AbsoluteName 
		{
			get
			{
				return _OperationsDelegate.AbsoluteName;
			}
		}
		public override CORBA.IRepository ContainingRepository 
		{
			get
			{
				return _OperationsDelegate.ContainingRepository;
			}
		}
		[IdlName("describe")]
		public override CORBA.Contained.Description Describe()
		{
			return _OperationsDelegate.Describe();
		}
		[IdlName("move")]
		public override void Move(CORBA.IContainer newContainer, [WideChar(false)] string newName, [WideChar(false)] string newVersion)
		{
			_OperationsDelegate.Move(newContainer, newName, newVersion);
		}
	}
}
