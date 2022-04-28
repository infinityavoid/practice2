using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace practice2
{
    public class DataGridViewNoEnter : DataGridView
    {
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
                return base.ProcessDialogKey(Keys.Tab);
            else
                return base.ProcessDialogKey(keyData);
        }
    }
}
