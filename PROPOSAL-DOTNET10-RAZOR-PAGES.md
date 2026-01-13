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
- Simple username/password authentication using ASP.NET Core Identity
- Cookie-based authentication for admin sessions
- Configurable admin credentials in appsettings.json
- Optional: Integration with Azure AD or other OAuth providers

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

```csharp
// Program.cs
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/admin/login";
        options.LogoutPath = "/admin/logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization();

// After app.UseRouting()
app.UseAuthentication();
app.UseAuthorization();
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

    public async Task<bool> SavePost(PostEditorModel model)
    {
        // 1. Validate model (slug uniqueness, required fields)
        // 2. Generate YAML front matter from model properties
        // 3. Combine YAML + Markdown content
        // 4. Write to disk: wwwroot/posts/{slug}/index.md
        // 5. Trigger content re-indexing
        // 6. Return success/failure
    }

    public async Task<string> UploadImage(string slug, IFormFile file)
    {
        // 1. Validate file type and size
        // 2. Create media folder if needed: wwwroot/posts/{slug}/media/
        // 3. Generate safe filename
        // 4. Save file to disk
        // 5. Return relative path: media/{filename}
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

```json
{
  "downr": {
    "title": "downr",
    "rootUrl": "https://localhost:5001",
    "pageSize": 4,
    "siteMode": "Blog",
    "admin": {
      "enabled": true,
      "username": "admin",
      "passwordHash": "$2a$11$...", // BCrypt hash
      "sessionTimeoutMinutes": 480
    }
  }
}
```

### Security Considerations

1. **Secrets Management**: **Never store credentials in appsettings.json**
   - Use Azure Key Vault, AWS Secrets Manager, or HashiCorp Vault
   - Use environment variables for local development
   - Example: `builder.Configuration.AddAzureKeyVault()`

2. **Password Hashing**: Use BCrypt or Argon2 for password hashing
   - Never store plain text passwords
   - Use strong work factors (BCrypt cost 12+, Argon2 recommended defaults)

3. **CSRF Protection**: ASP.NET Core's built-in anti-forgery tokens
   - Automatically validated for POST requests
   - Include `@Html.AntiForgeryToken()` in all forms

4. **Rate Limiting**: Limit login attempts to prevent brute force
   - Use ASP.NET Core rate limiting middleware (.NET 10)
   - Lock accounts after N failed attempts
   - Implement exponential backoff

5. **HTTPS Only**: Require HTTPS for admin pages
   - Configure HSTS headers
   - Redirect HTTP to HTTPS
   - Use `[RequireHttps]` attribute on admin pages

6. **Input Validation**: Validate all user input (file uploads, markdown content)
   - Server-side validation is mandatory
   - Whitelist allowed characters in slugs
   - Sanitize file names

7. **File Upload Restrictions**: Whitelist allowed file types and sizes
   - Verify file content, not just extension
   - Limit file sizes (e.g., 5MB max)
   - Store uploads outside wwwroot if possible

8. **Content Security Policy**: Implement strict CSP headers to mitigate XSS risks
   ```csharp
   // Program.cs - Production-ready CSP
   app.Use(async (context, next) => {
       if (context.Request.Path.StartsWithSegments("/admin")) {
           // Use nonces for inline scripts or move all to external files
           var nonce = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
           context.Items["csp-nonce"] = nonce;
           context.Response.Headers.Add("Content-Security-Policy", 
               $"default-src 'self'; script-src 'self' 'nonce-{nonce}'; " +
               $"style-src 'self' 'nonce-{nonce}'; img-src 'self' data:; " +
               $"font-src 'self'; connect-src 'self'; frame-ancestors 'none'");
       }
       await next();
   });
   ```

9. **HTML Sanitization**: Multi-layer defense against XSS
   - Server-side sanitization using HtmlAgilityPack or AngleSharp
   - Consider using DOMPurify on client-side as second layer
   - Validate against known-good patterns

10. **Accessibility**: Implement proper UI patterns
    - Replace `prompt()` with custom modal dialogs
    - Use ARIA labels and roles
    - Ensure keyboard navigation works correctly
    - Test with screen readers

11. **Error Handling**: Provide user-facing feedback
    - Show clear error messages for failed operations
    - Log detailed errors server-side only
    - Never expose stack traces or internal paths to users

### Benefits of Admin Section

✅ **Accessibility**: Edit posts from any device with a browser  
✅ **Ease of Use**: No need for Git knowledge or VS Code  
✅ **Live Preview**: See rendered output while editing  
✅ **Reduced Errors**: Form validation for metadata  
✅ **Faster Workflow**: No commit/push/pull cycle  
✅ **Image Management**: Upload images directly through UI  
✅ **Auto-Save**: Prevent accidental data loss  

### Optional vs. Default

The admin section should be **optional** and disabled by default:
- Existing users can continue using Git + VS Code workflow
- New users can opt-in to admin section via configuration
- Both workflows can coexist (admin UI + Git commits)
- Admin UI respects the same file structure

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
- `BCrypt.Net-Next` → For password hashing
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` (optional) → For advanced auth
- `Bootstrap Icons` → For admin UI icons

### Packages to Keep
- `Microsoft.AspNetCore.ResponseCompression`
- Core ASP.NET packages (automatic with .NET 10)

## Configuration Changes

Minimal changes required. Current `appsettings.json` structure remains compatible, with optional admin section:

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
      "username": "admin",
      "passwordHash": "",
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

**Note**: Admin section is disabled by default. Users who want to use the web-based editor can enable it by setting `admin.enabled` to `true` and configuring credentials.

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

The migration is straightforward, maintains backward compatibility for content and configuration, and provides a solid foundation for the next 5+ years of downr development.

### Key Enhancements

1. **Core Migration**: Move from Blazor WebAssembly to server-rendered Razor Pages
2. **Admin Section** (Optional): Web-based Markdown editor with live preview, making downr accessible to users who prefer not to use Git/VS Code
3. **Dual Workflow Support**: Continue using Git + VS Code, or use the new admin UI, or both

## Next Steps

1. **Community Feedback**: Gather feedback on this proposal
2. **Prototype**: Create proof-of-concept with Index, Post pages, and basic admin editor
3. **Performance Testing**: Benchmark proposed vs current architecture
4. **Admin UI Mockups**: Design mockups for admin section
5. **Implementation**: Follow the migration strategy outlined above
6. **Documentation**: Update all documentation for new architecture
7. **Release**: Ship .NET 10 version as downr 4.0

---

**Proposal Version**: 1.1  
**Date**: January 2026  
**Author**: GitHub Copilot Agent  
**Status**: Draft for Review  
**Last Updated**: Added admin section with online Markdown editor
