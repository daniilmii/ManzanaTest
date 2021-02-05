using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckServiceWCF.Entities
{
    public class CheckEntityList:CheckBaseType
    {
       public List<CheckEntity> CheckList { get; set; }
    }
}
