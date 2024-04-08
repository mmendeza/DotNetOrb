/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 26/02/2024 12:18:44
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace Test.VT
{
	public class _ValueTypesIfzStub: CORBA.Object, IValueTypesIfz
	{
		private new string[] _ids = {"IDL:Test/VT/ValueTypesIfz:1.0"};

		public override string[] _Ids()
		{
			return _ids;
		}

		public static Type _opsType = typeof(IValueTypesIfzOperations);

		[IdlName("GetRecord")]
		public Test.VT.ManagerRecord GetRecord(Test.VT.ManagerRecord record)
		{
			while(true)
			{
				IInputStream inputStream = null;
				IOutputStream outputStream = null;
				try
				{
					outputStream = _Request("GetRecord", true);
					ManagerRecordHelper.Write(outputStream, record);
					inputStream = _Invoke(outputStream);
					Test.VT.ManagerRecord _result;
					_result = ManagerRecordHelper.Read(inputStream);
					return _result;
				}
				catch(RemarshalException)
				{
					continue;
				}
				catch(CORBA.ApplicationException aex)
				{
					try
					{
						switch (aex.Id)
						{
							default:
								throw new RuntimeException("Unexpected exception " + aex.Id);
						}						
					}
					finally
					{
						try
						{
							aex.InputStream.Close();
						}
						catch (Exception ex)
						{
							throw new RuntimeException("Unexpected exception " + ex.ToString());
						}
					}
				}
				finally
				{
					if (outputStream != null)
					{
						try
						{
							outputStream.Close();
						}
						catch (Exception e)
						{
							throw new RuntimeException("Unexpected exception " + e.ToString());
						}
					}
					if (inputStream != null)
					{
						this._ReleaseReply(inputStream);
					}
				}
			}
		}

	}

}
