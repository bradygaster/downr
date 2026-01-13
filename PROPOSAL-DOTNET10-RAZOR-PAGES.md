# downr .NET 10 Razor Pages Rewrite Proposal

## Executive Summary

This proposal outlines a comprehensive plan to rewrite downr from its current .NET Core 3.1 + Blazor WebAssembly architecture to a modern .NET 10 application using exclusively Razor Pages, with no Blazor Server or Blazor WebAssembly components.

## Current Architecture

### Technology Stack
- **.NET Core 3.1** - Core framework (released November 2019)
- **Blazor WebAssembly** - Client-side UI framework running in browser
- **ASP.NET Core Web API** - Server-side API endpoints
- **Markdig** - Markdown rendering engine
- **YamlDotNet** - YAML front-matter parsing

### Project Structure
```
downr/
├── Client/               # Blazor WebAssembly SPA
│   ├── Pages/           # .razor components (Index, Posts, Category)
│   ├── Shared/          # Shared Blazor components
│   └── wwwroot/         # Static client assets
├── Server/              # ASP.NET Core host + API
│   ├── Controllers/     # API controllers (PostsController, FeedController)
│   ├── Pages/          # Minimal Razor Pages (Error page only)
│   ├── Services/       # Content indexing services
│   └── wwwroot/posts/  # Markdown content files
└── Shared/             # Shared models and services
    ├── Models/         # Post, PostListModel, etc.
    └── Services/       # PostService, PostFileParser, etc.
```

### Current Flow
1. **Client-Side Rendering**: Blazor WebAssembly app loads in browser
2. **API Calls**: Client makes HTTP requests to `/api/posts`, `/api/post/{slug}`, etc.
3. **JSON Responses**: Server returns JSON data
4. **Browser Rendering**: Blazor components render HTML in browser using JavaScript

### Limitations of Current Architecture
1. **Initial Load Performance**: Blazor WebAssembly requires downloading .NET runtime and assemblies (~2MB+)
2. **SEO Challenges**: Client-side rendering makes content harder for search engines to index
3. **Complexity**: Separate Client/Server projects with API layer adds complexity
4. **Browser Requirements**: JavaScript must be enabled; older browsers have issues
5. **Development Overhead**: Maintaining separate client and server codebases
6. **Outdated Framework**: .NET Core 3.1 reached end-of-life in December 2022

## Proposed Architecture (.NET 10 + Razor Pages)

### Technology Stack
- **.NET 10** - Latest LTS framework (expected November 2025)
- **Razor Pages** - Server-side rendered pages
- **Markdig** - Markdown rendering (keep existing)
- **YamlDotNet** - YAML parsing (keep existing)
- **Azure Storage SDK** (updated) - For optional blob storage support

### Project Structure
```
downr/
├── downr.csproj         # Single consolidated project
├── Program.cs           # Minimal hosting model
├── Pages/              # Razor Pages
│   ├── Index.cshtml    # Homepage (blog or workshop mode)
│   ├── Index.cshtml.cs # Page model
│   ├── Post.cshtml     # Individual post display
│   ├── Post.cshtml.cs  # Post page model
│   ├── Category.cshtml # Category/Phase listing
│   ├── Error.cshtml    # Error page
│   ├── Admin/          # Admin section (optional feature)
│   │   ├── Login.cshtml          # Admin login
│   │   ├── Dashboard.cshtml      # Post management dashboard
│   │   ├── EditPost.cshtml       # Markdown editor
│   │   ├── CreatePost.cshtml     # New post creation
│   │   └── _AdminLayout.cshtml   # Admin layout
│   └── Shared/         # Shared layouts and partials
│       ├── _Layout.cshtml
│       ├── _PostCard.cshtml
│       ├── _Header.cshtml
│       ├── _NextPrevious.cshtml
│       └── _Tracker.cshtml
├── Services/           # Business logic services
│   ├── IContentIndexer.cs
│   ├── FileSystemContentIndexer.cs
│   ├── AzureStorageContentIndexer.cs
│   ├── PostService.cs
│   ├── PostFileParser.cs
│   ├── PostFileSorter.cs
│   └── PostEditorService.cs      # Admin: Post CRUD operations
├── Models/             # Domain models
│   ├── Post.cs
│   ├── PostListModel.cs
│   ├── PostPageModel.cs
│   ├── DownrOptions.cs
│   └── PostEditorModel.cs        # Admin: Editor view model
├── Controllers/        # API endpoints (for RSS feed)
│   └── FeedController.cs
├── Extensions/         # Service registration extensions
│   ├── DownrServiceExtensions.cs
│   ├── FileSystemExtensions.cs
│   └── AzureStorageExtensions.cs
├── Workers/           # Background workers (content refresh)
│   └── ContentRefreshWorker.cs
└── wwwroot/
    ├── posts/         # Markdown content
    ├── css/           # Stylesheets (simplified, no Blazor CSS)
    ├── js/            # Minimal JavaScript
    │   ├── syntax-highlighting.js
    │   └── markdown-editor.js    # Admin: Markdown editor integration
    └── admin/         # Admin-specific assets
        ├── css/
        └── js/
```

### Flow in New Architecture
1. **HTTP Request**: Browser requests page (e.g., `/post/my-post`)
2. **Razor Page Execution**: Server executes corresponding Razor Page
3. **Server-Side Rendering**: Page model loads data, HTML rendered on server
4. **HTML Response**: Complete HTML page sent to browser
5. **Progressive Enhancement**: Optional JavaScript for syntax highlighting

## .NET Aspire Support for Local Development

### Overview

.NET Aspire integration provides a modern cloud-ready development experience while maintaining simple deployment options. The downr application can run as a standalone Razor Pages app OR as an Aspire-orchestrated application, giving developers flexibility without compromising simplicity.

### Why Aspire for downr?

**Benefits:**
- **Local Development Dashboard**: Visual overview of app health, logs, and telemetry
- **Service Discovery**: Easy integration if you want to add databases, Redis, or other services
- **Observability**: Built-in OpenTelemetry support for distributed tracing and metrics
- **Configuration Management**: Centralized configuration and secrets across services
- **Future Extensibility**: Simple path to add APIs, background jobs, or other microservices
- **Testing**: Easy to test with Azure resources (storage, databases) locally via emulators

**Key Design Principle:**
- Aspire is **optional** for development - the app runs perfectly fine without it
- Deployment remains simple - deploy as a standard Linux App Service
- OR deploy full Aspire solution to Azure Container Apps for advanced scenarios

### Project Structure with Aspire

```
downr/
├── downr.AppHost/              # Aspire orchestration (optional)
│   ├── downr.AppHost.csproj
│   ├── Program.cs              # Aspire app host configuration
│   └── appsettings.json
├── downr.ServiceDefaults/      # Shared Aspire defaults (optional)
│   ├── downr.ServiceDefaults.csproj
│   └── Extensions.cs           # Health checks, telemetry, resilience
├── downr.Web/                  # Main application (can run standalone)
│   ├── downr.Web.csproj        # Updated with Aspire references
│   ├── Program.cs              # Works with or without Aspire
│   ├── Pages/                  # Razor Pages
│   ├── Services/               # Business logic
│   └── wwwroot/
└── downr.sln                   # Solution file
```

### AppHost Configuration (Program.cs)

```csharp
// downr.AppHost/Program.cs
var builder = DistributedApplication.CreateBuilder(args);

// Add the main downr web application
var web = builder.AddProject<Projects.downr_Web>("downr-web")
    .WithExternalHttpEndpoints(); // Allow external access

// Optional: Add Azure Storage emulator for local development
var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator();

var blobs = storage.AddBlobs("posts");

// Configure downr to use the storage
web.WithReference(blobs);

// Optional: Add Redis for output caching (future enhancement)
// var redis = builder.AddRedis("cache");
// web.WithReference(redis);

// Optional: Add Application Insights for telemetry
// var appInsights = builder.AddAzureApplicationInsights("insights");
// web.WithReference(appInsights);

builder.Build().Run();
```

### ServiceDefaults Configuration

```csharp
// downr.ServiceDefaults/Extensions.cs
public static class Extensions
{
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        // Add health checks
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy());

        // Add default OpenTelemetry configuration
        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation()
                       .AddRuntimeInstrumentation();
            })
            .WithTracing(tracing =>
            {
                tracing.AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation();
            });

        // Configure logging with OpenTelemetry
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        // Add default resilience patterns (retry, circuit breaker)
        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            http.AddStandardResilienceHandler();
        });

        return builder;
    }
}
```

### Updated downr.Web Program.cs

