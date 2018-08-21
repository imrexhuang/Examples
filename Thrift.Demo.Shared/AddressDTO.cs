/**
 * Autogenerated by Thrift Compiler (0.10.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Thrift;
using Thrift.Collections;
using Thrift.Protocol;
using Thrift.Transport;

namespace Thrift.Demo.Shared
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class AddressDTO : TBase
  {
    private string _AREA;
    private string _REMARK;

    /// <summary>
    /// 地点编号
    /// </summary>
    public long ADDR_ID { get; set; }

    /// <summary>
    /// 地点名
    /// </summary>
    public string ADDRESS { get; set; }

    /// <summary>
    /// 区域
    /// </summary>
    public string AREA
    {
      get
      {
        return _AREA;
      }
      set
      {
        __isset.AREA = true;
        this._AREA = value;
      }
    }

    /// <summary>
    /// 经度
    /// </summary>
    public double FLONG { get; set; }

    /// <summary>
    /// 纬度
    /// </summary>
    public double LAT { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string REMARK
    {
      get
      {
        return _REMARK;
      }
      set
      {
        __isset.REMARK = true;
        this._REMARK = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool AREA;
      public bool REMARK;
    }

    public AddressDTO() {
    }

    public AddressDTO(long ADDR_ID, string ADDRESS, double FLONG, double LAT) : this() {
      this.ADDR_ID = ADDR_ID;
      this.ADDRESS = ADDRESS;
      this.FLONG = FLONG;
      this.LAT = LAT;
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        bool isset_ADDR_ID = false;
        bool isset_ADDRESS = false;
        bool isset_FLONG = false;
        bool isset_LAT = false;
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
                ADDR_ID = iprot.ReadI64();
                isset_ADDR_ID = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.String) {
                ADDRESS = iprot.ReadString();
                isset_ADDRESS = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 3:
              if (field.Type == TType.String) {
                AREA = iprot.ReadString();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 4:
              if (field.Type == TType.Double) {
                FLONG = iprot.ReadDouble();
                isset_FLONG = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 5:
              if (field.Type == TType.Double) {
                LAT = iprot.ReadDouble();
                isset_LAT = true;
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 6:
              if (field.Type == TType.String) {
                REMARK = iprot.ReadString();
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
        if (!isset_ADDR_ID)
          throw new TProtocolException(TProtocolException.INVALID_DATA);
        if (!isset_ADDRESS)
          throw new TProtocolException(TProtocolException.INVALID_DATA);
        if (!isset_FLONG)
          throw new TProtocolException(TProtocolException.INVALID_DATA);
        if (!isset_LAT)
          throw new TProtocolException(TProtocolException.INVALID_DATA);
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
        TStruct struc = new TStruct("AddressDTO");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        field.Name = "ADDR_ID";
        field.Type = TType.I64;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI64(ADDR_ID);
        oprot.WriteFieldEnd();
        field.Name = "ADDRESS";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(ADDRESS);
        oprot.WriteFieldEnd();
        if (AREA != null && __isset.AREA) {
          field.Name = "AREA";
          field.Type = TType.String;
          field.ID = 3;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(AREA);
          oprot.WriteFieldEnd();
        }
        field.Name = "FLONG";
        field.Type = TType.Double;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(FLONG);
        oprot.WriteFieldEnd();
        field.Name = "LAT";
        field.Type = TType.Double;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(LAT);
        oprot.WriteFieldEnd();
        if (REMARK != null && __isset.REMARK) {
          field.Name = "REMARK";
          field.Type = TType.String;
          field.ID = 6;
          oprot.WriteFieldBegin(field);
          oprot.WriteString(REMARK);
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
      StringBuilder __sb = new StringBuilder("AddressDTO(");
      __sb.Append(", ADDR_ID: ");
      __sb.Append(ADDR_ID);
      __sb.Append(", ADDRESS: ");
      __sb.Append(ADDRESS);
      if (AREA != null && __isset.AREA) {
        __sb.Append(", AREA: ");
        __sb.Append(AREA);
      }
      __sb.Append(", FLONG: ");
      __sb.Append(FLONG);
      __sb.Append(", LAT: ");
      __sb.Append(LAT);
      if (REMARK != null && __isset.REMARK) {
        __sb.Append(", REMARK: ");
        __sb.Append(REMARK);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}