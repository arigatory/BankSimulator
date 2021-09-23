using System;
using System.Collections.Generic;
using System.Text;

namespace BankSimulator.DataAccess
{
    enum DataProviderEnum
    {
        SqlServer,
        #if PC
        OleDb,
        #endif
        Odbc,
        None
    }
}
