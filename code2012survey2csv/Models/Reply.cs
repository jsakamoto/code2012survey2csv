using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace code2012survey2csv.Models
{
    public class Reply
    {
        public int id { get; set; }

        public DateTime created_at { get; set; }

        public string app_name { get; set; }

        public string why { get; set; }

        public string locale { get; set; }

        public string language { get; set; }

        public int? how_year { get; set; }

        public string free_comment { get; set; }

        private bool[] _why_choices;

        internal bool[] GetChoices()
        {
            if (_why_choices == null)
            {
                try
                {
                    _why_choices = JsonConvert.DeserializeObject<bool[]>(this.why) ?? new bool[0];
                }
                catch (Exception)
                {
                    _why_choices = new bool[0];
                }
            }
            return _why_choices;
        }

        public bool GetChoiceOf(int nth)
        {
            var choices = GetChoices();
            return nth < choices.Length ? choices[nth] : false;
        }
    }
}