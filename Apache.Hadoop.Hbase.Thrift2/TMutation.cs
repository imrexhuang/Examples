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

  /// <summary>
  /// Atomic mutation for the specified row. It can be either Put or Delete.
  /// </summary>
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class TMutation : TBase
  {
    private TPut _put;
    private TDelete _deleteSingle;

    public TPut Put
    {
      get
      {
        return _put;
      }
      set
      {
        __isset.put = true;
        this._put = value;
      }
    }

    public TDelete DeleteSingle
    {
      get
      {
        return _deleteSingle;
      }
      set
      {
        __isset.deleteSingle = true;
        this._deleteSingle = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool put;
      public bool deleteSingle;
    }

    public TMutation() {
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
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
                Put = new TPut();
                Put.Read(iprot);
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.Struct) {
                DeleteSingle = new TDelete();
                DeleteSingle.Read(iprot);
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
        TStruct struc = new TStruct("TMutation");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (Put != null && __isset.put) {
          field.Name = "put";
          field.Type = TType.Struct;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          Put.Write(oprot);
          oprot.WriteFieldEnd();
        }
        if (DeleteSingle != null && __isset.deleteSingle) {
          field.Name = "deleteSingle";
          field.Type = TType.Struct;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          DeleteSingle.Write(oprot);
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
      StringBuilder __sb = new StringBuilder("TMutation(");
      bool __first = true;
      if (Put != null && __isset.put) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Put: ");
        __sb.Append(Put== null ? "<null>" : Put.ToString());
      }
      if (DeleteSingle != null && __isset.deleteSingle) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("DeleteSingle: ");
        __sb.Append(DeleteSingle== null ? "<null>" : DeleteSingle.ToString());
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
