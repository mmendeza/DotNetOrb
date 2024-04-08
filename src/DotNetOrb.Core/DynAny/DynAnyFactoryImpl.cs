// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DynamicAny;
using static DynamicAny.DynAny;

namespace DotNetOrb.Core.DynAny
{
    public class DynAnyFactoryImpl : DynAnyFactory
    {
        private ORB orb;
        private ILogger logger;

        public DynAnyFactoryImpl(ORB orb) : base()
        {
            this.orb = orb;
            logger = orb.Configuration.GetLogger(GetType());
        }

        public override IDynAny CreateDynAny(CORBA.Any value)
        {
            try
            {
                var dynAny = CreateDynAnyFromTypeCode(value.Type);
                dynAny.FromAny(value);
                return dynAny;
            }
            catch (InvalidValue iv)
            {
                logger.Error("unable to create DynAny", iv);
            }
            catch (TypeMismatch itc)
            {
                logger.Error("unable to create DynAny", itc);
            }
            throw new InconsistentTypeCode();
        }

        public override IDynAny CreateDynAnyFromTypeCode(CORBA.TypeCode typeCode)
        {
            var type = typeCode.OriginalTypeCode;

            try
            {
                switch (type.Kind)
                {
                    case TCKind.TkNull:
                    case TCKind.TkVoid:
                    case TCKind.TkShort:
                    case TCKind.TkLong:
                    case TCKind.TkUshort:
                    case TCKind.TkUlong:
                    case TCKind.TkFloat:
                    case TCKind.TkDouble:
                    case TCKind.TkBoolean:
                    case TCKind.TkChar:
                    case TCKind.TkOctet:
                    case TCKind.TkAny:
                    case TCKind.TkTypeCode:
                    case TCKind.TkObjref:
                    case TCKind.TkString:
                    case TCKind.TkLonglong:
                    case TCKind.TkUlonglong:
                    case TCKind.TkWchar:
                    case TCKind.TkWstring:
                        {
                            return new DynAny(this, type, orb, logger);
                        }
                    case TCKind.TkFixed:
                        {
                            return new DynFixed(this, type, orb, logger);
                        }
                    case TCKind.TkExcept:
                    case TCKind.TkStruct:
                        {
                            return new DynStruct(this, type, orb, logger);
                        }
                    case TCKind.TkEnum:
                        {
                            return new DynEnum(this, type, orb, logger);
                        }
                    case TCKind.TkArray:
                        {
                            return new DynArray(this, type, orb, logger);
                        }
                    case TCKind.TkSequence:
                        {
                            return new DynSequence(this, type, orb, logger);
                        }
                    case TCKind.TkUnion:
                        {
                            return new DynUnion(this, type, orb, logger);
                        }
                    case TCKind.TkValue:
                        {
                            throw new NoImplement();
                        }
                    default:
                        throw new InconsistentTypeCode();
                }
            }
            catch (TypeMismatch itc)
            {
                logger.Debug("unexpected exception during create_dyn_any_fromtype_code", itc);
                throw new InconsistentTypeCode();
            }
        }

        public override IDynAny CreateDynAnyWithoutTruncation(CORBA.Any value)
        {
            throw new NoImplement();
        }

        public override CORBA.Any[] CreateMultipleAnys(IDynAny[] values)
        {
            throw new NoImplement();
        }

        public override IDynAny[] CreateMultipleDynAnys(CORBA.Any[] values, bool allowTruncate)
        {
            throw new NoImplement();
        }
    }
}
