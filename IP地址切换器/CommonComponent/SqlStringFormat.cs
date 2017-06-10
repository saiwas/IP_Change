using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IP地址切换器.CommonComponent
{
    public class SqlStringFormat
    {
        /// <summary>
        /// 将文本转化为SQL识别的语句
        /// </summary>
        /// <param name="pStr"></param>
        /// <returns>转化后的语句</returns>
        public static String GetQuotedString(String pStr)
        {
            return ("'" + pStr.Replace("'", "''") + "'");
        }
    }
}