```csharp
// downr.Web/Program.cs
var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults if available (graceful degradation)
if (builder.Environment.IsDevelopment())
{
    try
    {
        builder.AddServiceDefaults();
    }
    catch
    {
        // Aspire not available, continue without it
    }
}

// Standard downr configuration
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "image/svg+xml", "application/font-woff2" });
});

builder.Services.AddRazorPages();

// Add downr services
builder.Services.AddDownr(builder.Configuration)
    .WithWebServerFileSystemStorage();
    // OR .WithAzureStorage(); // Aspire can inject Azure Storage connection

// Admin section
var authProvider = builder.Configuration["downr:admin:authProvider"] ?? "github";
if (authProvider == "github")
{
    builder.Services.AddAuthentication()
        .AddCookie()
        .AddGitHub(options =>
        {
            options.ClientId = builder.Configuration["downr:admin:github:clientId"];
            options.ClientSecret = builder.Configuration["downr:admin:github:clientSecret"];
        });
}

builder.Services.AddAuthorization();

var app = builder.Build();

// Map default endpoints (includes health checks from Aspire)
if (builder.Environment.IsDevelopment())
{
    try
    {
        app.MapDefaultEndpoints(); // Aspire health checks
    }
    catch
    {
        // Aspire not available
    }
}

// Standard downr middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseResponseCompression();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.UseDownr()
    .WithWebServerFileSystemStorage();

app.Run();
```

### Running with Aspire (Local Development)

**Option 1: With Aspire Dashboard**
```bash
# Run the Aspire AppHost - launches dashboard and all services
cd downr.AppHost
dotnet run

# Opens browser to Aspire dashboard (typically http://localhost:15888)
# Shows:
# - downr-web running with health status
# - Logs and traces from the application
# - Metrics and performance data
# - Connected resources (storage emulator, etc.)
```

**Option 2: Standalone (Traditional)**
```bash
# Run just the web app without Aspire
cd downr.Web
dotnet run

# App runs normally on https://localhost:5001
```

### Deployment Options

#### Option 1: Simple Linux App Service (Recommended for Most)

Deploy the `downr.Web` project as a standard web app:

```bash
# Build and publish
dotnet publish downr.Web -c Release -o ./publish

# Deploy to Azure App Service
az webapp up --name my-downr-blog --runtime "DOTNETCORE:10.0"
```

**Characteristics:**
- Simple, single-container deployment
- Low cost (can use free tier for testing)
- No Aspire dependencies in production
- Standard App Service features (auto-scaling, deployment slots, etc.)

#### Option 2: Azure Container Apps with Aspire (Advanced Scenarios)

Use when you need:
- Multiple services (future APIs, background workers)
- Service-to-service communication
- Advanced observability
- Microservices architecture

```bash
# Install Aspire tooling
dotnet tool install -g Microsoft.NET.Aspire.Hosting.Orchestration

# Deploy entire Aspire solution
azd init
azd up

# Deploys:
# - downr.Web to Container Apps
# - Azure Storage account (if configured)
# - Application Insights
# - Container Apps Environment with all infrastructure
```

### Aspire Dashboard Features

When running with Aspire locally, developers get:

**1. Resources View**
- Health status of downr.Web application
- Connected resources (storage, cache, databases)
- Resource endpoints and connection strings

**2. Console Logs**
- Real-time log streaming from the application
- Filterable by log level and source
- Search across all logs

**3. Structured Logs**
- Detailed log entries with structured data
- Correlation IDs for request tracking
- Exception details and stack traces

**4. Traces**
- Distributed tracing for HTTP requests
- Database query performance
- External service calls
- Visual timeline of request flow

**5. Metrics**
- Request rates and durations
- Memory and CPU usage
- Custom business metrics
- Historical data visualization

### Adding Azure Storage via Aspire

Example: Switch from filesystem to Azure Storage for posts

```csharp
// downr.AppHost/Program.cs
var storage = builder.AddAzureStorage("storage");
var blobs = storage.AddBlobs("posts");

var web = builder.AddProject<Projects.downr_Web>("downr-web")
    .WithReference(blobs)
    .WithEnvironment("downr__StorageMode", "AzureBlobs");

// downr.Web/Program.cs automatically receives:
// - Connection string via environment variables
// - Configuration from Aspire service discovery
builder.Services.AddDownr(builder.Configuration)
    .WithAzureStorage(); // Uses connection from Aspire
```

### Future Extensions Made Easy

With Aspire foundation, adding features becomes simple:

**Example 1: Add Redis for Output Caching**
```csharp
// AppHost
var redis = builder.AddRedis("cache");
web.WithReference(redis);

// Web app automatically gets Redis connection
builder.Services.AddOutputCache()
    .AddRedisOutputCache();
```

**Example 2: Add Background Worker for GitHub Sync**
```csharp
// AppHost
var worker = builder.AddProject<Projects.downr_SyncWorker>("sync-worker");
worker.WithReference(storage);

// Both web and worker can access same storage
```

**Example 3: Add SQL Database for Comments Feature**
```csharp
// AppHost
var sql = builder.AddSqlServer("sql")
    .AddDatabase("comments");
web.WithReference(sql);

// Web app receives connection string automatically
```

### Package Updates for Aspire

```xml
<!-- downr.AppHost/downr.AppHost.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <IsAspireHost>true</IsAspireHost>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Aspire.Hosting" Version="10.0.0" />
    <PackageReference Include="Aspire.Hosting.Azure.Storage" Version="10.0.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\downr.Web\downr.Web.csproj" />
  </ItemGroup>
</Project>

<!-- downr.ServiceDefaults/downr.ServiceDefaults.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Http.Resilience" Version="10.0.0" />
    <PackageReference Include="Microsoft.Extensions.ServiceDiscovery" Version="10.0.0" />
    <PackageReference Include="OpenTelemetry.Exporter.OpenTelemetryProtocol" Version="1.7.0" />
    <PackageReference Include="OpenTelemetry.Extensions.Hosting" Version="1.7.0" />
    <PackageReference Include="OpenTelemetry.Instrumentation.AspNetCore" Version="1.7.0" />
    <PackageReference Include="OpenTelemetry.Instrumentation.Http" Version="1.7.0" />
    <PackageReference Include="OpenTelemetry.Instrumentation.Runtime" Version="1.7.0" />
  </ItemGroup>
</Project>

<!-- downr.Web/downr.Web.csproj - ADD these for Aspire support -->
<ItemGroup Condition="'$(Configuration)' == 'Debug'">
  <PackageReference Include="Aspire.Microsoft.Extensions.ServiceDiscovery" Version="10.0.0" />
  <PackageReference Include="Microsoft.Extensions.Http.Resilience" Version="10.0.0" />
  <ProjectReference Include="..\downr.ServiceDefaults\downr.ServiceDefaults.csproj" />
</ItemGroup>
```

### Benefits Summary

| Aspect | Without Aspire | With Aspire |
|--------|---------------|-------------|
| **Local Development** | Run with `dotnet run` | Run with `dotnet run` (same!) OR use AppHost for dashboard |
| **Observability** | Basic logging | Full distributed tracing, metrics, structured logs |
| **Service Integration** | Manual configuration | Auto-configured service discovery |
| **Testing** | Manual emulator setup | One-click emulator launch |
| **Deployment (Simple)** | App Service | App Service (unchanged) |
| **Deployment (Advanced)** | Manual ACA setup | `azd up` for full stack |
| **Adding Services** | Manual wiring | Add one line to AppHost |

### Migration Path

**Phase 1 (Optional)**: Add Aspire for Development
1. Add `downr.AppHost` and `downr.ServiceDefaults` projects
2. Update `downr.Web` to reference ServiceDefaults (conditional, Debug only)
3. Developers can choose to use Aspire dashboard or not

**Phase 2 (Optional)**: Leverage Aspire for Testing
1. Configure Azure Storage emulator in AppHost
2. Configure Redis for caching experiments
3. Test observability and monitoring locally

**Phase 3 (Optional)**: Production Aspire Deployment
1. Only if you add microservices or need advanced features
2. Deploy to Azure Container Apps using `azd`
3. Get automatic infrastructure setup

### Key Design Decisions

1. **Aspire is Development-Time Only by Default**
   - Production deployments don't require Aspire
   - Simple App Service deployment remains the primary path
   - Aspire dependencies only in Debug configuration

2. **Graceful Degradation**
   - App works perfectly without Aspire
   - Try/catch blocks around Aspire-specific calls
   - No breaking changes to existing deployment

3. **Opt-In Complexity**
   - Developers choose whether to use Aspire dashboard
   - `dotnet run` on downr.Web still works without AppHost
   - Advanced features only when you need them

4. **Future-Proof**
   - Easy to add databases, caching, APIs later
   - Aspire handles service discovery and configuration
   - Standardized patterns for cloud-native features

## Benefits of Razor Pages Approach

### 1. Performance Improvements
- **Faster Initial Load**: No need to download .NET runtime or assemblies
- **Reduced Bandwidth**: Only HTML/CSS/images sent to client (not MB of DLLs)
- **Better Perceived Performance**: Content visible immediately (no loading spinner)
- **Server-Side Caching**: Output caching can dramatically improve performance

### 2. SEO Advantages
- **Full HTML Delivered**: Search engines see complete content immediately
- **Meta Tags Present**: Open Graph and Twitter Card tags in initial HTML
- **No Rendering Wait**: Crawlers don't need to execute JavaScript
- **Better Indexing**: Content is available in raw HTML source

