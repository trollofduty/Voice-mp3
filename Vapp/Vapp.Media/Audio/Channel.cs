using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Media.Audio
{
    public class Channel
    {
        public Channel(int id)
        {
            this.ID = id;
        }

        public int ID { get; }

        public List<Sample> Samples { get; set; } = new List<Sample>();
    }
}
