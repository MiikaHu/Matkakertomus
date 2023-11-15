using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matkakertomus.Shared
{
    public class ImageDto
    {
        public uint ImageId { get; set; }
        public byte[] Image { get; set; }

        public uint StoryId { get; set; }
    }
}