### 3. Simplified Architecture
- **Single Project**: No Client/Server split, reducing complexity
- **No API Layer**: Direct data access from page models
- **Fewer Dependencies**: Remove Blazor-specific packages
- **Standard Patterns**: Traditional ASP.NET Core patterns familiar to most developers

### 4. Improved Compatibility
- **Works Without JavaScript**: Core functionality works with JS disabled
- **Older Browser Support**: No requirement for modern JavaScript features
- **Accessibility**: Server-rendered HTML easier to make accessible
- **Progressive Enhancement**: Add JavaScript for enhanced features only

### 5. Developer Experience
- **Simplified Debugging**: Server-side only, easier to debug
- **Faster Build Times**: No Blazor WebAssembly compilation
- **Hot Reload**: .NET 10 hot reload works excellently with Razor Pages
- **Fewer Concepts**: No need to understand Blazor component lifecycle

### 6. Modern .NET 10 Features
- **Performance**: .NET 10 runtime improvements
- **Native AOT**: Option to compile to native code for even faster startup
- **Minimal APIs**: For feed endpoints if desired
- **Rate Limiting**: Built-in rate limiting middleware
- **Output Caching**: New output caching middleware for better performance

## Admin Section: Online Markdown Editor

> **Note**: The code examples in this section are for illustrative purposes and demonstrate the basic structure and functionality. Production implementation would require additional hardening including:
> - Proper secrets management (Azure Key Vault, environment variables instead of appsettings.json)
> - Enhanced CSP policies without 'unsafe-inline' (using nonces or external files)
> - Custom accessible modal dialogs instead of browser prompt()
> - User-facing error notifications for failed operations
> - Additional security audits and penetration testing

### Overview

A key enhancement for the .NET 10 rewrite is the addition of an optional admin section that provides a web-based interface for managing blog posts. This eliminates the need to manually edit Markdown files in VS Code and commit changes through Git, making downr more accessible to content creators who prefer a browser-based workflow.

### Features

#### 1. **Authentication & Authorization**
- **Primary**: GitHub OAuth authentication for seamless GitHub integration
- **Alternative**: Static username/password from environment variables (no database required)
- Cookie-based authentication for admin sessions
- Easy configuration to swap between authentication providers
- Future-ready for GitHub repository integration (commit/push from admin UI)

**GitHub OAuth Benefits:**
- Leverage existing GitHub account (no separate credentials)
- Natural fit for developers already using GitHub for content
- Enables future features like committing posts directly to GitHub repo
- Built-in security and 2FA support from GitHub

**Static Credentials Option:**
- Simple deployment without external dependencies
- Username and password stored in environment variables
- Suitable for single-user scenarios or air-gapped deployments

#### 2. **Post Management Dashboard**
- List all posts with search and filter capabilities
- Sort by date, category, or title
- Bulk operations (delete, unpublish)
- Quick preview of post content
- Status indicators (published, draft)

#### 3. **Online Markdown Editor**
- **Live Preview**: Split-pane editor with real-time Markdown preview
- **Syntax Highlighting**: Code blocks with syntax highlighting in preview
- **YAML Front Matter Editor**: Separate form for metadata (title, slug, categories, etc.)
- **Auto-Save**: Periodic auto-save to prevent data loss
- **Image Upload**: Drag-and-drop image upload to post media folder
- **Markdown Toolbar**: Quick insert buttons for common Markdown syntax

#### 4. **Post CRUD Operations**
- **Create**: New post with slug auto-generation from title
- **Read**: View existing posts in editor
- **Update**: Edit and save posts with validation
- **Delete**: Remove posts with confirmation

#### 5. **File Management**
- Browse and manage post media files
- Upload multiple images at once
- Preview images before insertion
- Automatic media folder creation per post

### Technical Implementation

#### Authentication Setup

The admin section supports two authentication providers that can be easily swapped via configuration.

**GitHub OAuth Authentication (Recommended)**

```csharp
// Program.cs
var authProvider = builder.Configuration["downr:admin:authProvider"] ?? "github";

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = authProvider == "github" ? "GitHub" : CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/admin/login";
    options.LogoutPath = "/admin/logout";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

// Add GitHub OAuth if configured
if (authProvider == "github")
{
    // Validate required configuration
    var clientId = builder.Configuration["downr:admin:github:clientId"];
    var clientSecret = builder.Configuration["downr:admin:github:clientSecret"];
    var allowedUsersConfig = builder.Configuration["downr:admin:github:allowedUsers"];
    
    if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
    {
        throw new InvalidOperationException(
            "GitHub OAuth requires downr:admin:github:clientId and " +
            "downr:admin:github:clientSecret to be configured");
    }
    
    if (string.IsNullOrEmpty(allowedUsersConfig))
    {
        throw new InvalidOperationException(
            "GitHub OAuth requires downr:admin:github:allowedUsers to be configured. " +
            "Specify comma-separated GitHub usernames who can access the admin section.");
    }
    
    builder.Services.AddAuthentication()
        .AddGitHub(options =>
        {
            options.ClientId = clientId;
            options.ClientSecret = clientSecret;
            options.CallbackPath = "/signin-github";
            options.Scope.Add("user:email");
            // Future: Add "repo" scope for GitHub repository integration
            options.SaveTokens = true; // Save access token for future GitHub API calls
            
            options.ClaimActions.MapJsonKey("urn:github:login", "login");
            options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
            options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");
            
            options.Events.OnCreatingTicket = async context =>
            {
                // Validate that the GitHub user is authorized
                // Parse allowed users from configuration (already validated as non-empty above)
                var allowedUsers = allowedUsersConfig
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(u => u.Trim())
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);
                
                var username = context.Principal.FindFirst("urn:github:login")?.Value;
                
                // Fail-safe: deny access if username is missing or not in allowed list
                if (string.IsNullOrEmpty(username) || !allowedUsers.Contains(username))
                {
                    context.Fail($"User '{username}' is not authorized to access the admin section");
                }
            };
        });
}

builder.Services.AddAuthorization();

// After app.UseRouting()
app.UseAuthentication();
app.UseAuthorization();
```

**Static Username/Password Authentication**

```csharp
// Program.cs - When authProvider is "static"
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/admin/login";
        options.LogoutPath = "/admin/logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization();

// Register static credentials service
builder.Services.AddSingleton<IStaticAuthService, StaticAuthService>();
```

**StaticAuthService Implementation**

```csharp
public interface IStaticAuthService
{
    bool ValidateCredentials(string username, string password);
    string GetAuthorizedUsername();
}

public class StaticAuthService : IStaticAuthService
{
    private readonly string _username;
    private readonly string _passwordHash;

    public StaticAuthService(IConfiguration configuration)
    {
        // Read username from environment variables or configuration
        _username = Environment.GetEnvironmentVariable("DOWNR_ADMIN_USERNAME") 
                   ?? configuration["downr:admin:static:username"];
        
        // Read password hash from environment variables or configuration
        // IMPORTANT: Store the BCrypt hash, not plaintext password
        _passwordHash = Environment.GetEnvironmentVariable("DOWNR_ADMIN_PASSWORD_HASH")
                       ?? configuration["downr:admin:static:passwordHash"];
        
        if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_passwordHash))
        {
            throw new InvalidOperationException(
                "Static auth requires DOWNR_ADMIN_USERNAME and DOWNR_ADMIN_PASSWORD_HASH " +
                "environment variables or downr:admin:static configuration");
        }
        
        // Validate that the hash looks like a BCrypt hash
        // BCrypt hashes start with $2a$, $2b$, $2x$, or $2y$ followed by cost factor
        if (!System.Text.RegularExpressions.Regex.IsMatch(_passwordHash, @"^\$2[abxy]\$\d{2}\$.{53}$"))
        {
            throw new InvalidOperationException(
                "DOWNR_ADMIN_PASSWORD_HASH must be a valid BCrypt hash. " +
                "Generate one using: BCrypt.Net.BCrypt.HashPassword(\"your-password\", 12)");
        }
    }

    public bool ValidateCredentials(string username, string password)
    {
        // Use constant-time comparison to prevent timing attacks
        // Always perform both username and password checks
        
        bool usernameMatches = string.Equals(username, _username, StringComparison.Ordinal);
        bool passwordMatches = false;
        
        try
        {
            // Perform password verification even if username doesn't match
            // This prevents timing attacks that could determine valid usernames
            passwordMatches = BCrypt.Net.BCrypt.Verify(password, _passwordHash);
        }
        catch
        {
            // If verification fails (e.g., malformed hash), return false
            passwordMatches = false;
        }
        
        return usernameMatches && passwordMatches;
    }

    public string GetAuthorizedUsername() => _username;
}
```

**Login Page Implementation**

