// ===========================================================================
//	©2013-2025 WebSupergoo. All rights reserved.
//
//	This source code is for use exclusively with the ABCpdf product with
//	which it is distributed, under the terms of the license for that
//	product. Details can be found at
//
//		http://www.websupergoo.com/
//
//	This copyright notice must not be deleted and must be reproduced alongside
//	any sections of code extracted from this module.
// ===========================================================================

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

using WebSupergoo.ABCpdf14;
using WebSupergoo.ABCpdf14.Objects;


namespace SignPdf {
	class Program {
		// You will need to change these values to ones appropriate for your login and setup.
		// See the instructions at the link below for details of what these values are and how to get them.
		// https://www.websupergoo.com/abcpdf-pdf-signatures-globalsign.aspx
		// Files should be placed in the top level folder - the one in which the .csproj is located.
		static Rest rest = new Rest("0000000000", "0000000000000000000000000000000000000000", "wsg.pfx", "password");

		static void Main(string[] args) {
			if (rest.Dir == null) {
				Console.WriteLine($"Unable to find pfx file. See Notes.rtf for what to do.");
				Console.ReadKey();
				return;
			}

			Console.WriteLine($"Connecting to GlobalSign");

			var l = rest.Login();

			// The validation policy tells which fields you should set.
			// You must set the required ones, you may set the optional ones, ignore all the rest.
			// Many fields are static which means they are pre-filled for you by GlobalSign.
			var v = rest.ValidationPolicy(l);

			// For example...
			Rest.IdentityRequest id = new Rest.IdentityRequest();
			if (v.subject_dn.common_name.presence == "REQUIRED") {
				id.subject_dn = new Rest.IdentityRequest.Subject();
				id.subject_dn.common_name = "John Doe";
			}

			var identity = rest.Identity(l, id);
			var cert = X509Certificate2.CreateFromPem(identity.signing_cert);

			Console.WriteLine($"Signing");

			using (Doc doc = new Doc()) {
				doc.FontSize = 96;
				doc.Rect.Inset(36, 36);
				doc.AddText("GlobalSign Example");
				Signature sig = doc.Form.AddSignature(new XRect("100 100 300 200"), "Sig");

				sig.CustomSigner2 = (data, state) => {
					if (state.HasFlag(Signature.State.Signing)) {
						using (var s = SHA256.Create())
							data = s.ComputeHash(data);
						var signature = rest.Sign(l, identity, Convert.ToHexString(data)).signature;
						return Convert.FromHexString(signature);
					}
					if (state.HasFlag(Signature.State.Timestamping)) {
						var signature = rest.Timestamp(l, Convert.ToHexString(data)).token;
						return Convert.FromBase64String(signature);
					}
					throw new Exception("State not recognized.");
				};

				// It is important to embed Long Term Validation (LTV) information
				// as GlobalSign certificates typically expire after ten minutes.
				// An easy way to do this is to specify an appropriate PAdES level.
				sig.CompliancePades = Signature.PadesLevel.PAdES_B_LTA;
				var type = Signature.DataType.Pkcs9Digest |
					Signature.DataType.TimeStampDigest |
					Signature.DataType.TimeStampToken;
				sig.Sign(new Oid("SHA256"), type, 5120, cert);
				doc.Save(Path.Combine(rest.Dir, "_GlobalSign.pdf"));
			}

			Console.WriteLine($"Validating");

			using (Doc doc = new Doc()) {
				doc.Read(Path.Combine(rest.Dir, "_GlobalSign.pdf"));
				var sig = (Signature)doc.Form.Fields["Sig"];
				var ok = sig.Validate(new X509Certificate2[] { cert });
				if (!ok || !sig.Validity.SigningTimeIsTrusted)
					Console.WriteLine($"Error: Signature invalid");
			}

			Console.WriteLine($"Finished");
			Console.ReadKey();
		}

		class Rest {
			public readonly string Dir;
			private readonly string _key;
			private readonly string _secret;
			private readonly HttpClientHandler _handler;
			private readonly HttpClient _client;

			private Rest() { }
			public Rest(string apiKey, string apiSecret, string mTlsName, string mTlsPassword) {
				_key = apiKey;
				_secret = apiSecret;
				var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
				while (!File.Exists(Path.Combine(dir.FullName, mTlsName))) {
					dir = dir.Parent;
					if (dir == null)
						return;
				}
				Dir = dir.FullName;
				_handler = new HttpClientHandler();
				_handler.ClientCertificateOptions = ClientCertificateOption.Manual;
				_handler.SslProtocols = SslProtocols.Tls12;
				_handler.ClientCertificates.Add(new X509Certificate2(Path.Combine(Dir, mTlsName), mTlsPassword));
				_client = new HttpClient(_handler);
			}

			// These JSON deserialization classes are based on the structures detailed here
			// https://www.globalsign.com/en/resources/apis/api-documentation/digital-signing-service-api-documentation.html

