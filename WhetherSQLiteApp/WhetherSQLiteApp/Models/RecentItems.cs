using System;
using System.Collections.Generic;
using System.Text;
using Realms;

namespace WhetherSQLiteApp.Models
{
    public class RecentItems: RealmObject
    {

        [PrimaryKey]
        public string Name { get; set; }
        public string OpenedDate { get; set; }

        public string IconUrl { get; set; }

    }
}