```csharp
// Pages/Admin/Login.cshtml.cs
public class LoginModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly IStaticAuthService _staticAuthService;
    private readonly string _authProvider;

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string ReturnUrl { get; set; }
    
    // Expose auth provider for view
    public string AuthProvider => _authProvider;

    public LoginModel(IConfiguration configuration, IStaticAuthService staticAuthService = null)
    {
        _configuration = configuration;
        _staticAuthService = staticAuthService;
        _authProvider = configuration["downr:admin:authProvider"] ?? "github";
        
        // Validate that static auth service is available when using static provider
        if (_authProvider == "static" && staticAuthService == null)
        {
            throw new InvalidOperationException(
                "IStaticAuthService must be registered when using static authentication. " +
                "Add 'services.AddSingleton<IStaticAuthService, StaticAuthService>();' to Program.cs");
        }
    }

    public void OnGet(string returnUrl = null)
    {
        ReturnUrl = returnUrl ?? "/admin/dashboard";
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (_authProvider == "static")
        {
            if (!ModelState.IsValid)
                return Page();

            // _staticAuthService is guaranteed to be non-null here due to constructor validation
            if (_staticAuthService!.ValidateCredentials(Username, Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Username),
                    new Claim("auth_provider", "static")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return LocalRedirect(ReturnUrl);
            }

            ModelState.AddModelError("", "Invalid username or password");
            return Page();
        }
        else // GitHub OAuth
        {
            // Trigger GitHub OAuth flow
            return Challenge(new AuthenticationProperties { RedirectUri = ReturnUrl }, "GitHub");
        }
    }
}
```

**Login Page UI**

```html
<!-- Pages/Admin/Login.cshtml -->
@page
@model LoginModel
@{
    ViewData["Title"] = "Admin Login";
    Layout = null;
}

<!DOCTYPE html>
<html>
<head>
    <title>@ViewData["Title"] - downr</title>
    <link rel="stylesheet" href="~/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/admin/css/admin.css" />
</head>
<body class="login-page">
    <div class="container">
        <div class="row justify-content-center mt-5">
            <div class="col-md-6 col-lg-4">
                <div class="card shadow">
                    <div class="card-body">
                        <h2 class="card-title text-center mb-4">downr Admin</h2>
                        
                        @if (Model.AuthProvider == "github")
                        {
                            <form method="post">
                                <input type="hidden" name="ReturnUrl" value="@Model.ReturnUrl" />
                                <button type="submit" class="btn btn-dark btn-lg w-100">
                                    <svg width="20" height="20" fill="currentColor" class="me-2" viewBox="0 0 16 16">
                                        <path d="M8 0C3.58 0 0 3.58 0 8c0 3.54 2.29 6.53 5.47 7.59.4.07.55-.17.55-.38 0-.19-.01-.82-.01-1.49-2.01.37-2.53-.49-2.69-.94-.09-.23-.48-.94-.82-1.13-.28-.15-.68-.52-.01-.53.63-.01 1.08.58 1.23.82.72 1.21 1.87.87 2.33.66.07-.52.28-.87.51-1.07-1.78-.2-3.64-.89-3.64-3.95 0-.87.31-1.59.82-2.15-.08-.2-.36-1.02.08-2.12 0 0 .67-.21 2.2.82.64-.18 1.32-.27 2-.27.68 0 1.36.09 2 .27 1.53-1.04 2.2-.82 2.2-.82.44 1.1.16 1.92.08 2.12.51.56.82 1.27.82 2.15 0 3.07-1.87 3.75-3.65 3.95.29.25.54.73.54 1.48 0 1.07-.01 1.93-.01 2.2 0 .21.15.46.55.38A8.012 8.012 0 0 0 16 8c0-4.42-3.58-8-8-8z"/>
                                    </svg>
                                    Sign in with GitHub
                                </button>
                            </form>
                        }
                        else
                        {
                            <form method="post">
                                <input type="hidden" asp-for="ReturnUrl" />
                                <div class="mb-3">
                                    <label asp-for="Username" class="form-label">Username</label>
                                    <input asp-for="Username" class="form-control" autofocus />
                                    <span asp-validation-for="Username" class="text-danger"></span>
                                </div>
                                <div class="mb-3">
                                    <label asp-for="Password" class="form-label">Password</label>
                                    <input asp-for="Password" type="password" class="form-control" />
                                    <span asp-validation-for="Password" class="text-danger"></span>
                                </div>
                                <div asp-validation-summary="ModelOnly" class="text-danger mb-3"></div>
                                <button type="submit" class="btn btn-primary w-100">Sign In</button>
                            </form>
                        }
                    </div>
                </div>
                <p class="text-center text-muted mt-3">
                    <small>Authentication: @Model.AuthProvider</small>
                </p>
            </div>
        </div>
    </div>
</body>
</html>
```
```

#### PostEditorService

```csharp
public interface IPostEditorService
{
    Task<Post> GetPostForEditing(string slug);
    Task<bool> SavePost(PostEditorModel model);
    Task<bool> CreatePost(PostEditorModel model);
    Task<bool> DeletePost(string slug);
    Task<string> UploadImage(string slug, IFormFile file);
    Task<List<string>> GetPostImages(string slug);
}

public class PostEditorService : IPostEditorService
{
    private readonly IContentIndexer _indexer;
    private readonly DownrOptions _options;
    private readonly IWebHostEnvironment _env;

    public async Task<Post> GetPostForEditing(string slug)
    {
        // Implementation: Load post from file system or Azure Storage
        return await Task.FromResult(_indexer.Posts.FirstOrDefault(p => p.Slug == slug));
    }

    public async Task<bool> SavePost(PostEditorModel model)
    {
        // 1. Validate model (slug uniqueness, required fields)
        // 2. Generate YAML front matter from model properties
        // 3. Combine YAML + Markdown content
        // 4. Write to disk: wwwroot/posts/{slug}/index.md
        // 5. Trigger content re-indexing
        // 6. Return success/failure
        return await Task.FromResult(true);
    }

    public async Task<bool> CreatePost(PostEditorModel model)
    {
        // Similar to SavePost but create new directory structure
        return await Task.FromResult(true);
    }

    public async Task<bool> DeletePost(string slug)
    {
        // Delete post directory and trigger re-indexing
        return await Task.FromResult(true);
    }

    public async Task<string> UploadImage(string slug, IFormFile file)
    {
        // 1. Validate file type and size
        // 2. Create media folder if needed: wwwroot/posts/{slug}/media/
        // 3. Generate safe filename
        // 4. Save file to disk
        // 5. Return relative path: media/{filename}
        return await Task.FromResult($"media/{file.FileName}");
    }

    public async Task<List<string>> GetPostImages(string slug)
    {
        // Get list of images in post's media folder
        var mediaPath = Path.Combine(_env.WebRootPath, "posts", slug, "media");
        if (!Directory.Exists(mediaPath))
            return new List<string>();
            
        return await Task.FromResult(
            Directory.GetFiles(mediaPath)
                     .Select(f => Path.GetFileName(f))
                     .ToList()
        );
    }
}
```

#### Admin Page Models

**Dashboard.cshtml.cs**
```csharp
[Authorize]
public class DashboardModel : PageModel
{
    private readonly PostService _postService;
    
    public List<PostSummary> Posts { get; set; }
    public string SearchTerm { get; set; }
    public string CategoryFilter { get; set; }

    public async Task OnGetAsync(string search = null, string category = null)
    {
        SearchTerm = search;
        CategoryFilter = category;
        
        var allPosts = _postService.GetPosts();
        
        if (!string.IsNullOrEmpty(search))
            allPosts = allPosts.Where(p => 
                p.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
        
        if (!string.IsNullOrEmpty(category))
            allPosts = allPosts.Where(p => p.Categories.Contains(category));
        
        Posts = allPosts.Select(p => new PostSummary
        {
            Slug = p.Slug,
            Title = p.Title,
            PublicationDate = p.PublicationDate,
            Categories = p.Categories
        }).ToList();
    }
}
```

**EditPost.cshtml.cs**
```csharp
[Authorize]
public class EditPostModel : PageModel
{
    private readonly IPostEditorService _editorService;
    
    [BindProperty]
    public PostEditorModel Post { get; set; }
    
    public async Task<IActionResult> OnGetAsync(string slug)
    {
        var post = await _editorService.GetPostForEditing(slug);
        if (post == null) return NotFound();
        
        Post = new PostEditorModel
        {
            Slug = post.Slug,
            Title = post.Title,
            Author = post.Author,
            Description = post.Description,
            Categories = string.Join(", ", post.Categories),
            Content = post.Content,
            PublicationDate = post.PublicationDate,
            LastModified = post.LastModified
        };
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();
        
        var success = await _editorService.SavePost(Post);
        if (success)
        {
            TempData["Message"] = "Post saved successfully!";
            return RedirectToPage("/Admin/Dashboard");
        }
        
        ModelState.AddModelError("", "Failed to save post");
        return Page();
    }
    
    public async Task<IActionResult> OnPostUploadImageAsync(IFormFile file)
    {
        var imagePath = await _editorService.UploadImage(Post.Slug, file);
        return new JsonResult(new { path = imagePath });
    }
}
```

#### Markdown Editor UI (EditPost.cshtml)

```html
@page "/admin/edit/{slug}"
@model EditPostModel
@{
    ViewData["Title"] = "Edit Post";
    Layout = "_AdminLayout";
}

<div class="container-fluid">
    <div class="row">
        <div class="col-12">
            <h2>Edit Post: @Model.Post.Title</h2>
        </div>
    </div>
    
