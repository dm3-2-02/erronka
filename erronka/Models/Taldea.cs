using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erronka.Models
{
    public class Taldea
    {
        public string id { get; set; }
        public string izena { get; set; }
        public string[] taldekideak { get; set; }
        public string[] generoa { get; set; }
        public string sorrera { get; set; }
        public List<Diskak> diskak { get; set; }
        public List<Kontzertuak> kontzertuak { get; set; }
        public string[] hizkuntza { get; set; }
        public string weborria { get; set; }

    }
}