			public class LoginRequest {
				public string api_key { get; set; }
				public string api_secret { get; set; }
			}
			public class LoginResponse {
				public string access_token { get; set; }
			}
			public class IdentityRequest {
				public class TypeAndValue {
					public string type { get; set; }
					public string value { get; set; }
				}
				public class Subject {
					public string country { get; set; }
					public string state { get; set; }
					public string locality { get; set; }
					public string street_address { get; set; }
					public string organization { get; set; }
					public string[] organizational_unit { get; set; }
					public string common_name { get; set; }
					public string email { get; set; }
					public string jurisdiction_of_incorporation_locality_name { get; set; }
					public string jurisdiction_of_incorporation_state_or_province_name { get; set; }
					public string jurisdiction_of_incorporation_country_name { get; set; }
					public string business_category { get; set; }
					public TypeAndValue[] extra_attributes { get; set; }
				}
				public class San {
					public string[] dns_names { get; set; }
					public string[] emails { get; set; }
					public string[] ip_addresses { get; set; }
					public string[] uris { get; set; }
					public TypeAndValue[] other_names { get; set; }
					public TypeAndValue[] Items { get; set; }
				}
				public class KeyUsages {
					public string digital_signature { get; set; }
					public bool content_commitment { get; set; }
					public bool key_encipherment { get; set; }
					public bool data_encipherment { get; set; }
					public bool key_agreement { get; set; }
					public bool key_certificate_sign { get; set; }
					public bool crl_sign { get; set; }
					public bool encipher_only { get; set; }
					public bool decipher_only { get; set; }
				}
				public Subject subject_dn { get; set; }
				public San san { get; set; }
				public KeyUsages key_usages { get; set; }
				public string[] extended_key_usages { get; set; }
				// more properties can be found in GlobalSign docs
			}
			public class IdentityResponse {
				public string id { get; set; }
				public string signing_cert { get; set; }
				public string ocsp_response { get; set; }
			}

			public class ValidationResponse {
				public DN subject_dn { get; set; }
				public class DN {
					public class Policy {
						public string presence { get; set; }

						public string format { get; set; }
					}
					public Policy common_name { get; set; }
					public Policy organization { get; set; }
					public Policy organizational_unit { get; set; }
					public Policy country { get; set; }
					public Policy state { get; set; }
					public Policy locality { get; set; }
					public Policy street_address { get; set; }
					public Policy email { get; set; }
					public Policy jurisdiction_of_incorporation_locality_name { get; set; }
					public Policy jurisdiction_of_incorporation_state_or_province_name { get; set; }
					public Policy jurisdiction_of_incorporation_country_name { get; set; }
					public Policy business_category { get; set; }
					public string extra_attributes { get; set; }
				}
			}

			public class IdentitySignResponse {
				public string signature { get; set; }
			}
			public class TimestampResponse {
				public string token { get; set; }
			}
			public class CertificateResponse {
				public string path { get; set; }
			}

			public class TrustChainResponse {
				public string[] trustchain { get; set; }
				public string[] ocsp_revocation_info { get; set; }
			}

			public LoginResponse Login() {
				var login = new LoginRequest() { api_key = _key, api_secret = _secret };
				return Call<LoginRequest, LoginResponse>(false, "login", login, null);
			}

			public IdentityResponse Identity(LoginResponse login, IdentityRequest identity) {
				return Call<IdentityRequest, IdentityResponse>(false, "identity", identity, login.access_token);
			}

			public ValidationResponse ValidationPolicy(LoginResponse login) {
				return Call<string, ValidationResponse>(true, "validationpolicy", "", login.access_token);
			}

			public CertificateResponse Certificate(LoginResponse login) {
				return Call<string, CertificateResponse>(true, "certificate_path", "", login.access_token);
			}

			public TrustChainResponse TrustChain(LoginResponse login) {
				return Call<string, TrustChainResponse>(true, "trustchain", "", login.access_token);
			}

			public IdentitySignResponse Sign(LoginResponse login, IdentityResponse identity, string digest) {
				return Call<string, IdentitySignResponse>(true, $"identity/{identity.id}/sign/{digest}", "", login.access_token);
			}

			public TimestampResponse Timestamp(LoginResponse login, string digest) {
				return Call<string, TimestampResponse>(true, $"timestamp/{digest}", "", login.access_token);
			}

			private A Call<Q, A>(bool get, string verb, Q question, string token) {
				var task = CallAsync<Q, A>(get, verb, question, token);
				task.Wait();
				return task.Result;
			}

			private async Task<A> CallAsync<Q, A>(bool get, string verb, Q question, string token) {
				var opts = new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
				var json = question is string ? (string)(object)question : JsonSerializer.Serialize(question, opts);
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				var uri = "https://emea.api.dss.globalsign.com:8443/v2/" + verb;
				var request = new HttpRequestMessage(get ? HttpMethod.Get : HttpMethod.Post, uri);
				if (!get)
					request.Content = content;
				request.Headers.Add("Authorization", "Bearer " + token);
				HttpResponseMessage response = await _client.SendAsync(request);
				var str = await response.Content.ReadAsStringAsync();
				if (response.StatusCode != HttpStatusCode.OK)
					throw new Exception($"Error calling {verb}: {str}.");
				return typeof(A) == typeof(string) ? (A)(object)str : JsonSerializer.Deserialize<A>(str);
			}
		}
	}
}