    <form method="post">
        <!-- YAML Front Matter Section -->
        <div class="card mb-3">
            <div class="card-header">Post Metadata</div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-6">
                        <div class="mb-3">
                            <label asp-for="Post.Title" class="form-label">Title</label>
                            <input asp-for="Post.Title" class="form-control" />
                            <span asp-validation-for="Post.Title" class="text-danger"></span>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="mb-3">
                            <label asp-for="Post.Slug" class="form-label">Slug</label>
                            <input asp-for="Post.Slug" class="form-control" readonly />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="mb-3">
                            <label asp-for="Post.Author" class="form-label">Author</label>
                            <input asp-for="Post.Author" class="form-control" />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="mb-3">
                            <label asp-for="Post.PublicationDate" class="form-label">Publication Date</label>
                            <input asp-for="Post.PublicationDate" type="datetime-local" class="form-control" />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="mb-3">
                            <label asp-for="Post.Categories" class="form-label">Categories (comma-separated)</label>
                            <input asp-for="Post.Categories" class="form-control" />
                        </div>
                    </div>
                </div>
                <div class="mb-3">
                    <label asp-for="Post.Description" class="form-label">Description</label>
                    <textarea asp-for="Post.Description" class="form-control" rows="2"></textarea>
                </div>
            </div>
        </div>
        
        <!-- Markdown Editor Section -->
        <div class="card">
            <div class="card-header d-flex justify-content-between align-items-center">
                <span>Content</span>
                <div class="btn-toolbar" role="toolbar">
                    <div class="btn-group btn-group-sm me-2" role="group">
                        <button type="button" class="btn btn-outline-secondary" id="btn-bold" title="Bold">
                            <i class="bi bi-type-bold"></i>
                        </button>
                        <button type="button" class="btn btn-outline-secondary" id="btn-italic" title="Italic">
                            <i class="bi bi-type-italic"></i>
                        </button>
                        <button type="button" class="btn btn-outline-secondary" id="btn-link" title="Link">
                            <i class="bi bi-link"></i>
                        </button>
                        <button type="button" class="btn btn-outline-secondary" id="btn-image" title="Image">
                            <i class="bi bi-image"></i>
                        </button>
                        <button type="button" class="btn btn-outline-secondary" id="btn-code" title="Code">
                            <i class="bi bi-code"></i>
                        </button>
                    </div>
                    <button type="button" class="btn btn-sm btn-outline-primary" id="btn-upload">
                        <i class="bi bi-upload"></i> Upload Image
                    </button>
                </div>
            </div>
            <div class="card-body p-0">
                <div class="row g-0">
                    <div class="col-md-6 border-end">
                        <textarea asp-for="Post.Content" id="markdown-editor" 
                                  class="form-control border-0" 
                                  style="min-height: 600px; font-family: monospace; resize: none;"></textarea>
                    </div>
                    <div class="col-md-6">
                        <div id="markdown-preview" class="p-3" style="min-height: 600px; overflow-y: auto;"></div>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="mt-3 d-flex justify-content-between">
            <a asp-page="/Admin/Dashboard" class="btn btn-secondary">Cancel</a>
            <div>
                <button type="submit" class="btn btn-primary">Save Changes</button>
                <a asp-page="/Posts" asp-route-slug="@Model.Post.Slug" class="btn btn-outline-primary" target="_blank">
                    Preview Post
                </a>
            </div>
        </div>
    </form>
</div>

@section Scripts {
    <script src="~/admin/js/markdown-editor.js"></script>
    <script>
        // Initialize live preview
        const editor = new MarkdownEditor('markdown-editor', 'markdown-preview');
        editor.init();
    </script>
}
```

#### JavaScript for Markdown Editor (markdown-editor.js)

```javascript
class MarkdownEditor {
    constructor(editorId, previewId) {
        this.editor = document.getElementById(editorId);
        this.preview = document.getElementById(previewId);
        this.debounceTimer = null;
    }
    
    init() {
        // Live preview with debounce
        this.editor.addEventListener('input', () => {
            clearTimeout(this.debounceTimer);
            this.debounceTimer = setTimeout(() => this.updatePreview(), 300);
        });
        
        // Toolbar buttons
        document.getElementById('btn-bold').addEventListener('click', () => 
            this.wrapSelection('**', '**'));
        document.getElementById('btn-italic').addEventListener('click', () => 
            this.wrapSelection('*', '*'));
        document.getElementById('btn-link').addEventListener('click', () => 
            this.insertLink());
        document.getElementById('btn-image').addEventListener('click', () => 
            this.insertImage());
        document.getElementById('btn-code').addEventListener('click', () => 
            this.wrapSelection('`', '`'));
        
        // Initial preview
        this.updatePreview();
        
        // Auto-save every 30 seconds
        setInterval(() => this.autoSave(), 30000);
    }
    
    async updatePreview() {
        try {
            const markdown = this.editor.value;
            const response = await fetch('/api/preview-markdown', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ markdown })
            });
            
            if (!response.ok) {
                throw new Error('Preview generation failed');
            }
            
            const html = await response.text();
            // Server sanitizes HTML using HtmlAgilityPack before returning
            // Additional CSP headers should be configured in Program.cs
            this.preview.innerHTML = html;
            
            // Apply syntax highlighting to code blocks
            this.preview.querySelectorAll('pre code').forEach(block => {
                hljs.highlightElement(block);
            });
        } catch (error) {
            console.error('Failed to update preview:', error);
            this.preview.textContent = 'Preview unavailable. Please check your connection.';
        }
    }
    
    wrapSelection(before, after) {
        const start = this.editor.selectionStart;
        const end = this.editor.selectionEnd;
        const text = this.editor.value;
        const selection = text.substring(start, end);
        
        this.editor.value = 
            text.substring(0, start) + 
            before + selection + after + 
            text.substring(end);
        
        this.editor.focus();
        this.editor.setSelectionRange(
            start + before.length, 
            end + before.length
        );
        
        this.updatePreview();
    }
    
    insertText(text) {
        const start = this.editor.selectionStart;
        const value = this.editor.value;
        this.editor.value = 
            value.substring(0, start) + 
            text + 
            value.substring(start);
        this.editor.focus();
        this.editor.setSelectionRange(start + text.length, start + text.length);
        this.updatePreview();
    }
    
    insertLink() {
        // Note: In production, use custom modal dialogs instead of prompt()
        // for better UX and accessibility
        const url = prompt('Enter URL:');
        if (url) {
            const text = prompt('Enter link text:', url);
            this.wrapSelection(`[${text}](`, `${url})`);
        }
    }
    
    insertImage() {
        // Note: In production, use custom modal dialogs instead of prompt()
        // for better UX and accessibility
        const url = prompt('Enter image path (e.g., media/image.png):');
        if (url) {
            const alt = prompt('Enter alt text:', 'image');
            this.insertText(`![${alt}](${url})`);
        }
    }
    
    async autoSave() {
        try {
            const data = new FormData(this.editor.closest('form'));
            const response = await fetch(window.location.href, {
                method: 'POST',
                body: data,
                headers: { 'X-Auto-Save': 'true' }
            });
            
            if (response.ok) {
                console.log('Auto-saved at', new Date().toLocaleTimeString());
            } else {
                console.error('Auto-save failed:', response.statusText);
                // In production, show user notification
            }
        } catch (error) {
            console.error('Auto-save error:', error);
            // In production, show user notification that auto-save failed
        }
    }
}
```

### Configuration

Add admin settings to `appsettings.json`:

**GitHub OAuth Configuration (Recommended)**

```json
{
  "downr": {
    "title": "downr",
    "rootUrl": "https://localhost:5001",
    "pageSize": 4,
    "siteMode": "Blog",
    "admin": {
      "enabled": true,
      "authProvider": "github",
      "github": {
        "clientId": "[FROM_ENVIRONMENT]",
        "clientSecret": "[FROM_ENVIRONMENT]",
        "allowedUsers": "yourgithubusername,anothergithubuser"
      },
      "sessionTimeoutMinutes": 480
    }
  }
}
```

**Environment Variables for GitHub OAuth:**
```bash
# Set these in your environment or hosting platform
export DOWNR__ADMIN__GITHUB__CLIENTID="your-github-oauth-app-client-id"
export DOWNR__ADMIN__GITHUB__CLIENTSECRET="your-github-oauth-app-client-secret"
```

**Setting up GitHub OAuth App:**
1. Go to GitHub Settings → Developer settings → OAuth Apps
2. Create a new OAuth App
3. Set Authorization callback URL to: `https://yourdomain.com/signin-github`
4. Copy Client ID and Client Secret to environment variables
5. Add your GitHub username to `allowedUsers` in configuration

**Static Username/Password Configuration**

```json
{
  "downr": {
    "title": "downr",
    "rootUrl": "https://localhost:5001",
    "pageSize": 4,
    "siteMode": "Blog",
    "admin": {
      "enabled": true,
      "authProvider": "static",
      "static": {
        "username": "[FROM_ENVIRONMENT]",
        "passwordHash": "[FROM_ENVIRONMENT]"
      },
      "sessionTimeoutMinutes": 480
    }
  }
}
```

