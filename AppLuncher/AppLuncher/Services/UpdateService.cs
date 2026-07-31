using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace AppLuncher.Services
{
    public sealed class UpdateService
    {
        public const string UpdateManifestUrl =
            "https://hmovaghari.ir/root/AppLuncher/FramworkApp.Update.txt";

        public async Task<UpdateInfo> GetLatestUpdateAsync()
        {
            RemoteCertificateValidationCallback previousCallback =
                ServicePointManager.ServerCertificateValidationCallback;

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = AcceptInvalidCertificate;

                using (WebClient client = new WebClient())
                {
                    string manifest = await client.DownloadStringTaskAsync(new Uri(UpdateManifestUrl));
                    string[] lines = manifest.Replace("\r\n", "\n").Split('\n');

                    if (lines.Length < 2)
                    {
                        throw new InvalidDataException("The update manifest must contain a version and download URL.");
                    }

                    decimal latestVersion;
                    if (!decimal.TryParse(lines[0].Trim(),
                        System.Globalization.NumberStyles.Number,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out latestVersion))
                    {
                        throw new InvalidDataException("The update manifest contains an invalid version.");
                    }

                    string downloadUrl = lines[1].Trim();
                    Uri downloadUri;
                    if (!Uri.TryCreate(downloadUrl, UriKind.Absolute, out downloadUri) ||
                        (downloadUri.Scheme != Uri.UriSchemeHttp && downloadUri.Scheme != Uri.UriSchemeHttps))
                    {
                        throw new InvalidDataException("The update manifest contains an invalid download URL.");
                    }

                    return new UpdateInfo(latestVersion, downloadUri.AbsoluteUri);
                }
            }
            finally
            {
                ServicePointManager.ServerCertificateValidationCallback = previousCallback;
            }
        }

        private static bool AcceptInvalidCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }

    public sealed class UpdateInfo
    {
        public UpdateInfo(decimal version, string downloadUrl)
        {
            Version = version;
            DownloadUrl = downloadUrl;
        }

        public decimal Version { get; private set; }

        public string DownloadUrl { get; private set; }
    }
}
