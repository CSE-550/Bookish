using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.Models
{
    public class OpenLibraryBook
    {
        public NamedValue description { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public string by_statement { get; set; }
        public List<NamedObject> authors { get; set; }
        public List<NamedObject> works { get; set; }
    }

    public class NamedObject
    {
        public string key { get; set; }
    }

    public class NamedValue 
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class OpenLibraryAuthor
    {
        public string name { get; set; }
    }
}