**Environment Variables for Static Auth:**
```bash
# IMPORTANT: Store the BCrypt hash, NOT the plaintext password
# Generate hash using: dotnet run --project HashPassword -- "your-password"
# Or use an online BCrypt generator with cost factor 12

export DOWNR_ADMIN_USERNAME="admin"
export DOWNR_ADMIN_PASSWORD_HASH='$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5sPQ3DjG3vuJm'

# Example C# code to generate hash:
# var hash = BCrypt.Net.BCrypt.HashPassword("your-password", 12);
# Console.WriteLine(hash);
```

**Helper Script for Generating Password Hash:**
```csharp
// Create a small console app or use dotnet-script
// dotnet new console -n HashPassword
// cd HashPassword
// dotnet add package BCrypt.Net-Next
// Replace Program.cs with:

using BCrypt.Net;

if (args.Length == 0)
{
    Console.WriteLine("Usage: dotnet run -- <password>");
    return;
}

var password = args[0];
var hash = BCrypt.HashPassword(password, 12);
Console.WriteLine($"BCrypt Hash (cost 12):\n{hash}");
Console.WriteLine($"\nSet environment variable:");
Console.WriteLine($"export DOWNR_ADMIN_PASSWORD_HASH='{hash}'");
```

### Security Considerations

1. **Secrets Management**: **Never store credentials in appsettings.json**
   - Use Azure Key Vault, AWS Secrets Manager, or HashiCorp Vault for production
   - Use environment variables for local development
   - GitHub OAuth secrets: Store Client ID and Client Secret in environment variables
   - Static auth: Store username and password in environment variables
   - Example: `builder.Configuration.AddAzureKeyVault()`

2. **GitHub OAuth Security** (When using GitHub authentication)
   - **User Validation**: Always validate authorized users via `allowedUsers` list
   - **Scope Limitations**: Only request necessary OAuth scopes (`user:email` is sufficient for auth)
   - **Token Storage**: Access tokens stored securely in encrypted cookies
   - **Callback URL**: Ensure callback URL matches OAuth app configuration exactly
   - **State Validation**: ASP.NET Core OAuth middleware handles CSRF state validation automatically
   - **Future Scopes**: When adding GitHub repo features, add `repo` scope only then

3. **Password Hashing** (When using static authentication)
   - **Store BCrypt hashes in environment variables, NEVER plaintext passwords**
   - Use BCrypt with cost factor 12 or higher
   - Generate hash once using helper script or BCrypt tool
   - Store hash in `DOWNR_ADMIN_PASSWORD_HASH` environment variable
   - Validate hash format on application startup (must start with $2a$, $2b$, or $2y$)
   - Never hash passwords on every application startup (security and performance issue)

4. **CSRF Protection**: ASP.NET Core's built-in anti-forgery tokens
   - Automatically validated for POST requests
   - Include `@Html.AntiForgeryToken()` in all forms
   - OAuth flow includes state parameter for CSRF protection

5. **Rate Limiting**: Limit login attempts to prevent brute force
   - Use ASP.NET Core rate limiting middleware (.NET 10)
   - Apply rate limiting to `/admin/login` endpoint
   - Implement exponential backoff for failed attempts
   - GitHub OAuth has built-in rate limiting

6. **HTTPS Only**: Require HTTPS for admin pages
   - Configure HSTS headers
   - Redirect HTTP to HTTPS
   - Use `[RequireHttps]` attribute on admin pages
   - GitHub OAuth requires HTTPS callback URLs in production

7. **Input Validation**: Validate all user input (file uploads, markdown content)
   - Server-side validation is mandatory
   - Whitelist allowed characters in slugs
   - Sanitize file names
   - Validate GitHub usernames against allowed list

8. **File Upload Restrictions**: Whitelist allowed file types and sizes
   - Verify file content, not just extension
   - Limit file sizes (e.g., 5MB max)
   - Store uploads outside wwwroot if possible

9. **Content Security Policy**: Implement strict CSP headers to mitigate XSS risks
   ```csharp
   // Program.cs - Production-ready CSP with proper nonce generation
   app.Use(async (context, next) => {
       if (context.Request.Path.StartsWithSegments("/admin")) {
           // Generate cryptographically secure nonce
           byte[] nonceBytes = new byte[32];
           using (var rng = RandomNumberGenerator.Create())
           {
               rng.GetBytes(nonceBytes);
           }
           var nonce = Convert.ToBase64String(nonceBytes);
           
           context.Items["csp-nonce"] = nonce;
           context.Response.Headers.Add("Content-Security-Policy", 
               $"default-src 'self'; script-src 'self' 'nonce-{nonce}'; " +
               $"style-src 'self' 'nonce-{nonce}'; img-src 'self' data: https://avatars.githubusercontent.com; " +
               $"font-src 'self'; connect-src 'self' https://github.com; frame-ancestors 'none'");
       }
       await next();
   });
   ```

10. **HTML Sanitization**: Multi-layer defense against XSS
    - Server-side sanitization using HtmlAgilityPack or AngleSharp
    - Consider using DOMPurify on client-side as second layer
    - Validate against known-good patterns

11. **Accessibility**: Implement proper UI patterns
    - Replace `prompt()` with custom modal dialogs
    - Use ARIA labels and roles
    - Ensure keyboard navigation works correctly
    - Test with screen readers

12. **Error Handling**: Provide user-facing feedback
    - Show clear error messages for failed operations
    - Log detailed errors server-side only
    - Never expose stack traces or internal paths to users

13. **Authorization**: Additional checks beyond authentication
    - Verify user is in `allowedUsers` list (GitHub OAuth)
    - Use `[Authorize]` attribute on all admin pages
    - Check authorization on every admin API endpoint
    - Log all admin actions for audit trail

### Benefits of Admin Section

✅ **Accessibility**: Edit posts from any device with a browser  
✅ **Ease of Use**: No need for Git knowledge or VS Code  
✅ **Live Preview**: See rendered output while editing  
✅ **Reduced Errors**: Form validation for metadata  
✅ **Faster Workflow**: No commit/push/pull cycle (when using filesystem)  
✅ **Image Management**: Upload images directly through UI  
✅ **Auto-Save**: Prevent accidental data loss  
✅ **GitHub Integration**: Native OAuth authentication with GitHub account  
✅ **Future-Ready**: Foundation for GitHub repository integration features  

### GitHub Integration Roadmap

The GitHub OAuth authentication lays the groundwork for powerful future features:

**Phase 1 (Current)**: GitHub OAuth Authentication
- Authenticate using your GitHub account
- Validate authorized users via GitHub username
- Store GitHub access token for future use

**Phase 2 (Future)**: Git Commit & Push from Admin UI
- Save posts and automatically commit to GitHub repository
- Generate meaningful commit messages (e.g., "Update post: post-title")
- Push changes directly to your blog's GitHub repo
- View commit history in admin dashboard

**Phase 3 (Future)**: GitHub Repository as Storage
- Pull latest posts from GitHub repository on startup
- Sync local file system with GitHub repo
- Enable collaboration: multiple authors can commit via GitHub
- Content versioning through Git history

**Phase 4 (Future)**: Advanced GitHub Features
- Create pull requests from admin UI for draft posts
- Review and approve posts via GitHub PR workflow
- Branch management for different environments (staging/production)
- Webhook integration for automatic deployments

**Implementation Notes:**
- Access token saved during OAuth flow enables GitHub API calls
- `SaveTokens = true` in OAuth configuration stores access token
- Future `PostEditorService` can use `Octokit` library for GitHub operations
- Example future code:
  ```csharp
  public async Task<bool> SavePostToGitHub(PostEditorModel model)
  {
      var accessToken = await HttpContext.GetTokenAsync("access_token");
      var github = new GitHubClient(new ProductHeaderValue("downr"))
      {
          Credentials = new Credentials(accessToken)
      };
      
      // Create/update file in repository
      // Generate commit message
      // Push to GitHub
  }
  ```

### Optional vs. Default

The admin section should be **optional** and disabled by default:
- Existing users can continue using Git + VS Code workflow
- New users can opt-in to admin section via configuration
- Both workflows can coexist (admin UI + Git commits)
- Admin UI respects the same file structure
- **Authentication provider is configurable**: GitHub OAuth or static credentials

## Migration Strategy

### Phase 1: Project Setup (Week 1)
1. Create new .NET 10 project with `dotnet new razor`
2. Copy existing models and services to new project
3. Update all package references to .NET 10 compatible versions
4. Set up new project structure

### Phase 2: Core Services Migration (Week 1-2)
1. Migrate content indexing services (FileSystem and Azure Storage)
2. Update PostService to work without API layer
3. Migrate PostFileParser and PostFileSorter
4. Add background worker for auto-refresh functionality
5. Update DownrOptions and configuration system

### Phase 3: Page Creation (Week 2-3)
1. **Index Page** (`/`)
   - Blog mode: Display paginated post list
   - Workshop mode: Display phase listing
   - Implement "Load More" with query string pagination
   
