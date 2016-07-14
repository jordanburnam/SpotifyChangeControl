using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.Client.Models.Changes
{
    public class SearchModel
    {

        private DateTime _Start;

        private DateTime _End;

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "MM/dd/yyyy", HtmlEncode = true, NullDisplayText = "")]
        public DateTime? Start
        {
            get { return (this._Start == DateTime.MinValue ? (DateTime?)null : this._Start); }
            set
            {

                if (value == null)
                {
                    this._Start = DateTime.MinValue;
                }
                else
                {
                    this._Start = (DateTime)value;
                }
            }

        }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = true, DataFormatString = "MM/dd/yyyy", HtmlEncode = true, NullDisplayText = "")]
        public DateTime? End
        {
            get { return (this._End == DateTime.MaxValue ? (DateTime?)null : this._End); }
            set
            {

                if (value == null)
                {
                    this._End = DateTime.MaxValue;
                }
                else
                {
                    this._End = (DateTime)value;
                }
            }
        }
        public SearchModel()
        {
            this._Start = DateTime.MinValue;
            this._End = DateTime.MaxValue;
        }


        public DateTime GetStart()
        {
            return this._Start;
        }

        public DateTime GetEnd()
        {
            return this._End;
        }

    }
}
