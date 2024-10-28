using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebsiteApi;
using WebsiteApi.Context;
using WebsiteApi.Models;
using WebsiteApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Environment variables, appsettings and secret values
builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddUserSecrets<Program>();
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json");
}

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme = "github";
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/api/auth/login";
        options.LogoutPath = "/api/auth/logout";
    }) // cookie authentication middleware first
    .AddOAuth("github", options =>
        {
            // Oauth authentication middleware is second

            // When a user needs to sign in, they will be redirected to the authorize endpoint
            options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";

            // scopes
            options.Scope.Add("user");

            // After the user signs in, an authorization code will be sent to a callback
            // in this app. The OAuth middleware will intercept it
            options.CallbackPath = new PathString("/github-cb");

            // The OAuth middleware will send the ClientId, ClientSecret, and the
            // authorization code to the token endpoint, and get an access token in return
            options.ClientId = Secrets.ClientId;
            options.ClientSecret = Secrets.ClientSecret;
            options.TokenEndpoint = "https://github.com/login/oauth/access_token";

            // Below we call the userinfo endpoint to get information about the user
            options.UserInformationEndpoint = "https://api.github.com/user";

            // Describe how to map the user info we receive to user claims
            options.ClaimActions.MapJsonKey("GitHubId", "id");
            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
            options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");


            options.Events.OnCreatingTicket = async context =>
            {
                // Get user info from the userinfo endpoint and use it to populate user claims
                var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                request.Headers.Add("Authorization", $"Bearer {context.AccessToken}");

                var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead,
                    context.HttpContext.RequestAborted);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(jsonResponse);
                var user = jsonDoc.RootElement;
                context.Identity?.AddClaim(new Claim("github", "user"));

                // authentication complete, now check database for this user
                //var userService = context.HttpContext.RequestServices.GetService<UserService>();
                //var userIdString = user.GetProperty("id").ToString();
                //User? existingUser = null;
                //if (int.TryParse(userIdString, out int userId))
                //{
                //    // If parsing is successful, check if the user exists
                //    existingUser = await userService?.GetUserByGitHubId(userId);
                //}
                //// check if the user exists

                //if (existingUser is null || context.AccessToken is null)
                //{
                //    return;
                //} 

                //await userService.SetAccessToken(existingUser.UserId, context.AccessToken, 60 * 60 * 8);

                //context.Identity?.AddClaim(new Claim("id", existingUser.UserId.ToString()));
                context.RunClaimActions(user);
            };
        }
    );

builder.Services.AddAuthorization(b =>
{
    b.AddPolicy("github-enabled", pb =>
    {
        pb.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
            .RequireClaim("github", "user")
            .RequireAuthenticatedUser();
    });
});


builder.Services.AddControllers();
//builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<JobService>();
builder.Services.AddHttpClient();

// Database connection string
var connection = builder.Environment.IsDevelopment()
    ? builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")
    : Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");


// Register DbContext
builder.Services.AddDbContext<DataContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer(
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MechanicShopExpenseTracker;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False",
        options => options.EnableRetryOnFailure());
});


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

}

var app = builder.Build();
// Use CORS
// app.UseCors();
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();


// Simple API routes
app.MapGet("/", (HttpContext context) =>
{
    if (context.User.Identity?.IsAuthenticated ?? false)
    {
        // Return the user's claims as JSON
        return Results.Json(context.User.Claims.Select(c => new { c.Type, c.Value }));
    }

    // Return a message indicating the user is not logged in
    return Results.Text("not logged in");
});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
