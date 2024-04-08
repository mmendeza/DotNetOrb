// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CORBA
{
    public class AliasTypeCode : ComplexTypeCode
    {

        protected TypeCode contentType;
        public override TypeCode ContentType
        {
            get
            {
                return contentType;
            }
        }

        public AliasTypeCode(string id, string name, TypeCode originalType) : base(TCKind.TkAlias, id, name)
        {
            this.contentType = originalType;
        }

        /// <summary>
        /// return the content type if the argument is an alias, or the argument itself otherwise
        /// </summary>
        public override TypeCode OriginalTypeCode
        {
            get
            {
                TypeCode typeCode = this;
                try
                {
                    while (typeCode.Kind == TCKind.TkAlias || typeCode.Kind == TCKind.TkValueBox)
                    {
                        typeCode = typeCode.ContentType;
                    }
                }
                catch (BadKind)
                {
                    throw new Internal("Should never happen");
                }
                return typeCode;
            }            
        }

        public override TypeCode CompactTypeCode
        {
            get
            {
                // New typecode with same kind, id and a blank name.
                var result = new AliasTypeCode(id, "", contentType.CompactTypeCode);
                return result;
            }            
        }

        public override bool Equal(TypeCode tc)
        {
            try
            {
                if (tc is RecursiveTypeCode rtc)
                {
                    return Equal(rtc.ActualTypeCode);
                }
                if (!base.Equal(tc))
                {
                    return false;
                }                    
                return (contentType.Equal(tc.ContentType));
            }
            catch (Bounds)
            {
                return false;
            }
            catch (BadKind)
            {
                return false;
            }
        }

        public override bool Equivalent(TypeCode tc)
        {
            try
            {
                if (tc is RecursiveTypeCode rtc)
                {
                    return Equivalent(rtc.ActualTypeCode);
                }
                if (tc is AliasTypeCode tcAlias)
                {
                    return Equivalent(tcAlias.ContentType);
                }
                return contentType.Equivalent(tc);
            }
            catch (Bounds)
            {
                return false;
            }
            catch (BadKind)
            {
                return false;
            }
        }

        public override void RemarshalValue(IInputStream input, IOutputStream output)
        {
            try
            {
                (contentType as TypeCode).RemarshalValue(input, output);
            }
            catch (BadKind ex)
            {
                throw new CORBA.Marshal("When processing TypeCode with kind: " + kind + " caught " + ex);
            }
            catch (Bounds ex)
            {
                throw new CORBA.Marshal("When processing TypeCode with kind: " + kind + " caught " + ex);
            }            
        }

        public static TypeCode ReadTypeCode(IInputStream inputStream, IDictionary<int, string> recursiveTCMap, IDictionary<int, TypeCode> repeatedTCMap, int startPosition)
        {
            int size = inputStream.OpenEncapsulation();
            string repositoryID = inputStream.ReadString();
            if (string.IsNullOrEmpty(repositoryID))
            {
                repositoryID = "IDL:";
            }
            TypeCode? result = inputStream.ReadTypeCodeCache(repositoryID, startPosition);
            if (result == null)
            {
                recursiveTCMap.Add(startPosition, repositoryID);
                //result = doReadTypeCodeInternal(in, recursiveTCMap, repeatedTCMap, startPosition, kind, repositoryID);
                string name = inputStream.ReadString();
                if (string.IsNullOrEmpty(name))
                {
                    name = null;
                }
                var originalType = inputStream.ReadTypeCode(recursiveTCMap, repeatedTCMap);
                result = orb.CreateAliasTc(repositoryID, name, originalType);
                recursiveTCMap.Remove(startPosition);
                repeatedTCMap.Add(startPosition, result);
                inputStream.UpdateTypeCodeCache(repositoryID, startPosition, size);
            }
            else
            {
                //Skip remaining typecode
                inputStream.Skip(size - ((inputStream.Pos - startPosition) - 4 - 4));
            }
            inputStream.CloseEncapsulation();
            return result;
        }

        protected override void WriteTypeCodeParameters(IOutputStream outputStream, IDictionary<string, int> recursiveTCMap, IDictionary<TypeCode, int> repeatedTCMap)
        {
            outputStream.WriteString(id);
            outputStream.WriteString(name);
            outputStream.WriteTypeCode(contentType, recursiveTCMap, repeatedTCMap);
        }

    }
}
