using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OutlineVpn;
using Serilog;

namespace OutlineBot
{
    public class OutlineManager
    {
        private ILogger _logger;
        private OutlineApi _outline;

        public OutlineManager(string managementUrl)
        {
            _logger = Log.Logger.ForContext<OutlineManager>();
            
            _outline = new OutlineApi(managementUrl);
        }

        public bool IsKeyExists(string name)
        {
            return _outline.GetKeys().Any(x => x.Name == name);
        }

        public OutlineKey GetOrNewKey(string name)
        {
            var key = _outline.GetKeys().SingleOrDefault(x => x.Name == name);
            if (key != null) return key;
            key = _outline.CreateKey();
            _outline.RenameKey(key.Id, name);
            _outline.DeleteDataLimit(key.Id);
            _outline.AddDataLimit(key.Id, Program.Configuration.DataLimit);

            return key;
        }

        public void ResetDataLimit(string name)
        {
            var key = GetOrNewKey(name);
            _outline.AddDataLimit(key.Id, Program.Configuration.DataLimit);
        }

        public OutlineKey GetUsedBytes(string name)
        {
            var key = GetOrNewKey(name);
            return _outline.GetTransferredData().SingleOrDefault(x => x.Id == key.Id);
        }

        public void RevokeKey(string name)
        {
            var key = GetOrNewKey(name);
            _outline.DeleteKey(key.Id);
        }
    }
}
