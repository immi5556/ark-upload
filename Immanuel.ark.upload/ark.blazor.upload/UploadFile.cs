using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace ark.blazor.upload
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

    public class UploadFile : IAsyncDisposable
    {
        private Lazy<Task<IJSObjectReference>> moduleTask;
        IJSRuntime jsRuntime;
        public UploadFile(IJSRuntime jjsRuntime)
        {
            jsRuntime = jjsRuntime;
            //moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            //   "import", "./_content/ark.blazor.upload/upload.js").AsTask());
            //moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            //   "script", "./_content/ark.blazor.upload/upload.js").AsTask());
        }
        public void LoadTaskk()
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/ark.blazor.upload/upload.js").AsTask());
        }
        public async ValueTask<string> HandleFileSelect(object args)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("handleFileSelect", args);
        }
        public async ValueTask<string> TriggerInput(params object[] args)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("triggerInput", args);
        }
        public async Task Init(params object[] args)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("ark");
        }
        public async ValueTask<string> Prompt(string message)
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("showPrompt", message);
        }

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
