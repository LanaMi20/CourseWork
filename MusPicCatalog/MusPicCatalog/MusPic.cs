using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusPicCatalog
{
    internal class MusPic
    {
        public string Name { get; set; }
        public string Img { get; set; }
        public string Category { get; set; }
        public List<string> Tags { get; set; }
        public MusPic() { }
        public MusPic(string name, string img, string category)
        {
            Name = name;
            Img = img;
            Category = category;
            Tags = new List<string>();
        }

        public void add_tag(string tag)
        {
            Tags.Add(tag);
        }
    }
}
