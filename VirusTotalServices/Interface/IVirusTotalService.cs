using System;
using System.Collections.Generic;
using System.Text;
using VirusTotalServices.Models;

namespace VirusTotalServices.Interface
{
    public interface IVirusTotalService
    {
        ScanOutput ScanFile(string fileName, byte[] data);
    }
}
