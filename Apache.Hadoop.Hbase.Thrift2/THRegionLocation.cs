/**
 * Autogenerated by Thrift Compiler (0.11.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace Apache.Hadoop.Hbase.Thrift2
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class THRegionLocation : TBase
  {

    public TServerName ServerName { get; set; }

    public THRegionInfo RegionInfo { get; set; }

    public THRegionLocation() {
    }

    public THRegionLocation(TServerName serverName, THRegionInfo regionInfo) : this() {
      this.ServerName = serverName;
      this.RegionInfo = regionInfo;
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        bool isset_serverName = false;
        bool isset_regionInfo = false;
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.Struct) {
                ServerName = new TServerName();
                ServerName.Read(iprot);
                isset_serverName = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.Struct) {
                RegionInfo = new THRegionInfo();
                RegionInfo.Read(iprot);
                isset_regionInfo = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
        if (!isset_serverName)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field ServerName not set");
        if (!isset_regionInfo)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field RegionInfo not set");
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public void Write(TProtocol oprot) {
      oprot.IncrementRecursionDepth();
      try
      {
        TStruct struc = new TStruct("THRegionLocation");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (ServerName == null)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field ServerName not set");
        field.Name = "serverName";
        field.Type = TType.Struct;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        ServerName.Write(oprot);
        oprot.WriteFieldEnd();
        if (RegionInfo == null)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field RegionInfo not set");
        field.Name = "regionInfo";
        field.Type = TType.Struct;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        RegionInfo.Write(oprot);
        oprot.WriteFieldEnd();
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("THRegionLocation(");
      __sb.Append(", ServerName: ");
      __sb.Append(ServerName== null ? "<null>" : ServerName.ToString());
      __sb.Append(", RegionInfo: ");
      __sb.Append(RegionInfo== null ? "<null>" : RegionInfo.ToString());
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
