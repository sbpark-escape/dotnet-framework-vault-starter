using System.Web;
using DotNetFrameworkVaultStarter;

namespace LegacyWebFormsExample
{
    public sealed class LegacyDiagnosticsHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(VaultDiagnostics.CreateReport());
        }
    }
}
