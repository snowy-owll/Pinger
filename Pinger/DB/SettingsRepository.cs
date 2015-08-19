using Pinger.Models;
using System;
using System.Data.Linq;
using System.Linq.Expressions;

namespace Pinger.DB
{
    class SettingsRepository : RepositoryBase<Setting, SettingsTable>
    {
        protected override Expression<Func<SettingsTable, Setting>> GetConverter()
        {
            return c => new Setting
            {
                Id = c.Id,
                Name = c.Name,
                Value = c.Value
            };
        }

        protected override Table<SettingsTable> GetTable()
        {
            return context.SettingsTable;
        }

        protected override void UpdateEntry(SettingsTable dbSetting, Setting setting)
        {
            dbSetting.Name = setting.Name;
            dbSetting.Value = setting.Value;
        }
    }
}
