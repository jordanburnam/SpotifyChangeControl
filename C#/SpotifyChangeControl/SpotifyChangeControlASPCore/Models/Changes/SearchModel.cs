using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Client.Models.Changes
{
    public class SearchModel
    {
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "MM/dd/yyyy", HtmlEncode = true, NullDisplayText = "NA")]
        private DateTime _Start;
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true,ConvertEmptyStringToNull =true, DataFormatString = "MM/dd/yyyy", HtmlEncode = true,NullDisplayText = "NA")]
        private DateTime _End;
        

        public DateTime Start
        {
            get { return this._Start; }
            set {

                    this._Start = value;

                }
        }

        public DateTime End
        {
            get { return this._End; }
            set { this._End = value; }
        }
        public SearchModel()
        {
            
        }
    }
}
