using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revitalize.Framework.Utilities;

namespace Revitalize.Framework.World.Objects.Interfaces
{
    public interface IInventoryManagerProvider
    {
        public InventoryManager GetInventoryManager();
        public void SetInventoryManager(InventoryManager Manager);
    }
}