2. **Post Page** (`/posts/{slug}`)
   - Display individual post with rendered Markdown
   - Previous/Next navigation
   - Phase/Step display for workshop mode
   
3. **Category Page** (`/category/{name}`)
   - List posts in category
   - Useful for blog mode and workshop phases

4. **Shared Components**
   - Layout with header/footer
   - Partial views for post cards
   - Navigation components

### Phase 4: Feed Support (Week 3)
1. Keep FeedController for RSS feed generation
2. Ensure RSS feed works with new architecture
3. Add feed auto-discovery tags in layout

### Phase 5: Styling and Assets (Week 3-4)
1. Migrate CSS (remove Blazor-specific styles)
2. Update to use modern CSS features (CSS Grid, Container Queries)
3. Add minimal JavaScript only for:
   - Syntax highlighting (Highlight.js)
   - Google Analytics (if configured)
4. Optimize images and static assets

### Phase 6: Storage Adapters (Week 4)
1. Ensure FileSystem storage works correctly
2. Update Azure Storage implementation for new SDK
3. Test both storage modes thoroughly

### Phase 7: Admin Section (Optional) (Week 5-6)
1. **Authentication**
   - Implement ASP.NET Core Identity with cookie authentication
   - Create login/logout pages
   - Add admin configuration options

2. **Dashboard**
   - Post listing with search and filter
   - Quick actions (edit, delete, preview)
   - Statistics overview

3. **Post Editor**
   - Markdown editor with live preview
   - YAML front matter form
   - Syntax highlighting integration
   - Toolbar for common Markdown operations

4. **Image Management**
   - File upload functionality
   - Image browser
   - Drag-and-drop support

5. **PostEditorService**
   - CRUD operations for posts
   - File system operations
   - Content re-indexing after saves

6. **Security**
   - CSRF protection
   - Rate limiting for login attempts
   - Input validation and sanitization
   - File upload restrictions

### Phase 8: Testing and Documentation (Week 6-7)
1. Manual testing of all pages and modes
2. Test both Blog and Workshop modes
3. Test pagination
4. Test both storage modes
5. Test admin section (if enabled)
6. Update README.md with new architecture details
7. Create migration guide for existing users
8. Document admin section setup and usage

## Implementation Details

### Page Models Structure

#### Index.cshtml.cs
```csharp
public class IndexModel : PageModel
{
    private readonly PostService _postService;
    private readonly DownrOptions _options;

    public List<Post> Posts { get; set; }
    public List<string> Categories { get; set; }
    public int CurrentPage { get; set; } = 0;
    public bool HasMore { get; set; }

    public IndexModel(PostService postService, IOptions<DownrOptions> options)
    {
        _postService = postService;
        _options = options.Value;
    }

    public void OnGet(int page = 0)
    {
        CurrentPage = page;
        var pageSize = _options.PageSize;
        
        if (_options.SiteMode == SiteMode.Blog)
        {
            var totalToLoad = (page + 1) * pageSize;
            Posts = _postService.GetPosts(0, totalToLoad).ToList();
            HasMore = _postService.GetNumberOfPosts() > totalToLoad;
        }
        else // Workshop mode
        {
            Categories = _postService.GetCategories().ToList();
        }
    }
}
```

#### Post.cshtml.cs
```csharp
public class PostModel : PageModel
{
    private readonly PostService _postService;
    private readonly DownrOptions _options;

    public Post Post { get; set; }
    public Post NextPost { get; set; }
    public Post PreviousPost { get; set; }

    public PostModel(PostService postService, IOptions<DownrOptions> options)
    {
        _postService = postService;
        _options = options.Value;
    }

    public IActionResult OnGet(string slug)
    {
        Post = _postService.GetPostBySlug(slug);
        
        if (Post == null)
        {
            return NotFound();
        }

        var (previous, next) = _postService.GetPreviousAndNextPosts(slug);
        PreviousPost = previous;
        NextPost = next;

        return Page();
    }
}
```

### Routing Configuration

Use conventional routing and route templates:
- `/` → Index page (blog/workshop home)
- `/posts/{slug}` → Individual post page
- `/category/{name}` → Category listing page
- `/feed/rss` → RSS feed controller

### Service Registration (Program.cs)

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages
builder.Services.AddRazorPages();

// Add downr services
builder.Services.AddDownr(builder.Configuration)
    .WithWebServerFileSystemStorage();
    // or .WithAzureStorage();

// Add response compression
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "image/svg+xml", "application/font-woff2" });
});

// Add output caching (available in .NET 10)
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder => builder.Cache());
});

var app = builder.Build();

// Configure middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx => 
        ctx.Context.Response.Headers["Cache-Control"] = "public,max-age=604800"
});

app.UseRouting();
app.UseResponseCompression();
app.UseOutputCache();

app.MapRazorPages();
app.MapControllers(); // For RSS feed

// Initialize downr
app.UseDownr()
    .WithWebServerFileSystemStorage();
    // or .WithAzureStorage();

