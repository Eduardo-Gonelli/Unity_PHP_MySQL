using UnityEngine.Networking;

/// <summary>
/// Used to bypass the requirement for an HTTPS server. 
/// If you have only a HTTP server, this is a way to bypass it, 
/// however it is recommended to always use an HTTPS server.
/// From: https://answers.unity.com/questions/1874008/curl-error-60-cert-verify-failed-unitytls-x509veri-1.html
/// </summary>
public class ByPassHTTPSCertificate : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}
