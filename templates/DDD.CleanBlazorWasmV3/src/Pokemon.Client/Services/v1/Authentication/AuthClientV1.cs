using System.Net.Http.Headers;
using System.Text.Json;
using Pokemon.Client.Models;
using Pokemon.Contracts.v1.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace Pokemon.Client.Services.v1.Authentication;

public class AuthClientV1 : IAuthClient
{
    public string BaseUrl { get; }

    private readonly HttpClient _httpClient;

    public AuthClientV1(string baseUrl, HttpClient httpClient)
    {
        BaseUrl = baseUrl;
        _httpClient = httpClient;
    }


    public async Task<ServiceResponse<AuthenticationResponse>> VerifyEmailAsync(VerifyEmailRequest body)
    {
        var urlBuilder_ = new System.Text.StringBuilder();
        urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/auth/account/verify-email");

        var client_ = _httpClient;
        var serviceResponse = new ServiceResponse<AuthenticationResponse>()
        {
            Data = null,
            StatusCode = 0,
            Message = "Failed"
        };
        var disposeClient_ = false;
        try {
            using var request_ = new HttpRequestMessage();
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            request_.Content = content;
            request_.Method = new HttpMethod("POST");
            request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var url = urlBuilder_.ToString();
            request_.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

            var response_ = await client_.SendAsync(
                request_,
                HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);

            var disposeResponse_ = true;
            try
            {
                var status = (int)response_.StatusCode;
                if (status == 200)
                {
                    using var responseStream = await response_.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    var data = await JsonSerializer.DeserializeAsync<AuthenticationResponse>(responseStream);
                    serviceResponse.Data = data;
                    serviceResponse.StatusCode = status;
                    serviceResponse.Message = "Success";
                }
                else {
                    var problemDetails = await response_.Content.ReadFromJsonAsync<ProblemDetails>();
                    serviceResponse.StatusCode = status;
                    serviceResponse.Message = problemDetails!.Title;
                }
            }
            finally
            {
                if (disposeResponse_)
                {
                    response_.Dispose();
                }
            }
        }
        finally {
            if (disposeClient_)
            {
                client_.Dispose();
            }
        }
        return serviceResponse;
    }


    public async Task<ServiceResponse<AuthenticationResponse>> LoginAsync(LoginRequest body)
    {
        var urlBuilder_ = new System.Text.StringBuilder();
        urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/auth/login");

        var client_ = _httpClient;
        var serviceResponse = new ServiceResponse<AuthenticationResponse>()
        {
            Data = null,
            StatusCode = 0,
            Message = "Failed"
        };
        var disposeClient_ = false;
        try {
            using var request_ = new HttpRequestMessage();
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            request_.Content = content;
            request_.Method = new HttpMethod("POST");
            request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var url = urlBuilder_.ToString();
            request_.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

            var response_ = await client_.SendAsync(
                request_,
                HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);

            var disposeResponse_ = true;
            try
            {
                var status = (int)response_.StatusCode;
                if (status == 200)
                {
                    using var responseStream = await response_.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    var data = await JsonSerializer.DeserializeAsync<AuthenticationResponse>(responseStream);
                    serviceResponse.Data = data;
                    serviceResponse.StatusCode = status;
                    serviceResponse.Message = "Success";
                }
                else
                {
                    var problemDetails = await response_.Content.ReadFromJsonAsync<ProblemDetails>();
                    serviceResponse.StatusCode = status;
                    serviceResponse.Message = problemDetails!.Title;
                }
            }
            finally
            {
                if (disposeResponse_)
                {
                    response_.Dispose();
                }
            }
        }
        finally
        {
            if (disposeClient_)
            {
                client_.Dispose();
            }
        }

        return serviceResponse;
    }


