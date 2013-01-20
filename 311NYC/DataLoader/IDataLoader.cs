using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _311NYC.Model;

namespace _311NYC.DataLoader
{
    public interface IPointGroupDataLoader
    {
        void LoadPointGroup(PointGroup pg);
    }
}
