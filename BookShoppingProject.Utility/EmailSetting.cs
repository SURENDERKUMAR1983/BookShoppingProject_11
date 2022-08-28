using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppingProject.Utility
{
  public class EmailSetting
    {
       
            public string PrimaryDomain { get; set; }
            public int Primaryport { get; set; }
            public string SecondaryDomain { get; set; }
            public int SecondaryPort { get; set; }
            public string UserNameEmail { get; set; }
            public string UserNamePassword { get; set; }
            public string FromEmail { get; set; }
            public string ToEmail { get; set; }
            public string CCEmail { get; set; }
        
  }
}
