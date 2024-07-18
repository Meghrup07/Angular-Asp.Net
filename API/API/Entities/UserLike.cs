using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class UserLike
    {
        public AppUser SourceUser { get; set; } = null!;
        public int SourceUserId { get; set; }
        public AppUser TargetUser { get; set; } = null!;
        public int TargetUserId { get; set; }
    }
}