app.Run();
```

### Pagination Implementation

Instead of client-side "load more" functionality:

**Option 1: Query String Pagination**
```
/?page=0  (shows first N posts)
/?page=1  (shows first 2N posts)
```

**Option 2: Traditional Pagination**
```
/?page=1  (shows posts 1-N)
/?page=2  (shows posts N+1-2N)
```

Recommendation: Use Option 1 to maintain similar UX to current implementation.

## Breaking Changes and Migration Path

### For Users
1. **No JavaScript Required**: Site works without JavaScript (syntax highlighting won't work)
2. **URL Structure**: Remains identical (`/posts/{slug}`)
3. **Configuration**: appsettings.json format remains the same
4. **Content Files**: No changes to Markdown files or structure

### For Developers/Contributors
1. **No More Blazor**: All UI code is now server-side Razor Pages
2. **No API Controllers**: Data accessed directly from page models
3. **Single Project**: Client/Server/Shared collapsed into one
4. **Different Patterns**: Blazor component lifecycle replaced with Razor Page lifecycle

## Risks and Mitigations

### Risk 1: Loss of SPA Feel
**Mitigation**: Implement progressive enhancement with HTMX or minimal JavaScript for smooth page transitions if desired. However, blog sites typically don't need SPA behavior.

### Risk 2: Server Load
**Mitigation**: Implement output caching for rendered pages. Static content like blog posts cache extremely well.

### Risk 3: Breaking Existing Deployments
**Mitigation**: 
- Provide clear migration guide
- Keep same configuration format
- Same content file structure
- Tag old version before migration

### Risk 4: Feature Parity
**Mitigation**: Ensure all current features are maintained:
- Blog and Workshop modes
- Pagination
- Categories
- RSS feed
- Auto-refresh
- Azure Storage support
- Google Analytics

## Performance Expectations

### Current (Blazor WebAssembly)
- **First Load**: ~3-5 seconds (download assemblies)
- **Navigation**: Instant (client-side routing)
- **Bundle Size**: ~2MB+ (compressed)

### Proposed (Razor Pages)
- **First Load**: ~300-500ms (HTML only)
- **Navigation**: ~100-300ms (server round-trip with caching)
- **HTML Size**: ~50-100KB per page

**Overall**: 60-80% improvement in perceived performance for first-time visitors.

## Testing Strategy

### Unit Tests
- Service layer tests (PostService, ContentIndexer, etc.)
- Model validation tests
- Configuration parsing tests

### Integration Tests
- Page rendering tests
- Storage adapter tests (FileSystem and Azure)
- Background worker tests

### Manual Testing
- **Blog Mode**: Create sample blog posts, test pagination
- **Workshop Mode**: Create phased content, test navigation
- **Storage Modes**: Test both FileSystem and Azure Storage
- **RSS Feed**: Verify feed generation
- **Mobile**: Test responsive design
- **Accessibility**: Test with screen readers

## Timeline and Milestones

### Sprint 1 (Week 1)
- [ ] Project setup and migration
- [ ] Core services migration
- [ ] Configuration system

### Sprint 2 (Week 2)
- [ ] Index page implementation
- [ ] Post page implementation
- [ ] Basic styling

### Sprint 3 (Week 3)
- [ ] Category pages
- [ ] RSS feed migration
- [ ] Complete styling

### Sprint 4 (Week 4)
- [ ] Storage adapters
- [ ] Background workers
- [ ] Performance optimization

### Sprint 5 (Week 5)
- [ ] Testing core features
- [ ] Documentation
- [ ] Migration guide
- [ ] Performance optimization

### Sprint 6 (Week 6) - Admin Section (Optional)
- [ ] Authentication and authorization
- [ ] Admin dashboard page
- [ ] Post editor with live preview
- [ ] Image upload functionality
- [ ] PostEditorService implementation

### Sprint 7 (Week 7) - Finalization
- [ ] Admin section testing
- [ ] Security review
- [ ] Complete documentation
- [ ] Release preparation

### Sprint 8 (Optional) - Aspire Integration
- [ ] Create AppHost and ServiceDefaults projects
- [ ] Configure Aspire orchestration
- [ ] Add Azure Storage emulator support
- [ ] Setup OpenTelemetry and observability
- [ ] Test local development with Aspire dashboard
- [ ] Document Aspire setup and usage
- [ ] Verify standalone deployment still works

**Note**: Aspire integration is optional and doesn't block release. Can be added post-launch for developers who want enhanced local development experience.

## Package Updates

### Packages to Remove
- `Microsoft.AspNetCore.Components.WebAssembly`
- `Microsoft.AspNetCore.Components.WebAssembly.Server`
- `Microsoft.AspNetCore.Components.WebAssembly.Build`
- `Microsoft.AspNetCore.Components.WebAssembly.DevServer`
- `Blazored.LocalStorage`
- `System.Net.Http.Json` (built into .NET 10)

### Packages to Update
- `Markdig` → Latest stable version
- `YamlDotNet` → Replace `YamlDotNet.NetCore` with `YamlDotNet`
- `Azure.Storage.Blobs` → Latest stable version
- `HtmlAgilityPack` → Latest stable version

### Packages to Add (for Admin Section)
- `AspNet.Security.OAuth.GitHub` → For GitHub OAuth authentication
- `BCrypt.Net-Next` → For static password hashing (optional)
- `Bootstrap Icons` → For admin UI icons
- `Octokit` (future) → For GitHub API integration (commit/push features)

**Authentication Provider Packages:**
- **GitHub OAuth**: `AspNet.Security.OAuth.GitHub` (required)
- **Static Auth**: `BCrypt.Net-Next` (required only if using static auth)

### Packages to Add (for Aspire Support - Optional)

**downr.AppHost Project:**
- `Aspire.Hosting` → Core Aspire orchestration
- `Aspire.Hosting.Azure.Storage` → Azure Storage emulator support
- `Aspire.Hosting.Redis` (optional) → Redis caching support

**downr.ServiceDefaults Project:**
- `Microsoft.Extensions.Http.Resilience` → Retry and circuit breaker patterns
- `Microsoft.Extensions.ServiceDiscovery` → Service discovery
- `OpenTelemetry.Exporter.OpenTelemetryProtocol` → Telemetry export
- `OpenTelemetry.Extensions.Hosting` → OpenTelemetry hosting integration
- `OpenTelemetry.Instrumentation.AspNetCore` → ASP.NET Core metrics
- `OpenTelemetry.Instrumentation.Http` → HTTP client metrics
- `OpenTelemetry.Instrumentation.Runtime` → .NET runtime metrics

**downr.Web Project (Development Only):**
- `Aspire.Microsoft.Extensions.ServiceDiscovery` → Service discovery client
- `Microsoft.Extensions.Http.Resilience` → Resilience patterns
- Reference to `downr.ServiceDefaults` project (Debug configuration only)

**Note**: Aspire packages are only required if you want to use the Aspire dashboard for local development. The application runs perfectly without them.

### Packages to Keep
- `Microsoft.AspNetCore.ResponseCompression`
- Core ASP.NET packages (automatic with .NET 10)

## Configuration Changes

Minimal changes required. Current `appsettings.json` structure remains compatible, with optional admin section:

**Example 1: GitHub OAuth Authentication (Recommended)**

```json
{
  "downr": {
    "title": "downr",
    "rootUrl": "https://localhost:5001",
    "pageSize": 4,
    "author": "author name",
    "indexPageText": "my blog",
    "imagePathFormat": "/posts/{0}/media/",
    "autoRefreshInterval": 0,
    "googleTrackingCode": "",
    "siteMode": "Blog",
    "showTopMostTitleBar": true,
    "showCategoryMenu": true,
    "showPhaseLabels": true,
    "admin": {
      "enabled": false,
      "authProvider": "github",
      "github": {
        "clientId": "[FROM_ENVIRONMENT]",
        "clientSecret": "[FROM_ENVIRONMENT]",
        "allowedUsers": "yourgithubusername"
      },
      "sessionTimeoutMinutes": 480,
      "maxImageUploadSizeMB": 5,
      "allowedImageExtensions": [".jpg", ".jpeg", ".png", ".gif", ".webp"]
    }
  },
  "downr.AzureStorage": {
    "ConnectionString": "",
    "Container": "posts"
  }
}
```

**Example 2: Static Username/Password Authentication**

```json
{
  "downr": {
    "title": "downr",
    "rootUrl": "https://localhost:5001",
    "pageSize": 4,
    "author": "author name",
    "indexPageText": "my blog",
    "imagePathFormat": "/posts/{0}/media/",
    "autoRefreshInterval": 0,
    "googleTrackingCode": "",
    "siteMode": "Blog",
    "showTopMostTitleBar": true,
    "showCategoryMenu": true,
    "showPhaseLabels": true,
    "admin": {
      "enabled": false,
      "authProvider": "static",
      "static": {
        "username": "[FROM_ENVIRONMENT]",
        "passwordHash": "[FROM_ENVIRONMENT]"
      },
      "sessionTimeoutMinutes": 480,
      "maxImageUploadSizeMB": 5,
      "allowedImageExtensions": [".jpg", ".jpeg", ".png", ".gif", ".webp"]
    }
  },
  "downr.AzureStorage": {
    "ConnectionString": "",
    "Container": "posts"
  }
}
```

**Notes**: 
- Admin section is disabled by default
- Authentication provider is configurable: `"github"` or `"static"`
- Secrets (OAuth keys, passwords) should come from environment variables, not appsettings.json
- To swap auth providers, simply change `authProvider` value and provide appropriate configuration

## SEO Enhancements (Bonus)

With server-side rendering, we can add:

1. **Meta Tags**: Proper Open Graph and Twitter Card tags
2. **Structured Data**: JSON-LD for articles
3. **Sitemap**: Auto-generated XML sitemap
4. **Robots.txt**: Proper robots.txt file

## Accessibility Improvements

1. **Semantic HTML**: Proper heading hierarchy
2. **ARIA Labels**: Where needed for screen readers
3. **Keyboard Navigation**: Ensure all interactive elements are keyboard accessible
4. **Color Contrast**: Meet WCAG AA standards
5. **Alt Text**: Ensure all images have descriptive alt text

## Conclusion

Rewriting downr in .NET 10 with Razor Pages offers significant advantages:

✅ **Better Performance**: Faster initial load, better perceived performance  
✅ **Improved SEO**: Full HTML delivered to search engines  
✅ **Simplified Architecture**: Single project, no API layer  
✅ **Modern Framework**: .NET 10 with latest features  
✅ **Better Compatibility**: Works without JavaScript  
✅ **Easier Maintenance**: Standard ASP.NET Core patterns  
✅ **Future-Proof**: LTS framework with long-term support  
✅ **Enhanced Workflow**: Optional admin section for browser-based editing  
✅ **Developer Experience**: Optional .NET Aspire support for local development  
✅ **Flexible Deployment**: Simple App Service OR advanced Container Apps  

The migration is straightforward, maintains backward compatibility for content and configuration, and provides a solid foundation for the next 5+ years of downr development.

### Key Enhancements

1. **Core Migration**: Move from Blazor WebAssembly to server-rendered Razor Pages
2. **Admin Section** (Optional): Web-based Markdown editor with live preview, making downr accessible to users who prefer not to use Git/VS Code
3. **Flexible Authentication**: 
   - **GitHub OAuth** (recommended): Natural fit for developers, enables future GitHub integration
   - **Static credentials**: Simple alternative for air-gapped or single-user deployments
   - Easy to swap between providers via configuration
4. **Future GitHub Integration**: Foundation for committing/pushing posts directly to GitHub repository
5. **Dual Workflow Support**: Continue using Git + VS Code, or use the new admin UI, or both
6. **.NET Aspire Integration** (Optional):
   - Enhanced local development with dashboard, logs, and telemetry
   - Easy service integration (storage, databases, caching)
   - Simple testing with Azure emulators
   - Optional advanced deployment to Container Apps
   - Graceful degradation: works perfectly without Aspire

## Next Steps

1. **Community Feedback**: Gather feedback on this proposal
2. **GitHub OAuth Setup**: Create GitHub OAuth App and test authentication flow
3. **Prototype**: Create proof-of-concept with Index, Post pages, and admin editor with GitHub auth
4. **Performance Testing**: Benchmark proposed vs current architecture
5. **Admin UI Mockups**: Design mockups for admin section
6. **Aspire Evaluation** (Optional): Test local development with Aspire dashboard
7. **Implementation**: Follow the migration strategy outlined above
8. **Documentation**: Update all documentation for new architecture
9. **GitHub Integration Planning**: Design workflow for future GitHub repository features
10. **Aspire Setup** (Optional): Add AppHost project for developers who want enhanced local dev experience
11. **Release**: Ship .NET 10 version as downr 4.0

---

**Proposal Version**: 1.3  
**Date**: January 2026  
**Author**: GitHub Copilot Agent  
**Status**: Draft for Review  
**Last Updated**: 
- Added admin section with online Markdown editor
- Changed authentication to GitHub OAuth (primary) with static credentials fallback
- Added GitHub integration roadmap for future features
- Added .NET Aspire support for enhanced local development (optional)
