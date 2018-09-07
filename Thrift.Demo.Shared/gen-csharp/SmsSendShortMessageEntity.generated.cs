//------------------------------------------------------------------------------
// <copyright file="SmsSendShortMessageEntity.generated.cs">
//    Copyright (c) 2018, All rights reserved.
// </copyright>
// <author>YuanRui</author>
// <date>2018-08-24 16:40:34</date>
// <auto-generated>
//    This code was generated by Banana.AutoCode.exe
//    Template Version:20180425
//    Runtime Version:4.0.30319.42000
//
//    Changes to this file may cause incorrect behavior and will be lost if
//    the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Banana.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 短信通知发送信息
    /// </summary>
    public partial class SmsSendShortMessageEntity : ICloneable
    {
	
        /// <summary>
        /// get or set 编号
        /// </summary>
        public virtual String ID { get; set; }
	
        /// <summary>
        /// get or set 短信分类
        /// </summary>
        public virtual Int32 CATEGORY { get; set; }
	
        /// <summary>
        /// get or set 状态
        /// </summary>
        public virtual Int32 STATUS { get; set; }
	
        /// <summary>
        /// get or set 手机号码
        /// </summary>
        public virtual String PHONE { get; set; }
	
        /// <summary>
        /// get or set 短信内容
        /// </summary>
        public virtual String CONTENT { get; set; }
	
        /// <summary>
        /// get or set 通知时间
        /// </summary>
        public virtual DateTime SENT_AT { get; set; }
	
        /// <summary>
        /// get or set 短信发送执行结果
        /// </summary>
        public virtual String RESULT { get; set; }
	
        /// <summary>
        /// get or set 联系人(车主或管理员)
        /// </summary>
        public virtual String CONTACT { get; set; }
	
        /// <summary>
        /// get or set 短信设备编号
        /// </summary>
        public virtual String MODEM_ID { get; set; }
	
        /// <summary>
        /// get or set 发送次数
        /// </summary>
        public virtual Int32 SENT_COUNT { get; set; }
	
        /// <summary>
        /// get or set 车辆号牌
        /// </summary>
        public virtual String PLATE_NUMBER { get; set; }
	
        /// <summary>
        /// get or set 车辆号牌颜色
        /// </summary>
        public virtual String PLATE_COLOR { get; set; }
	
        /// <summary>
        /// get or set 违法类型
        /// </summary>
        public virtual String PECCANCY_TYPE { get; set; }
	
        /// <summary>
        /// get or set 路口设备编号
        /// </summary>
        public virtual String TERMINAL_CODE { get; set; }
	
        /// <summary>
        /// get or set 事件编号
        /// </summary>
        public virtual String EVENT_CODE { get; set; }
	
        /// <summary>
        /// get or set 文件路径
        /// </summary>
        public virtual String FILE_PATH { get; set; }
	
        /// <summary>
        /// get or set 请求时间
        /// </summary>
        public virtual DateTime REQUEST_AT { get; set; }
	
        /// <summary>
        /// get or set 创建时间
        /// </summary>
        public virtual DateTime CREATED_AT { get; set; }
	
        /// <summary>
        /// get or set 备注 接口返回的原始xml
        /// </summary>
        public virtual String REMARK { get; set; }
	
        /// <summary>
        /// get or set 图片信息 base64字符串
        /// </summary>
        public virtual String IMG_INFO { get; set; }
	
        /// <summary>
        /// get or set 实体编号 接口返回的内部序列号 serialNumber
        /// </summary>
        public virtual String ENTITY_ID { get; set; }
	
        /// <summary>
        /// get or set 短信条数 扣除短信条数
        /// </summary>
        public virtual Int32 SMS_COUNT { get; set; }
	
        /// <summary>
        /// get or set 剩余条数 当前短信剩余条数
        /// </summary>
        public virtual Int32 SURPLUS_COUNT { get; set; }
	
        /// <summary>
        /// get or set 短信发送提交状态 0:未提交 1:已提交
        /// </summary>
        public virtual Boolean SEND_STATE { get; set; }
	
        /// <summary>
        /// get or set 创建者(采集人员)
        /// </summary>
        public virtual String CREATED_BY { get; set; }
	
        public virtual SmsSendShortMessageEntity CloneFrom(SmsSendShortMessageEntity thatObj)
        {
            if (thatObj == null)
            {
                throw new ArgumentNullException("thatObj");
            }

	            this.ID = thatObj.ID;
	            this.CATEGORY = thatObj.CATEGORY;
	            this.STATUS = thatObj.STATUS;
	            this.PHONE = thatObj.PHONE;
	            this.CONTENT = thatObj.CONTENT;
	            this.SENT_AT = thatObj.SENT_AT;
	            this.RESULT = thatObj.RESULT;
	            this.CONTACT = thatObj.CONTACT;
	            this.MODEM_ID = thatObj.MODEM_ID;
	            this.SENT_COUNT = thatObj.SENT_COUNT;
	            this.PLATE_NUMBER = thatObj.PLATE_NUMBER;
	            this.PLATE_COLOR = thatObj.PLATE_COLOR;
	            this.PECCANCY_TYPE = thatObj.PECCANCY_TYPE;
	            this.TERMINAL_CODE = thatObj.TERMINAL_CODE;
	            this.EVENT_CODE = thatObj.EVENT_CODE;
	            this.FILE_PATH = thatObj.FILE_PATH;
	            this.REQUEST_AT = thatObj.REQUEST_AT;
	            this.CREATED_AT = thatObj.CREATED_AT;
	            this.REMARK = thatObj.REMARK;
	            this.IMG_INFO = thatObj.IMG_INFO;
	            this.ENTITY_ID = thatObj.ENTITY_ID;
	            this.SMS_COUNT = thatObj.SMS_COUNT;
	            this.SURPLUS_COUNT = thatObj.SURPLUS_COUNT;
	            this.SEND_STATE = thatObj.SEND_STATE;
	            this.CREATED_BY = thatObj.CREATED_BY;
	
            return this;
        }

        public virtual SmsSendShortMessageEntity CloneTo(SmsSendShortMessageEntity thatObj)
        {
            if (thatObj == null)
            {
                throw new ArgumentNullException("thatObj");
            }

	            thatObj.ID = this.ID;
	            thatObj.CATEGORY = this.CATEGORY;
	            thatObj.STATUS = this.STATUS;
	            thatObj.PHONE = this.PHONE;
	            thatObj.CONTENT = this.CONTENT;
	            thatObj.SENT_AT = this.SENT_AT;
	            thatObj.RESULT = this.RESULT;
	            thatObj.CONTACT = this.CONTACT;
	            thatObj.MODEM_ID = this.MODEM_ID;
	            thatObj.SENT_COUNT = this.SENT_COUNT;
	            thatObj.PLATE_NUMBER = this.PLATE_NUMBER;
	            thatObj.PLATE_COLOR = this.PLATE_COLOR;
	            thatObj.PECCANCY_TYPE = this.PECCANCY_TYPE;
	            thatObj.TERMINAL_CODE = this.TERMINAL_CODE;
	            thatObj.EVENT_CODE = this.EVENT_CODE;
	            thatObj.FILE_PATH = this.FILE_PATH;
	            thatObj.REQUEST_AT = this.REQUEST_AT;
	            thatObj.CREATED_AT = this.CREATED_AT;
	            thatObj.REMARK = this.REMARK;
	            thatObj.IMG_INFO = this.IMG_INFO;
	            thatObj.ENTITY_ID = this.ENTITY_ID;
	            thatObj.SMS_COUNT = this.SMS_COUNT;
	            thatObj.SURPLUS_COUNT = this.SURPLUS_COUNT;
	            thatObj.SEND_STATE = this.SEND_STATE;
	            thatObj.CREATED_BY = this.CREATED_BY;
	
            return thatObj;
        }

        public virtual SmsSendShortMessageEntity Clone()
        {
            var thatObj = new SmsSendShortMessageEntity();

            return this.CloneTo(thatObj);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}