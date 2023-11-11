using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Drone_Management_Tools.Models;

namespace Drone_Management_Tools.Organizer
{
    public class DataFrameConverter
    {       
        private EncodeUtils _encodeUtils;
        public EncodeUtils EncodeUtils
        {
            get { return _encodeUtils; }
            set { _encodeUtils = value; }
        }

        private DecodeUtils _decodeUtils;
        public DecodeUtils DecodeUtils
        {
            get { return _decodeUtils; }
            set { _decodeUtils = value; }
        }

        public DataFrameConverter() 
        {
            this._encodeUtils = new EncodeUtils();
            this._decodeUtils = new DecodeUtils();
        }
        
        public void UpdateUICmd(UICmdTypes uiCmd)
        {
            this._encodeUtils.UiCmdType = uiCmd;
            this._decodeUtils.UiCmdType = uiCmd;
        }
    }
}
