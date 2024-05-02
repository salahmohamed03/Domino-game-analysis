using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp1.Data;

namespace MauiApp1.Services.Interfaces
{
    public interface IGameService
	{
		public  Task<Settings> GetSettings();
		public  Task SetSettings(Settings settings);
	}
}
