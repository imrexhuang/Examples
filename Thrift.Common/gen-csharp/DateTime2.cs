/**
 * Autogenerated by Thrift Compiler (0.10.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Thrift;
using Thrift.Collections;
using Thrift.Protocol;
using Thrift.Transport;

#if !SILVERLIGHT
[Serializable]
#endif
public partial class DateTime2 : TBase
{
  private long _Value;

  public long Value
  {
    get
    {
      return _Value;
    }
    set
    {
      __isset.@Value = true;
      this._Value = value;
    }
  }


  public Isset __isset;
  #if !SILVERLIGHT
  [Serializable]
  #endif
  public struct Isset {
    public bool @Value;
  }

  public DateTime2() {
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
            if (field.Type == TType.I64) {
              Value = iprot.ReadI64();
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
      TStruct struc = new TStruct("DateTime2");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.@Value) {
        field.Name = "Value";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(Value);
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
    StringBuilder __sb = new StringBuilder("DateTime2(");
    bool __first = true;
    if (__isset.@Value) {
      if(!__first) { __sb.Append(", "); }
      __first = false;
      __sb.Append("Value: ");
      __sb.Append(Value);
    }
    __sb.Append(")");
    return __sb.ToString();
  }

}

