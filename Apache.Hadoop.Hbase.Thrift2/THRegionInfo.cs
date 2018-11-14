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
  public partial class THRegionInfo : TBase
  {
    private byte[] _startKey;
    private byte[] _endKey;
    private bool _offline;
    private bool _split;
    private int _replicaId;

    public long RegionId { get; set; }

    public byte[] TableName { get; set; }

    public byte[] StartKey
    {
      get
      {
        return _startKey;
      }
      set
      {
        __isset.startKey = true;
        this._startKey = value;
      }
    }

    public byte[] EndKey
    {
      get
      {
        return _endKey;
      }
      set
      {
        __isset.endKey = true;
        this._endKey = value;
      }
    }

    public bool Offline
    {
      get
      {
        return _offline;
      }
      set
      {
        __isset.offline = true;
        this._offline = value;
      }
    }

    public bool Split
    {
      get
      {
        return _split;
      }
      set
      {
        __isset.split = true;
        this._split = value;
      }
    }

    public int ReplicaId
    {
      get
      {
        return _replicaId;
      }
      set
      {
        __isset.replicaId = true;
        this._replicaId = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool startKey;
      public bool endKey;
      public bool offline;
      public bool split;
      public bool replicaId;
    }

    public THRegionInfo() {
    }

    public THRegionInfo(long regionId, byte[] tableName) : this() {
      this.RegionId = regionId;
      this.TableName = tableName;
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        bool isset_regionId = false;
        bool isset_tableName = false;
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
              if (field.Type == TType.I64) {
                RegionId = iprot.ReadI64();
                isset_regionId = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                TableName = iprot.ReadBinary();
                isset_tableName = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.String) {
                StartKey = iprot.ReadBinary();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.String) {
                EndKey = iprot.ReadBinary();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.Bool) {
                Offline = iprot.ReadBool();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 6:
              if (field.Type == TType.Bool) {
                Split = iprot.ReadBool();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 7:
              if (field.Type == TType.I32) {
                ReplicaId = iprot.ReadI32();
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
        if (!isset_regionId)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field RegionId not set");
        if (!isset_tableName)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field TableName not set");
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
        TStruct struc = new TStruct("THRegionInfo");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        field.Name = "regionId";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(RegionId);
        oprot.WriteFieldEnd();
        if (TableName == null)
          throw new TProtocolException(TProtocolException.INVALID_DATA, "required field TableName not set");
        field.Name = "tableName";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteBinary(TableName);
        oprot.WriteFieldEnd();
        if (StartKey != null && __isset.startKey) {
          field.Name = "startKey";
          field.Type = TType.String;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteBinary(StartKey);
          oprot.WriteFieldEnd();
        }
        if (EndKey != null && __isset.endKey) {
          field.Name = "endKey";
          field.Type = TType.String;
          field.ID = 4;
          oprot.WriteFieldBegin(field);
          oprot.WriteBinary(EndKey);
          oprot.WriteFieldEnd();
        }
        if (__isset.offline) {
          field.Name = "offline";
          field.Type = TType.Bool;
          field.ID = 5;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(Offline);
          oprot.WriteFieldEnd();
        }
        if (__isset.split) {
          field.Name = "split";
          field.Type = TType.Bool;
          field.ID = 6;
          oprot.WriteFieldBegin(field);
          oprot.WriteBool(Split);
          oprot.WriteFieldEnd();
        }
        if (__isset.replicaId) {
          field.Name = "replicaId";
          field.Type = TType.I32;
          field.ID = 7;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(ReplicaId);
          oprot.WriteFieldEnd();
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("THRegionInfo(");
      __sb.Append(", RegionId: ");
      __sb.Append(RegionId);
      __sb.Append(", TableName: ");
      __sb.Append(TableName);
      if (StartKey != null && __isset.startKey) {
        __sb.Append(", StartKey: ");
        __sb.Append(StartKey);
      }
      if (EndKey != null && __isset.endKey) {
        __sb.Append(", EndKey: ");
        __sb.Append(EndKey);
      }
      if (__isset.offline) {
        __sb.Append(", Offline: ");
        __sb.Append(Offline);
      }
      if (__isset.split) {
        __sb.Append(", Split: ");
        __sb.Append(Split);
      }
      if (__isset.replicaId) {
        __sb.Append(", ReplicaId: ");
        __sb.Append(ReplicaId);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}