    public async Task<ServiceResponse<AuthenticationResponse>> MeAsync()
    {
        var urlBuilder_ = new System.Text.StringBuilder();
        urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/auth/account/me");

        var client_ = _httpClient;
        var serviceResponse = new ServiceResponse<AuthenticationResponse>()
        {
            Data = null,
            StatusCode = 0,
            Message = "Failed"
        };
        var disposeClient_ = false;

        try {
            using var request_ = new HttpRequestMessage();
            request_.Method = new HttpMethod("GET");
            request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var url_ = urlBuilder_.ToString();
            request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

            var response_ = await client_.SendAsync(request_, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            var disposeresponse_ = true;
            try
            {
                var status = (int)response_.StatusCode;
                if (status == 200)
                {
                    using var responseStream = await response_.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    var data = await JsonSerializer.DeserializeAsync<AuthenticationResponse>(responseStream);
                    serviceResponse.Data = data;
                    serviceResponse.Message = "Success";
                    serviceResponse.StatusCode = status;
                }
                else
                {
                    var problemDetails = await response_.Content.ReadFromJsonAsync<ProblemDetails>();
                    serviceResponse.StatusCode = status;
                    serviceResponse.Message = problemDetails!.Title;
                }
            }
            finally
            {
                if (disposeresponse_)
                {
                    response_.Dispose();
                }
            }
        }
        finally
        {
            if (disposeClient_)
            {
                client_.Dispose();
            }
        }

        return serviceResponse;
    }


    public async Task<ServiceResponse<AuthenticationResponse>> RegisterAsync(RegisterRequest body)
    {
        var urlBuilder_ = new System.Text.StringBuilder();
        urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/auth/account/register");

        var client_ = _httpClient;
        var serviceResponse = new ServiceResponse<AuthenticationResponse>()
        {
            Data = null,
            StatusCode = 0,
            Message = "Failed"
        };
        var disposeClient_ = false;
        try {
            using var request_ = new HttpRequestMessage();
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            request_.Content = content;
            request_.Method = new HttpMethod("POST");
            request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var url = urlBuilder_.ToString();
            request_.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

            var response_ = await client_.SendAsync(
                request_,
                HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);

            var disposeResponse_ = true;
            try
            {
                var status = (int)response_.StatusCode;
                if (status == 201)
                {
                    using var responseStream = await response_.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    var data = await JsonSerializer.DeserializeAsync<AuthenticationResponse>(responseStream);
                    serviceResponse.Data = data;
                    serviceResponse.StatusCode = status;
                    serviceResponse.Message = "Success";
                }
                else {
                    var problemDetails = await response_.Content.ReadFromJsonAsync<ProblemDetails>();
                    serviceResponse.StatusCode = status;
                    serviceResponse.Message = problemDetails!.Title;
                }
            }
            finally
            {
                if (disposeResponse_)
                {
                    response_.Dispose();
                }
            }
        }
        finally
        {
            if (disposeClient_)
            {
                client_.Dispose();
            }
        }

        return serviceResponse;
    }


    public async Task<ServiceResponse<string>> LogoutAsync()
    {
        var urlBuilder_ = new System.Text.StringBuilder();
        urlBuilder_.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/api/v1/auth/logout");

        var client_ = _httpClient;
        var serviceResponse = new ServiceResponse<string>()
        {
            Data = string.Empty,
            StatusCode = 0,
            Message = "Failed"
        };
        var disposeClient_ = false;
        try {
            using var request_ = new HttpRequestMessage();

            var content = new StringContent(string.Empty);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            request_.Content = content;
            request_.Method = new HttpMethod("POST");
            request_.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

            var url = urlBuilder_.ToString();
            request_.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

            var response_ = await client_.SendAsync(
                request_,
                HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);

            var disposeResponse_ = true;
            try
            {
                var status = (int)response_.StatusCode;
                if (status == 204)
                {
                    serviceResponse.StatusCode = status;
                    serviceResponse.Message = "Success";
                }
                else
                {
                    var problemDetails = await response_.Content.ReadFromJsonAsync<ProblemDetails>();
                    serviceResponse.StatusCode = status;
                    serviceResponse.Message = problemDetails!.Title;
                }
            }
            finally
            {
                if (disposeResponse_)
                {
                    response_.Dispose();
                }
            }
        }
        finally
        {
            if (disposeClient_)
            {
                client_.Dispose();
            }
        }

        return serviceResponse;
    }
}