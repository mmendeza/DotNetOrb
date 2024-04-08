using CORBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeCode = CORBA.TypeCode;

namespace CORBA
{
    public interface CORBA.Object
    {
        public bool _Is_a(string id);
    }

    public class Object: CORBA.Object
    {
        public bool _Is_a(string id)
        {
            return false;
        }
    }

    public class ORB
    {
        public static ORB Init()
        {
            return new ORB();
        }      
        
        public TypeCode CreateInterfaceTc(string id, string name)
        {
            return new TypeCode();
        }
    }

    public class TypeCode
    {

    }

    public class InputStream
    {
        public CORBA.Object ReadObject()
        {
            return null;
        }
    }

    public class OutputStream
    {
        public void WriteObject(CORBA.Object value)
        {
            
        }
    }

    public class Any
    {
        public void InsertObject(CORBA.Object value, CORBA.TypeCode typeCode)
        {

        }

        public CORBA.Object ExtractObject()
        {
            return null;
        }
    }

    public class BAD_PARAM: Exception
    {
        public BAD_PARAM(String message) : base(message)
        {

        }
    }
}

namespace DotNetOrb.IdlCompiler
{
    public class _ApoloAuthServiceStub
    {


    }

    public interface ApoloAuthService : CORBA.Object
    {

        private static ORB _orb = null;
        private static ORB _Orb
        {
            get
            {
                if (_orb == null)
                {
                    _orb = ORB.Init();
                }
                return _orb;
            }
            
        }

        private static CORBA.TypeCode _type = null;
        public static CORBA.TypeCode _Type
        {
            get
            {
                if (_type == null)
                {
                    _type = _Orb.CreateInterfaceTc(_Id, "ApoloAuthService");
                }
                return _type;
            }
            
        }

        public static String _Id
        {
            get
            {
                return "IDL:ApoloAuthService:1.0";
            }            
        }

        public static void _Insert(Any any, ApoloAuthService value)
        {
            any.InsertObject(value, _Type);
        }

        public static ApoloAuthService _Extract(Any any)
        {
            CORBA.Object obj = any.ExtractObject();
            ApoloAuthService value = _Narrow(obj);
            return value;
        }

        public static ApoloAuthService _Read(InputStream stream)
        {
            return _Narrow(stream.ReadObject());
        }

        public static void _Write(OutputStream os, ApoloAuthService val)
        {            
            os.WriteObject(val);
        }

        public static ApoloAuthService _Narrow(CORBA.Object obj)
        {
            if (obj == null)
            {
                return null;
            }
            else if (obj is ApoloAuthService)
            {
                return (ApoloAuthService)obj;
            }
            else if (obj._Is_a(_Id))
            {
                _ApoloAuthServiceStub result = new _ApoloAuthServiceStub();
                //((CORBA.portable.ObjectImpl)result)._set_delegate(((CORBA.portable.ObjectImpl)obj)._get_delegate());
                return (ApoloAuthService)result;
            }
            throw new BAD_PARAM("Object is not a " + _Id);
        }

        public static ApoloAuthService _UncheckedNarrow(CORBA.Object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj is ApoloAuthService)
            {
                return (ApoloAuthService)obj;
            }
            else
            {
                _ApoloAuthServiceStub result = new _ApoloAuthServiceStub();
                //((CORBA.portable.ObjectImpl)result)._set_delegate(((CORBA.portable.ObjectImpl)obj)._get_delegate());
                return (ApoloAuthService)result;
            }            
        }
    }

}
