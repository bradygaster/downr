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
│   └── PostFileSorter.cs
├── Models/             # Domain models
│   ├── Post.cs
│   ├── PostListModel.cs
│   ├── PostPageModel.cs
│   └── DownrOptions.cs
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
    └── js/            # Minimal JavaScript (syntax highlighting only)
```

### Flow in New Architecture
1. **HTTP Request**: Browser requests page (e.g., `/post/my-post`)
2. **Razor Page Execution**: Server executes corresponding Razor Page
3. **Server-Side Rendering**: Page model loads data, HTML rendered on server
4. **HTML Response**: Complete HTML page sent to browser
5. **Progressive Enhancement**: Optional JavaScript for syntax highlighting

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

### Phase 7: Testing and Documentation (Week 4-5)
1. Manual testing of all pages and modes
2. Test both Blog and Workshop modes
3. Test pagination
4. Test both storage modes
5. Update README.md with new architecture details
6. Create migration guide for existing users

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
- ✅ Project setup and migration
- ✅ Core services migration
- ✅ Configuration system

### Sprint 2 (Week 2)
- ✅ Index page implementation
- ✅ Post page implementation
- ✅ Basic styling

### Sprint 3 (Week 3)
- ✅ Category pages
- ✅ RSS feed migration
- ✅ Complete styling

### Sprint 4 (Week 4)
- ✅ Storage adapters
- ✅ Background workers
- ✅ Performance optimization

### Sprint 5 (Week 5)
- ✅ Testing
- ✅ Documentation
- ✅ Migration guide
- ✅ Release preparation

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

### Packages to Keep
- `Microsoft.AspNetCore.ResponseCompression`
- Core ASP.NET packages (automatic with .NET 10)

## Configuration Changes

Minimal changes required. Current `appsettings.json` structure remains compatible:

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
    "showPhaseLabels": true
  },
  "downr.AzureStorage": {
    "ConnectionString": "",
    "Container": "posts"
  }
}
```

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

The migration is straightforward, maintains backward compatibility for content and configuration, and provides a solid foundation for the next 5+ years of downr development.

## Next Steps

1. **Community Feedback**: Gather feedback on this proposal
2. **Prototype**: Create proof-of-concept with Index and Post pages
3. **Performance Testing**: Benchmark proposed vs current architecture
4. **Implementation**: Follow the migration strategy outlined above
5. **Documentation**: Update all documentation for new architecture
6. **Release**: Ship .NET 10 version as downr 4.0

---

**Proposal Version**: 1.0  
**Date**: January 2026  
**Author**: GitHub Copilot Agent  
**Status**: Draft for Review
